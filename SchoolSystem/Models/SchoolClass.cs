using System;
using System.Collections.Generic;

namespace SchoolSystem.Models;

public partial class SchoolClass
{
    public int SchoolClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
