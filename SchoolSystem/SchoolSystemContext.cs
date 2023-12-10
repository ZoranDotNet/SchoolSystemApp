using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SchoolSystem
{
    public partial class SchoolSystemContext : DbContext
    {
        private readonly IConfiguration configuration;

        public SchoolSystemContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

    }
}
