using SchoolSystem.Models;
using System.Data.SqlClient;

namespace SchoolSystem
{
    internal class AdoCommands
    {
        private readonly string connectionString;
        public AdoCommands(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void GetEmployees()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT e.FirstName, e.LastName, p.PositionName, d.DepartmentName, e.WorkedYears AS 'Years Employed' FROM Employee e " +
                                       "join Position p on p.PositionId = e.FK_PositionId join Department d on d.DepartmentId = e.FK_DepartmentId", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("    Name       |  Position   |  Department  | Years Employed ");
                            Console.WriteLine("************************************************************");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]}     {reader["PositionName"]}     {reader["DepartmentName"]}    {reader["Years Employed"]}");
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception e)
                {

                    Console.WriteLine("Error:" + e.Message);
                }
            }
        }
        public void AddEmployee()
        {
            Employee emp = Enteremployee();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "Insert into Employee (FirstName, LastName, PersonalNumber, Salary, HiredDate, FK_PositionId, FK_DepartmentId) " +
                        "Values (@FirstName, @LastName, @PersonalNumber, @Salary, Cast(@HiredDate as Date), @FK_PositionId, @FK_DepartmentId)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", emp.LastName);
                        cmd.Parameters.AddWithValue("@PersonalNumber", emp.PersonalNumber);
                        cmd.Parameters.AddWithValue("@Salary", emp.Salary);
                        cmd.Parameters.AddWithValue("@HiredDate", emp.HiredDate);
                        cmd.Parameters.AddWithValue("@FK_PositionId", emp.FkPosition);
                        cmd.Parameters.AddWithValue("@FK_DepartmentId", emp.FkDepartment);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            Console.WriteLine($"New Employee added to system");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    }
                    connection.Close();
                }
                catch (Exception e)
                {

                    Console.WriteLine("Error: " + e.Message);
                }
            }

        }
        private Employee Enteremployee()
        {
            bool loop = false;
            string inputFirstName = "";
            string inputLastName = "";
            string inputPersonalNumber = "";

            while (!loop)
            {
                Console.WriteLine("Enter New Employees Firstname");
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
                Console.WriteLine("Enter PersonalNumber (YYYYMMDD-XXXX)");
                inputPersonalNumber = Console.ReadLine();
                loop = Utilities.ValidatePersonalNumber(inputPersonalNumber);
                if (!loop)
                {
                    Console.WriteLine("Wrong Format, try again ");
                }
            }
            loop = false;

            Console.WriteLine("Enter Salary");
            decimal inputSalary;
            while (!decimal.TryParse(Console.ReadLine(), out inputSalary) || inputSalary < 0)
            {
                Console.Write("Try again.. ");
            }

            Console.WriteLine("Enter HiredDate (YYYY-MM-DD)");
            string inputHired = Console.ReadLine();

            DateTime parsedHired = Utilities.ValidateDateFormat(inputHired);
            Console.Clear();
            //This is now hardcoded - should be info from db
            Console.WriteLine("Enter Position for new Employee");
            Console.WriteLine("1 for Teacher\n2 for Principal\n3 for Administration\n4 for Janitor");
            int position;
            while (!int.TryParse(Console.ReadLine(), out position) || position < 1 || position > 4)
            {
                Console.WriteLine("Only 1-4 are Valid Numbers");
            }

            Console.WriteLine("Enter Department");
            Console.WriteLine("1 for IT Dept\n2 for Math Dept\n3 for Administration Office\n4 for Maintenance\n5 for History Dept");
            int department;
            while (!int.TryParse(Console.ReadLine(), out department) || department < 0 || department > 5)
            {
                Console.WriteLine("Only 1-5 are Valid Number");
            }
            Employee emp = new()
            {
                FirstName = inputFirstName,
                LastName = inputLastName,
                PersonalNumber = inputPersonalNumber,
                Salary = inputSalary,
                HiredDate = parsedHired,
                FkPosition = position,
                FkDepartment = department
            };
            return emp;
        }

    }
}
