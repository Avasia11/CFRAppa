// CFRApp/Models/Route.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFRApp.Models
{
    public class Route
    {
        public int Id { get; set; }

        public int TrainId { get; set; }
        public Train Train { get; set; }

        public int StationId { get; set; }
        public Station Station { get; set; }

        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; }

        public int Order { get; set; } // ordinea opririi
        public string StopType { get; set; } // start, intermediate, end
    }

}
