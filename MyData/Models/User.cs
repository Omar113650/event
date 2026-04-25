
﻿using System.Collections.Generic;

﻿using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    // ===== AUTH FIELDS =====
    
    public bool IsAccountVerified { get; set; } = false;

    public string? Otp { get; set; }

    public DateTime? OtpExpiresAt { get; set; }

    public string? ResetPasswordToken { get; set; }

    // ===== NAVIGATION =====

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
