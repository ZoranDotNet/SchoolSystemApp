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
                    Console.WriteLine($"{s.FirstName} {s.LastName} {s.EmailAdress} {s.PersonalNumber} {s.FkSchoolClass?.ClassName}");
                }
            }
        }

    }
}
