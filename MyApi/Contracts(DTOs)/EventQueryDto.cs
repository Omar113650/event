namespace Eventix_Project.Controllers
{
    public class EventQueryDto
    {
        public string? Search { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}