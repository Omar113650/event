using System;
using System.Collections.Generic;

namespace Eventix_Project.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
