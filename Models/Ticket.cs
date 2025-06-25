public class Ticket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TrainId { get; set; }
    public int DepartureStationId { get; set; }
    public int ArrivalStationId { get; set; }
    public DateTime TravelDate { get; set; }
    public string Seat { get; set; }
    public string Carriage { get; set; }
    public string PassengerName { get; set; }
    public string PassengerType { get; set; }
    public string PassengerEmail { get; set; }
    public bool IsReturn { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? ReturnRouteId { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = "Confirmat";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
