namespace SchoolSystem.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public byte GradeValue { get; set; }

    public DateOnly GradeDate { get; set; }

    public int? FkStudentId { get; set; }

    public int? FkCourseId { get; set; }
    public int? FkEmployeeId { get; set; }
    public virtual Employee? FkEmployee { get; set; }

    public virtual Course? FkCourse { get; set; }

    public virtual Student? FkStudent { get; set; }
}
