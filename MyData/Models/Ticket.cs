using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int? EventId { get; set; }

    public string? Type { get; set; }

    public decimal? Price { get; set; }

    public int? QuantityTotal { get; set; }

    public int? QuantitySold { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Event? Event { get; set; }
}
