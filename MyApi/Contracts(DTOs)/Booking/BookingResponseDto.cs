namespace Eventix_Project.DTOs.Booking;

public class BookingResponseDto
{
    public int Id { get; set; }
    public string? Status { get; set; }
    public int? Seats { get; set; }
    public decimal? TotalPrice { get; set; }

    public int? EventId { get; set; }
    public string? EventTitle { get; set; }

    public int? UserId { get; set; }
    public string? UserName { get; set; }

    public DateTime? CreatedAt { get; set; }
}