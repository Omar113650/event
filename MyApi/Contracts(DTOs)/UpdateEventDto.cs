namespace Eventix_Project.Models
{
    public class UpdateEventDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }

        public DateTime? StartAt { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
    }
}