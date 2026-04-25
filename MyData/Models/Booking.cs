using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public int? Seats { get; set; }

    public decimal? TotalPrice { get; set; }

    public int? EventId { get; set; }

    public int? UserId { get; set; }

    public int? TicketId { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Ticket? Ticket { get; set; }

    public virtual User? User { get; set; }
}
