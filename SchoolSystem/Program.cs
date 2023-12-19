using Microsoft.Extensions.Configuration;

namespace SchoolSystem;

internal class Program
{
    static void Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

        App myApp = new App(configuration);
        myApp.Run();
    }
}
