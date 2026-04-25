using Eventix_Project.Models;
using Microsoft.AspNetCore.Http;

namespace Eventix_Project.Controllers
{
    public class CreateEventDto
    {
            public int Id { get; set; } // Primary Key

            public string Title { get; set; } = null!;

            public string? Description { get; set; }

            public string Location { get; set; } = null!;

            public DateTime StartAt { get; set; }

            public decimal Price { get; set; }

            public int UserId { get; set; } // Foreign Key

            public int CategoryId { get; set; } // Foreign Key

            public string? Image { get; set; }

            public virtual EventCategory? Category { get; set; }
            public IFormFile? File { get; set; } // 👈 هنا

    }
}