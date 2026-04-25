using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class EventCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Icon { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
