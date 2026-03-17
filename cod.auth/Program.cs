

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace cod.auth
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls($"http://*:{System.Environment.GetEnvironmentVariable("PORT") ?? "5000"}")  // Railway inyecta PORT; local: 5000
                .UseStartup<Startup>();
    }
}