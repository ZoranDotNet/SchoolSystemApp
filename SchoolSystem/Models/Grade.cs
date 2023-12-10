using System;
using System.Collections.Generic;

namespace SchoolSystem.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public byte GradeValue { get; set; }

    public DateOnly GradeDate { get; set; }

    public int? FkStudentId { get; set; }

    public int? FkCourseId { get; set; }

    public virtual Course? FkCourse { get; set; }

    public virtual Student? FkStudent { get; set; }
}
