using Microsoft.AspNetCore.Mvc;
using CFRApp.Data;
using System;
using System.Linq;

namespace CFRApp.Controllers
{
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RouteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Route/Search
        public IActionResult Search()
        {
            ViewBag.Stations = _context.Stations.ToList(); // <– foarte important
            return View();
        }

        // POST: /Route/Search
        [HttpPost]
        public IActionResult Search(int departureStationId, int arrivalStationId, DateTime travelDate)
        {
            ViewBag.Stations = _context.Stations.ToList();

            // Căutăm rute posibile
            var matchingTrains = _context.Routes
                .Where(r => r.StationId == departureStationId)
                .Join(
                    _context.Routes.Where(r2 => r2.StationId == arrivalStationId),
                    dep => dep.TrainId,
                    arr => arr.TrainId,
                    (dep, arr) => new { dep, arr }
                )
                .Where(x => x.dep.Order < x.arr.Order)
                .Select(x => new
                {
                    TrainId = x.dep.TrainId,
                    Train = x.dep.Train,
                    DepartureTime = x.dep.DepartureTime,
                    ArrivalTime = x.arr.ArrivalTime
                })
                .Distinct()
                .ToList();

            ViewBag.Results = matchingTrains;
            ViewBag.DepartureStation = _context.Stations.Find(departureStationId)?.Name;
            ViewBag.ArrivalStation = _context.Stations.Find(arrivalStationId)?.Name;
            ViewBag.Date = travelDate.ToShortDateString();

            return View();
        }

    }
}
