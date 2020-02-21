using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Volo.Abp;

namespace App1
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            CreateLoggerUsingJSONFile();
            try
            {
                Log.Information("Starting App1.WebHost.");
                CreateHostBuilder(args, Log.Logger).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args, ILogger logger) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog(logger);


        private static void CreateLoggerUsingJSONFile()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(configuration).CreateLogger();
        }

      
    }    
}
