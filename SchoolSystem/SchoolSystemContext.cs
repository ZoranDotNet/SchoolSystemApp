using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolSystem.Models;

namespace SchoolSystem;

public partial class SchoolSystemContext : DbContext
{
    private readonly IConfiguration configuration;

    public SchoolSystemContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void StudentOptions()
    {
        Console.WriteLine("1 - List All Students");
        Console.WriteLine("2 - Add New Student");
        Console.WriteLine("3 - Edit Student");
        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || option > 3 || option < 1)
        {
            Console.WriteLine("Try again, valid numbers 1-3");
        }

        if (option == 1)
        {
            GetStudents();
        }
        else if (option == 2)
        {
            AddStudent();
        }
        else if (option == 3)
        {
            EditStudent();
        }

    }
    public void GetStudents()
    {
        using (var dbContext = new SchoolSystemContext(configuration))
        {
            var allStudents = dbContext.Students.Include(x => x.FkSchoolClass).ToList();

            foreach (Student s in allStudents)
            {
                Console.WriteLine($"{s.FirstName} {s.LastName} * {s.EmailAdress} * {s.PersonalNumber} * {s.FkSchoolClass?.ClassName}");
            }
        }
    }
    public void AddStudent()
    {
        Student student = EnterStudent();

        using (SchoolSystemContext dbContext = new SchoolSystemContext(configuration))
        {
            SchoolClass classes = EnterSchoolClass();

            if (classes != null)
            {
                student.FkSchoolClassId = classes.SchoolClassId;
            }
            else
            {
                Console.WriteLine("Could not find the SchoolClass");
            }

            dbContext.Students.Add(student);
            dbContext.SaveChanges();
            Console.WriteLine("Student added to system");
        }

    }
    private Student EnterStudent()
    {
        bool loop = false;
        string inputFirstName = "";
        string inputLastName = "";
        string inputEmail = "";
        string inputPersonalNumber = "";

        while (!loop)
        {
            Console.WriteLine("Enter Students Firstname");
            inputFirstName = Console.ReadLine();
            loop = Utilities.ValidateString(inputFirstName);
            if (!loop) Console.WriteLine("Only letters are accepted ");
        }
        loop = false;
        while (!loop)
        {
            Console.WriteLine("Enter Lastname");
            inputLastName = Console.ReadLine();
            loop = Utilities.ValidateString(inputLastName);
            if (!loop) Console.WriteLine("Only letters are accepted");
        }
        loop = false;
        while (!loop)
        {
            Console.WriteLine("Enter Email");
            inputEmail = Console.ReadLine();
            loop = Utilities.ValidateEmail(inputEmail);
            if (!loop) Console.WriteLine("Not a valid Email");
        }
        loop = false;
        while (!loop)
        {
            Console.WriteLine("Enter PersonalNumber (YYYYMMDD-XXXX)");
            inputPersonalNumber = Console.ReadLine();
            loop = Utilities.ValidatePersonalNumber(inputPersonalNumber);
            if (!loop)
            {
                Console.WriteLine("Wrong Format, try again ");
            }
        }

        Student st = new()
        {
            FirstName = inputFirstName,
            LastName = inputLastName,
            EmailAdress = inputEmail,
            PersonalNumber = inputPersonalNumber,
        };
        Console.Clear();
        return st;
    }
    public void ListActiveCourses()
    {
        using (var dbContext = new SchoolSystemContext(configuration))
        {
            var activeCourses = dbContext.Courses.Where(c => c.StartDate <= DateOnly.FromDateTime(DateTime.Now)).Where(x => x.EndDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();

            Console.WriteLine("CourseName | Start | End ");
            foreach (var course in activeCourses)
            {
                Console.WriteLine($"{course.CourseName} * {course.StartDate} -- {course.EndDate}");
            }
        }
    }
    public void EmployeePerDepartment()
    {
        using (var dbContext = new SchoolSystemContext(configuration))
        {
            var nrOfEmployees = dbContext.Departments.Include(e => e.Employees).Select(x => new
            {
                DepartmentName = x.DepartmentName,
                EmployeesCount = x.Employees.Count()
            })
            .ToList();

            Console.WriteLine("Department |  Employees");
            Console.WriteLine("************************");
            foreach (var department in nrOfEmployees)
            {
                Console.WriteLine($"{department.DepartmentName}: {department.EmployeesCount}");
            }
        }
    }
    public void EditStudent()
    {
        int studentId = GetStudentId();

        using (var dbContext = new SchoolSystemContext(configuration))
        {
            var student = dbContext.Students.FirstOrDefault(x => x.StudentId == studentId);

            Console.WriteLine("You can now Edit the Student");
            Console.ReadKey();
            if (student != null)
            {
                Student editStudent = EnterStudent();

                student.FirstName = editStudent.FirstName;
                student.LastName = editStudent.LastName;
                student.EmailAdress = editStudent.EmailAdress;
                student.PersonalNumber = editStudent.PersonalNumber;
            }
            else
            {
                Console.WriteLine("Could not find Student");
                return;
            }

            SchoolClass selectedClass = EnterSchoolClass();
            if (selectedClass != null)
            {
                student.FkSchoolClass = selectedClass;
            }
            else
            {
                Console.WriteLine("Could not find the schoolclass");
                return;
            }

            dbContext.SaveChanges();

        }
    }
    private int GetStudentId()
    {
        Console.WriteLine("Wich Student. Enter Firstname");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Lastname");
        string lastName = Console.ReadLine();

        using (var context = new SchoolSystemContext(configuration))
        {
            var student = context.Students.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);

            return student.StudentId;
        }
    }
    public void RegisterStudentToCourse()
    {
        int id = GetStudentId();

        Console.Clear();
        Console.WriteLine("These are the Courses that are available:\n");
        ListAvailableCourses();
        using (var context = new SchoolSystemContext(configuration))
        {
            var selectedstudent = context.Students.FirstOrDefault(x => x.StudentId == id);

            StudentCourse sc = new StudentCourse();
            if (selectedstudent != null)
            {
                sc.FkStudentId = selectedstudent.StudentId;
            }
            Course selectedCourse = RegisterCourse();

            if (selectedCourse != null)
            {
                sc.FkCourseId = selectedCourse.CourseId;
            }

            context.StudentCourses.Add(sc);
            context.SaveChanges();
            Console.WriteLine("Student is now Registered");
        }

    }
    private SchoolClass EnterSchoolClass()
    {
        using (var dbContext = new SchoolSystemContext(configuration))
        {
            var allClasses = dbContext.SchoolClasses.ToList();

            foreach (var item in allClasses)
            {
                Console.WriteLine($"Class: {item.ClassName}");
            }

            Console.WriteLine("Wich class do you wish to register the student to");
            string input = Console.ReadLine().ToLower();

            var selectedClass = allClasses.FirstOrDefault(x => x.ClassName.ToLower() == input);

            if (selectedClass != null)
            {
                return selectedClass;
            }
            else
            {
                Console.WriteLine("Could not find the Schoolclass");
                return null;
            }
        }
    }
    private void ListAvailableCourses()
    {
        using (var context = new SchoolSystemContext(configuration))
        {
            var courses = context.Courses.Where(x => x.StartDate > DateOnly.FromDateTime(DateTime.Now)).ToList();

            foreach (var course in courses)
            {
                Console.WriteLine($"{course.CourseName} {course.StartDate} {course.EndDate}");
            }
        }
    }
    private Course RegisterCourse()
    {
        Console.WriteLine("\nWich course do you wish to register Student to");
        string selectedCourse = Console.ReadLine();

        using (var dbContext = new SchoolSystemContext(configuration))
        {
            if (selectedCourse != null)
            {
                Course foundCourse = dbContext.Courses.FirstOrDefault(x => x.CourseName.ToLower() == selectedCourse.ToLower());
                return foundCourse;
            }
            else
            {
                Console.WriteLine("Could not find the Course");
                return null;
            }
        }
    }
}
