using System;

namespace CFRApp.Models
{
    public class TrainRouteViewModel
    {
        public string Tip { get; set; }
        public Train Tren1 { get; set; }
        public Train? Tren2 { get; set; }
        public string? StatieIntermediar { get; set; }
        public TimeSpan? OraPlecare { get; set; }
        public TimeSpan? OraSosire { get; set; }
    }
}
