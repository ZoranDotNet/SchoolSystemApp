using System;
using System.Collections.Generic;

namespace SchoolSystem.Models;

public partial class DepartmentPosition
{
    public int FkDepartmentId { get; set; }

    public int FkPositionId { get; set; }

    public virtual Department FkDepartment { get; set; } = null!;

    public virtual Position FkPosition { get; set; } = null!;
}
