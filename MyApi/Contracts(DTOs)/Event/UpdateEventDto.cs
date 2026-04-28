namespace Eventix_Project.DTOs.Event;

public class UpdateEventDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int? Capacity { get; set; }
    public int? CategoryId { get; set; }
    public int? CommunityId { get; set; }
    public decimal? Price { get; set; }
    public string? Status { get; set; }
}