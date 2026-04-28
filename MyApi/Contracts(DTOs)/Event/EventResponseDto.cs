namespace Eventix_Project.DTOs.Event;

public class EventResponseDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Location { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public decimal Price { get; set; }
    public int? Capacity { get; set; }
    public string? Status { get; set; }
    public string? Image { get; set; }

    public string? CategoryName { get; set; }
    public string? CommunityName { get; set; }
}