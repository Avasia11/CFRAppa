using System;

namespace CFRApp.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "card"; // card, cash, transfer
        public string Status { get; set; } = "success";     // success, failed, pending

        public Ticket? Ticket { get; set; }
    }
}
