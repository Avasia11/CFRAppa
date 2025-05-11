using System;

namespace CFRApp.Models
{
    public class Delay
    {
        public int Id { get; set; }

        public int TrainId { get; set; }
        public int StationId { get; set; }
        public int DelayMinutes { get; set; } = 0;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigare
        public Train? Train { get; set; }
        public Station? Station { get; set; }
    }
}
