using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class Profile
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ProfileImage { get; set; }

    public virtual User? User { get; set; }
}
