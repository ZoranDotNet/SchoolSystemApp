using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace SchoolSystem;

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
                    ado.EmployeeOptions();
                    Console.ReadKey();
                    break;

                case 2:
                    dbContext.StudentOptions();
                    Console.ReadKey();
                    break;

                case 3:
                    dbContext.ListActiveCourses();
                    Console.ReadKey();
                    break;

                case 4:
                    ado.PayrollStats();
                    Console.ReadKey();
                    break;

                case 5:
                    ado.TeacherInfo();
                    Console.ReadKey();
                    break;

                case 6:
                    dbContext.EmployeePerDepartment();
                    Console.ReadKey();
                    break;

                case 7:
                    ado.StudentCourseInfo();
                    Console.ReadKey();
                    break;

                case 8:
                    dbContext.RegisterStudentToCourse();
                    Console.ReadKey();
                    break;

                case 9:
                    ado.SetGrade();
                    Console.ReadKey();
                    break;

                case 10:
                    ado.ViewGrades();
                    Console.ReadKey();
                    break;

                case 11:
                    ado.DeleteStudent();
                    Console.ReadKey();
                    break;

                case 12:
                    loop = false;
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

            Console.WriteLine($"{(option == 1 ? decorator : "   ")}1 Employees\u001b[34m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}2 Students\u001b[34m");
            Console.WriteLine($"{(option == 3 ? decorator : "   ")}3 Show Active Courses\u001b[34m");
            Console.WriteLine($"{(option == 4 ? decorator : "   ")}4 Payroll Statistics\u001b[34m");
            Console.WriteLine($"{(option == 5 ? decorator : "   ")}5 Show Teachers - Courses\u001b[34m");
            Console.WriteLine($"{(option == 6 ? decorator : "   ")}6 Employees / Department\u001b[34m");
            Console.WriteLine($"{(option == 7 ? decorator : "   ")}7 Student Course Info\u001b[34m");
            Console.WriteLine($"{(option == 8 ? decorator : "   ")}8 Register student to Course\u001b[34m");
            Console.WriteLine($"{(option == 9 ? decorator : "   ")}9 Set Grade\u001b[34m");
            Console.WriteLine($"{(option == 10 ? decorator : "   ")}10 View Grades\u001b[34m");
            Console.WriteLine($"{(option == 11 ? decorator : "   ")}11 Delete Student\u001b[34m");
            Console.WriteLine($"{(option == 12 ? decorator : "   ")}12 Exit\u001b[34m");

            key = Console.ReadKey(false);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    option = option == 1 ? 12 : option - 1;
                    break;

                case ConsoleKey.DownArrow:
                    option = option == 12 ? 1 : option + 1;
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
