namespace Eventix_Project.DTOs.Booking;

public class CreateBookingDto
{
    public int EventId { get; set; }
    public int? TicketId { get; set; }
    public int Seats { get; set; }
}