using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class Community
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();
}
