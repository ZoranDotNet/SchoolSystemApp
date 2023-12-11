namespace SchoolSystem.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PersonalNumber { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateTime HiredDate { get; set; }

    public int? WorkedYears { get; set; }

    public int? FkPosition { get; set; }

    public int? FkDepartment { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Department? FkDepartmentNavigation { get; set; }

    public virtual Position? FkPositionNavigation { get; set; }
}
