using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class Event
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public int? Capacity { get; set; }

    public int? UserId { get; set; }

    public int? CategoryId { get; set; }

    public int? CommunityId { get; set; }

    public string? Status { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual EventCategory? Category { get; set; }

    public virtual Community? Community { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User? User { get; set; }
    public decimal Price { get; set; }
}
