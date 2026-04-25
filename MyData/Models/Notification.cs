using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public int? UserId { get; set; }

    public int? EventId { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public virtual Event? Event { get; set; }

    public virtual User? User { get; set; }
}
