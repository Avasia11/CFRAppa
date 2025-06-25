using System;
using System.Collections.Generic;

namespace CFRApp.Models
{
    public class RouteSearchViewModel
    {
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public DateTime SearchDate { get; set; } = DateTime.Today;
        public bool IncludeReturn { get; set; }

        public List<Station> Stations { get; set; } = new();
        public List<StructuredRouteResult> ResultsStructured { get; set; } = new(); // pentru rute directe
        public List<MultiTrainRoute> MultiTrainRoutes { get; set; } = new();       // pentru rute cu schimbări
    }

    public class StructuredRouteResult
    {
        public string TrainNumber { get; set; }
        public string TrainType { get; set; }
        public string Operator { get; set; }
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string Duration => (ArrivalTime - DepartureTime).ToString(@"hh\:mm");

        public List<StopInfo> Stops { get; set; } = new();
        public double Distance { get; set; }
        public double Price { get; set; }
        public int TrainId { get; set; }


    }

    public class StopInfo
    {
        public string StationName { get; set; }
        public TimeSpan? Arrival { get; set; }
        public TimeSpan? Departure { get; set; }
    }

    public class MultiTrainRoute
    {
        public List<TransferStep> Steps { get; set; } = new();
        public string TotalDuration { get; set; }
    }

    public class TransferStep
    {
        public string TrainNumber { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public List<StopInfo> Stops { get; set; } = new List<StopInfo>();
        public string TrainType { get; set; }
        public string Operator { get; set; }
        public int EffectiveStops { get; set; } // ✅ număr de stații reale (fără plecare)
        public int TrainId { get; set; }


    }


}
