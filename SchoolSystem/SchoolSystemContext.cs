using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolSystem.Models;

namespace SchoolSystem
{
    public partial class SchoolSystemContext : DbContext
    {
        private readonly IConfiguration configuration;

        public SchoolSystemContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public void GetStudents()
        {
            using (var dbContext = new SchoolSystemContext(configuration))
            {
                var allStudents = dbContext.Students.Include(x => x.FkSchoolClass).ToList();

                foreach (Student s in allStudents)
                {
                    Console.WriteLine($"{s.FirstName} {s.LastName} * {s.EmailAdress} * {s.PersonalNumber} {s.FkSchoolClass?.ClassName}");
                }
            }
        }
        public void AddStudent()
        {
            Student student = EnterStudent();

            using (SchoolSystemContext dbContext = new SchoolSystemContext(configuration))
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
                    student.FkSchoolClass = selectedClass;
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
                Console.WriteLine("Enter New Students Firstname");
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
        public void ActiveCourses()
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

                Console.WriteLine("Department   NrEmployee");
                Console.WriteLine("***********************");
                foreach (var department in nrOfEmployees)
                {
                    Console.WriteLine($"{department.DepartmentName} {department.EmployeesCount}");
                }
            }
        }
    }
}
