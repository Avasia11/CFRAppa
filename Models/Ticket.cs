using System;

namespace CFRApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int TrainId { get; set; }
        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }

        public DateTime TravelDate { get; set; }
        public string? Seat { get; set; }
        public string Status { get; set; } = "reserved"; // reserved, paid, cancelled
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relații (opționale)
        public User? User { get; set; }
        public Train? Train { get; set; }
        public Station? DepartureStation { get; set; }
        public Station? ArrivalStation { get; set; }
    }
}
