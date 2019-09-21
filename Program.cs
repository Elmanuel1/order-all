using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwaggerApp.Data;
using System;
using System.Data.SqlClient;
using Evolve.Dialect.Cassandra;
using Microsoft.Extensions.Configuration;

namespace SwaggerApp
{
    
    public class Program
    {
        private static readonly IConfiguration _config;
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                }).
                Build();
            SeedDatabase(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void SeedDatabase(IWebHost host)
        {
            var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SampleContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                
                if (context.Database.EnsureCreated())
                {
                    try
                    {
                        var cnx = new SqlConnection("Data Source=localhost;Initial Catalog=orders;User ID=sa;Password=Generation1\"*;");
                        
                        var evolve = new Evolve.Evolve(cnx, msg => logger.LogInformation(msg))
                        {
                            Locations = new[] { "Data/migrations" },
                            IsEraseDisabled = true,
                        };

                        evolve.Migrate();
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical("Database migration failed.", ex);
                        throw;
                    }
                    
                    try
                    {
                        SeedData.Initialize(context);
                    }
                    catch (Exception ex)
                    {
                        
                        logger.LogError(ex, "A database seeding error occurred.");
                    }
                }
            }
        }
    }
}
