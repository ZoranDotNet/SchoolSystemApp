using SchoolSystem.Models;
using System.Data.SqlClient;

namespace SchoolSystem;

internal class AdoCommands
{
    private readonly string connectionString;
    public AdoCommands(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void EmployeeOptions()
    {
        Console.WriteLine("1 - List all Employees");
        Console.WriteLine("2 - Add New Employee");
        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || option > 2 || option < 1)
        {
            Console.WriteLine("Try again");
        }

        if (option == 1)
        {
            GetEmployees();
        }
        else
        {
            AddEmployee();
        }
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
        //using validation on all userinputs
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
    public void PayrollStats()
    {
        Console.WriteLine("1 for Total salary per Department");
        Console.WriteLine("2 for Average salary per Department");
        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || option < 0 || option > 2)
        {
            Console.WriteLine("Only 1 & 2 are valid here.");
        }

        if (option == 1)
        {
            Console.Clear();
            PayrollTotal();
        }
        else
        {
            Console.Clear();
            PayrollAverage();
        }

    }
    private void PayrollTotal()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT d.DepartmentName, SUM(e.Salary) AS [Per Month] from Employee e " +
                    "join Department d on d.DepartmentId = e.FK_DepartmentId group by d.DepartmentName", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Department   Total/Month");
                        Console.WriteLine("************************");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["DepartmentName"]}   {reader["Per Month"]}");
                        }
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
    private void PayrollAverage()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT d.DepartmentName, CAST(AVG(e.Salary)AS INT) AS [Avg per Month] from Employee e " +
                    "join Department d on d.DepartmentId = e.FK_DepartmentId group by d.DepartmentName", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Depertment       Avg per Month");
                        Console.WriteLine("******************************");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["DepartmentName"]} {reader["Avg per Month"]}");
                        }
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
    private int GetEmployeeId()
    {
        Console.WriteLine("Enter Firstname");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Lastname");
        string lastName = Console.ReadLine();
        Console.Clear();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT EmployeeId FROM Employee where FirstName = @firstName and LastName = @lastName", connection))
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);

                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)result;
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e.Message);
            }
            return -1;
        }
    }
    private int GetStudentId()
    {
        Console.WriteLine("Wich Student, enter Firstname");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Lastname");
        string lastName = Console.ReadLine();
        Console.Clear();


        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT StudentId FROM Student where FirstName = @firstName and LastName = @lastName", connection))
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);

                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)result;
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e.Message);
            }
            return -1;
        }
    }
    private int GetCourseId()
    {
        Console.WriteLine("\nWich Course do you wish to Grade");
        string course = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("Select CourseId from Course where CourseName = @courseName", connection))
                {
                    command.Parameters.AddWithValue("@courseName", course);

                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return (int)result;
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return -1;
        }
    }
    public void StudentCourseInfo()
    {
        //calling a StoredProcedure where we list all courses the student is assigned to
        int studentId = GetStudentId();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                if (studentId > 0)
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec spStudentCourseInfo @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", studentId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Student is assigned to these courses");
                            Console.WriteLine("*  Name  *  Course  *  Starts  *  Ends  *  Grade  *  GradeDate\n");
                            while (reader.Read())
                            {
                                //Need this to get just the date without time
                                string start = ((DateTime)reader["StartDate"]).ToString("yyyy-MM-dd");
                                string end = ((DateTime)reader["EndDate"]).ToString("yyyy-MM-dd");

                                Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]} * {reader["Course"]} * {start} * {end}      {reader["Grade"]}       {reader["GradeDate"]}");
                            }
                        }
                    }
                    connection.Close();
                }
                else
                {
                    Console.WriteLine("Could not find the Student");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
    public void TeacherInfo()
    {
        //using a View to show all Teachers for every course
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from ViewTeacherCourses", connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Name   |   Course   |   Department\n");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]} * {reader["CourseName"]} - {reader["DepartmentName"]}");
                        }
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
    public void DeleteStudent()
    {
        //StoredProcedure with Transaction. When we delete a student we save the student in a secret table
        int id = GetStudentId();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                if (id > 0)
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec spDeleteStudent @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                        Console.WriteLine("Student deleted");
                    }
                }
                else
                {
                    Console.WriteLine("Could not find the Student");
                }
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
    public void SetGrade()
    {
        //using transaction in c# code when we set grade
        int studentId = ListCoursesWithNoGrade();
        int courseId = GetCourseId();
        TeacherInfo();
        Console.WriteLine("\nWich Teacher is setting the Grade");
        int employeeId = GetEmployeeId();
        Console.WriteLine("What grade 1-5");
        int grade;
        while (!int.TryParse(Console.ReadLine(), out grade) || grade < 1 || grade > 5)
        {
            Console.WriteLine("Try again, 1-5 valid numbers");
        }
        Console.WriteLine("Enter GradeDate (YYYY-MM-DD)");
        string gradeDate = Console.ReadLine();
        DateTime parsedGradeDate = Utilities.ValidateDateFormat(gradeDate);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                string query = @"Insert into Grade (GradeValue, GradeDate, FK_StudentId, FK_CourseId, FK_EmployeeId) 
                            Values (@grade, Cast(@date as Date), @fkStudent, @fkCourse, @fkEmployee)";

                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@grade", grade);
                    command.Parameters.AddWithValue("@date", parsedGradeDate);
                    command.Parameters.AddWithValue("@fkStudent", studentId);
                    command.Parameters.AddWithValue("@fkCourse", courseId);
                    command.Parameters.AddWithValue("@fkEmployee", employeeId);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine($"New Grade registred");
                        transaction.Commit();
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

                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }
    }
    private int ListCoursesWithNoGrade()
    {
        int id = GetStudentId();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = @"
                            Select s.FirstName, s.LastName, c.CourseName, c.EndDate 
                            From Course c
                            join StudentCourse sc on sc.FK_CourseId = c.CourseId
                            join Student s on s.StudentId = sc.FK_StudentId and s.StudentId = @id
                            left join Grade g on g.FK_CourseId = c.CourseId
                            Where c.EndDate < GetDate() and g.GradeValue IS NULL";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Finished courses with Grade not set yet");
                        Console.WriteLine("Name  |  Course  | EndDate\n");
                        while (reader.Read())
                        {
                            string endDate = ((DateTime)reader["EndDate"]).ToString("yyyy-MM-dd");
                            Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]} * {reader["CourseName"]} {endDate}");
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e.Message);
            }
        }
        return id;
    }
    public void ViewGrades()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("Select * from ViewGrades Order By GradeDate", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("StudentId | Name  | Course | Grade | Date | Teacher\n");
                        while (reader.Read())
                        {
                            string date = ((DateTime)reader["GradeDate"]).ToString("yyyy-MM-dd");
                            Console.WriteLine($"{reader["StudentId"]} {reader["FirstName"]} {reader["LastName"]} * {reader["CourseName"]} * {reader["Grade"]} * {date} - {reader["Role"]} {reader["First"]} {reader["Last"]}");
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
