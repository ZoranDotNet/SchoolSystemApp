namespace SchoolSystem.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? FkEmployeeId { get; set; }

    public virtual Employee? FkEmployee { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
