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
            //Menu here
            DisplayMenu();
        }

        public void DisplayMenu()
        {
            /*
             
            30=Black, 31=Red, 32=Green, 33=Yellow, 
            34=Blue, 35=Magenta, 36=Cyan, 37=white

            */

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


                key = Console.ReadKey(false);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        option = option == 1 ? 4 : option - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        option = option == 4 ? 1 : option + 1;
                        break;

                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            Console.WriteLine($"\n{decorator}You selected {option}");
            Console.ReadLine();
        }
    }
}
