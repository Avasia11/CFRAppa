using System;

namespace CFRApp.Models
{
    public class Route
    {
        public int Id { get; set; }

        public int TrainId { get; set; }
        public int StationId { get; set; }

        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; }
        public int Order { get; set; }

        public string StopType { get; set; } = "intermediate"; // start, intermediate, end

        // Relații (opțional pentru navigare)
        public Train? Train { get; set; }
        public Station? Station { get; set; }
    }
}
