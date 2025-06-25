using System;
using System.Collections.Generic;

namespace CFRApp.Models
{
    public class BuyTicketViewModel
    {
        public int TrainId { get; set; }
        public string TrainNumber { get; set; }
        public string TrainType { get; set; }
        public string Operator { get; set; }

        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }

        public TimeSpan DepartureTime { get; set; }
        public DateTime Date { get; set; }

        // Retur
        public bool IsReturn { get; set; }  // schimbat din HasReturn
        public DateTime? ReturnDate { get; set; }
        public string ReturnRouteId { get; set; }
        public List<ReturnRouteOption> ReturnRoutes { get; set; }

        // Pasager
        public string PassengerName { get; set; }
        public string PassengerType { get; set; }
        public string PassengerEmail { get; set; }

        // Loc
        public string Carriage { get; set; }
        public string SeatCode { get; set; }  // redenumit din Seat

        // Final
        public string PaymentMethod { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

        // Extra
        public int UserId { get; set; }
    }

    public class ReturnRouteOption
    {
        public int TrainId { get; set; }
        public string TrainNumber { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
