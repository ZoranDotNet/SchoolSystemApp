using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace SchoolSystem
{
    internal class App
    {
        private readonly SchoolSystemContext dbContext;
        private readonly AdoCommands ado;

        public App(IConfiguration configuration)
        {
            dbContext = new SchoolSystemContext(configuration);
            ado = new AdoCommands(configuration.GetConnectionString("Default"));
        }

        public void Run()
        {
            bool loop = true;

            while (loop)
            {
                int option = DisplayMenu();

                switch (option)
                {
                    case 1:
                        ado.GetEmployees();
                        Console.ReadKey();
                        break;

                    case 2:
                        dbContext.GetStudents();
                        Console.ReadKey();
                        break;

                    case 3:
                        ado.AddEmployee();
                        Console.ReadKey();
                        break;

                    case 4:
                        dbContext.AddStudent();
                        Console.ReadKey();
                        break;

                    case 5:
                        dbContext.EditStudent();
                        Console.ReadKey();
                        break;

                    case 6:
                        dbContext.ListActiveCourses();
                        Console.ReadKey();
                        break;

                    case 7:
                        ado.PayrollStats();
                        Console.ReadKey();
                        break;

                    case 8:
                        ado.TeacherInfo();
                        Console.ReadKey();
                        break;

                    case 9:
                        dbContext.EmployeePerDepartment();
                        Console.ReadKey();
                        break;

                    case 10:
                        ado.StudentCourseInfo();
                        Console.ReadKey();
                        break;

                    case 11:
                        dbContext.RegisterStudentToCourse();
                        Console.ReadKey();
                        break;

                    case 12:
                        ado.SetGrade();
                        Console.ReadKey();
                        break;

                    case 13:
                        ado.DeleteStudent();
                        Console.ReadKey();
                        break;
                }
            }
        }

        public int DisplayMenu()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.WriteLine("\nUse ⬆️  and ⬇️  to navigate and press \u001b[34mEnter/Return\u001b[0m to select:");
            Console.WriteLine("Please make a choice\n ");
            (int left, int top) = Console.GetCursorPosition();
            var option = 1;
            var decorator = "✅ \u001b[31m";
            ConsoleKeyInfo key;
            bool isSelected = false;

            while (!isSelected)
            {
                Console.SetCursorPosition(left, top);

                Console.WriteLine($"{(option == 1 ? decorator : "   ")}1 All Employees\u001b[34m");
                Console.WriteLine($"{(option == 2 ? decorator : "   ")}2 All Students\u001b[34m");
                Console.WriteLine($"{(option == 3 ? decorator : "   ")}3 Add New Employee\u001b[34m");
                Console.WriteLine($"{(option == 4 ? decorator : "   ")}4 Add New Student\u001b[34m");
                Console.WriteLine($"{(option == 5 ? decorator : "   ")}5 Edit Student\u001b[34m");
                Console.WriteLine($"{(option == 6 ? decorator : "   ")}6 Show Active Courses\u001b[34m");
                Console.WriteLine($"{(option == 7 ? decorator : "   ")}7 Payroll Statistics\u001b[34m");
                Console.WriteLine($"{(option == 8 ? decorator : "   ")}8 Show Teachers - Courses\u001b[34m");
                Console.WriteLine($"{(option == 9 ? decorator : "   ")}9 Employees / Department\u001b[34m");
                Console.WriteLine($"{(option == 10 ? decorator : "   ")}10 Student Course Info\u001b[34m");
                Console.WriteLine($"{(option == 11 ? decorator : "   ")}11 Register student to Course\u001b[34m");
                Console.WriteLine($"{(option == 12 ? decorator : "   ")}12 Set Grade\u001b[34m");
                Console.WriteLine($"{(option == 13 ? decorator : "   ")}13 Delete Student\u001b[34m");

                key = Console.ReadKey(false);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        option = option == 1 ? 13 : option - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        option = option == 13 ? 1 : option + 1;
                        break;

                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }
            Console.WriteLine($"\u001b[0m");
            Console.CursorVisible = true;
            return option;
        }
    }
}
