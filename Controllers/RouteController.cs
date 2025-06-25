using Microsoft.AspNetCore.Mvc;
using CFRApp.Data;
using CFRApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace CFRApp.Controllers
{
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RouteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Search()
        {
            var model = new RouteSearchViewModel
            {
                Stations = _context.Stations.OrderBy(s => s.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(RouteSearchViewModel model)
        {
            model.Stations = _context.Stations.OrderBy(s => s.Name).ToList();
            model.ResultsStructured = new();
            model.MultiTrainRoutes = new();

            if (string.IsNullOrEmpty(model.DepartureStation) || string.IsNullOrEmpty(model.ArrivalStation))
                return View(model);

            var departure = _context.Stations.FirstOrDefault(s => s.Name == model.DepartureStation);
            var arrival = _context.Stations.FirstOrDefault(s => s.Name == model.ArrivalStation);

            if (departure == null || arrival == null || departure.Id == arrival.Id)
                return View(model);

            // 🔹 RUTE DIRECTE
            var directTrains = (from r1 in _context.Routes
                                join r2 in _context.Routes on r1.TrainId equals r2.TrainId
                                where r1.StationId == departure.Id && r2.StationId == arrival.Id
                                      && r1.Order < r2.Order
                                select new
                                {
                                    Train = r1.Train,
                                    DepartureTime = r1.DepartureTime,
                                    ArrivalTime = r2.ArrivalTime,
                                    Stops = _context.Routes
                                        .Where(r => r.TrainId == r1.TrainId && r.Order >= r1.Order && r.Order <= r2.Order)
                                        .Include(r => r.Station)
                                        .OrderBy(r => r.Order)
                                        .ToList()
                                }).ToList();

            model.ResultsStructured = directTrains.Select(t => new StructuredRouteResult
            {
                TrainId = t.Train.Id, // ✅ AICI
                TrainNumber = t.Train.Number,
                TrainType = t.Train.Type,
                Operator = t.Train.Operator,
                DepartureStation = model.DepartureStation,
                ArrivalStation = model.ArrivalStation,
                DepartureTime = t.DepartureTime ?? TimeSpan.Zero,
                ArrivalTime = t.ArrivalTime ?? TimeSpan.Zero,
                Stops = t.Stops.Select(s => new StopInfo

                {
                    StationName = s.Station.Name,
                    Arrival = s.ArrivalTime,
                    Departure = s.DepartureTime
                }).ToList()
            }).ToList();

            // 🔹 RUTE CU SCHIMBĂRI (1 schimbare)
            var allFirstLegs = _context.Routes
                .Include(r => r.Train)
                .Where(r => r.StationId == departure.Id && r.StopType != "end")
                .ToList();

            var allSecondLegs = _context.Routes
                .Include(r => r.Train)
                .Where(r => r.StationId == arrival.Id && r.StopType != "start")
                .ToList();

            foreach (var first in allFirstLegs)
            {
                var intermediateStops = _context.Routes
                    .Where(r => r.TrainId == first.TrainId && r.Order > first.Order)
                    .Include(r => r.Station)
                    .ToList();

                foreach (var transferStop in intermediateStops)
                {
                    var possibleSecondLegs = (from r1 in _context.Routes
                                              join r2 in _context.Routes on r1.TrainId equals r2.TrainId
                                              where r1.StationId == transferStop.StationId
                                                    && r2.StationId == arrival.Id
                                                    && r1.Order < r2.Order
                                              select new { r1, r2 }).ToList();

                    foreach (var leg in possibleSecondLegs)
                    {
                        if (transferStop.ArrivalTime.HasValue && leg.r1.DepartureTime.HasValue)
                        {
                            var waitTime = leg.r1.DepartureTime.Value - transferStop.ArrivalTime.Value;
                            if (waitTime.TotalMinutes >= 10)
                            {
                                var totalDuration = leg.r2.ArrivalTime.Value - first.DepartureTime.Value;

                                // ✅ Stops pentru primul tren
                                var firstStops = _context.Routes
    .Where(r => r.TrainId == first.TrainId && r.Order >= first.Order && r.Order <= transferStop.Order)
    .Include(r => r.Station)
    .OrderBy(r => r.Order)
    .Select(r => new StopInfo
    {
        StationName = r.Station.Name,
        Arrival = r.ArrivalTime,
        Departure = r.DepartureTime
    }).ToList();


                                // ✅ Stops pentru al doilea tren
                                var secondStops = _context.Routes
    .Where(r => r.TrainId == leg.r1.TrainId && r.Order >= leg.r1.Order && r.Order <= leg.r2.Order)
    .Include(r => r.Station)
    .OrderBy(r => r.Order)
    .Select(r => new StopInfo
    {
        StationName = r.Station.Name,
        Arrival = r.ArrivalTime,
        Departure = r.DepartureTime
    }).ToList();


                                model.MultiTrainRoutes.Add(new MultiTrainRoute
                                {
                                    TotalDuration = totalDuration.ToString(@"hh\:mm"),
                                    Steps = new List<TransferStep>
                            {
                                new TransferStep
                                {
                                    TrainNumber = first.Train.Number,
                                    TrainType = first.Train.Type,
                                    Operator = first.Train.Operator,
                                    FromStation = model.DepartureStation,
                                    ToStation = transferStop.Station.Name,
                                    DepartureTime = first.DepartureTime?.ToString(@"hh\:mm"),
                                    ArrivalTime = transferStop.ArrivalTime?.ToString(@"hh\:mm"),
                                    Stops = firstStops,
                                    EffectiveStops = firstStops.Count > 0 ? firstStops.Count - 1 : 0

                                },
                                new TransferStep
                                {
                                    TrainNumber = leg.r1.Train.Number,
                                    TrainType = leg.r1.Train.Type,
                                    Operator = leg.r1.Train.Operator,
                                    FromStation = transferStop.Station.Name,
                                    ToStation = model.ArrivalStation,
                                    DepartureTime = leg.r1.DepartureTime?.ToString(@"hh\:mm"),
                                    ArrivalTime = leg.r2.ArrivalTime?.ToString(@"hh\:mm"),
                                    Stops = secondStops,
                                    EffectiveStops = secondStops.Count > 0 ? secondStops.Count - 1 : 0
                                }
                            }
                                });
                            }
                        }
                    }
                }
            }

            return View(model);
        }

    }
}
