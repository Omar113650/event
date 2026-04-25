using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class UserCommunity
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CommunityId { get; set; }

    public string? Role { get; set; }

    public DateOnly? JoinedAt { get; set; }

    public virtual Community? Community { get; set; }

    public virtual User? User { get; set; }
}
