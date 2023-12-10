using System;
using System.Collections.Generic;

namespace SchoolSystem.Models;

public partial class StudentCourse
{
    public int FkCourseId { get; set; }

    public int FkStudentId { get; set; }

    public virtual Course FkCourse { get; set; } = null!;

    public virtual Student FkStudent { get; set; } = null!;
}
