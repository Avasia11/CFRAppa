using Microsoft.AspNetCore.Mvc;
using CFRApp.Data;
using CFRApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFRApp.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inițializare pop-up
        [HttpGet]
        public IActionResult Buy(int trainId, string from, string to, string departure)
        {
            var train = _context.Trains.FirstOrDefault(t => t.Id == trainId);
            if (train == null) return NotFound();

            var model = new BuyTicketViewModel
            {
                TrainId = train.Id,
                TrainNumber = train.Number,
                TrainType = train.Type,
                Operator = train.Operator,
                FromStation = from,
                ToStation = to,
                DepartureTime = TimeSpan.Parse(departure),
                Date = DateTime.Today,
                ReturnRoutes = new List<ReturnRouteOption>()
            };

            return PartialView("_BuyTicketPartial", model);
        }

        // GET: Returnează rutele inverse pentru data aleasă
        [HttpGet]
        public JsonResult GetReturnRoutes(string fromStation, string to, string date)
        {
            DateTime travelDate;
            if (!DateTime.TryParse(date, out travelDate))
                return Json(new List<ReturnRouteOption>());

            var routes = (from r1 in _context.Routes
                          join r2 in _context.Routes on r1.TrainId equals r2.TrainId
                          where r1.Station.Name == fromStation && r2.Station.Name == to
                                && r1.Order < r2.Order
                          select new
                          {
                              Train = r1.Train,
                              Departure = r1.DepartureTime,
                              Arrival = r2.ArrivalTime
                          }).Distinct().ToList();

            var result = routes.Select(r => new ReturnRouteOption
            {
                TrainId = r.Train.Id,
                TrainNumber = r.Train.Number,
                DepartureTime = r.Departure?.ToString(@"hh\:mm") ?? "--:--",
                ArrivalTime = r.Arrival?.ToString(@"hh\:mm") ?? "--:--"
            }).ToList();

            return Json(result);
        }

        // POST: Salvare bilet (urmează după finalizarea pașilor)
        [HttpPost]
        public IActionResult Buy(BuyTicketViewModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.PassengerName) || string.IsNullOrWhiteSpace(model.SeatCode))
                return BadRequest("Date incomplete.");

            // verificăm dacă locul e deja ocupat
            bool locOcupat = _context.Tickets.Any(t =>
                t.TrainId == model.TrainId &&
                t.TravelDate == model.Date &&
                t.Carriage == model.Carriage &&
                t.Seat == model.SeatCode);

            if (locOcupat)
            {
                return Json(new { success = false, message = "Locul selectat este deja ocupat." });
            }

            // mapăm BuyTicketViewModel în Ticket
            var ticket = new Ticket
            {
                UserId = model.UserId,
                TrainId = model.TrainId,
                DepartureStationId = model.DepartureStationId,
                ArrivalStationId = model.ArrivalStationId,
                TravelDate = model.Date,
                Seat = model.SeatCode,
                Carriage = model.Carriage,
                PassengerName = model.PassengerName,
                PassengerType = model.PassengerType,
                PassengerEmail = model.PassengerEmail,
                IsReturn = model.IsReturn,
                ReturnDate = model.ReturnDate,
                ReturnRouteId = model.ReturnRouteId,
                PaymentMethod = model.PaymentMethod,
                Price = model.TotalPrice,
                Status = "Confirmat",
                CreatedAt = DateTime.Now
            };

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return Json(new { success = true, message = "Bilet cumpărat cu succes!" });
        }

    }
}
