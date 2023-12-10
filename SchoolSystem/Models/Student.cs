using System;
using System.Collections.Generic;

namespace SchoolSystem.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailAdress { get; set; } = null!;

    public string PersonalNumber { get; set; } = null!;

    public int? FkSchoolClassId { get; set; }

    public virtual SchoolClass? FkSchoolClass { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
