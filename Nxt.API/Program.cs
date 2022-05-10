using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Security.Authentication;

namespace Nxt.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureHostConfiguration(configHost =>
            {
                configHost.AddCommandLine(args);
            })
            .UseSerilog()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var environment = string.Empty;
                if (args != null && args.Any())
                {
                    environment = args[0];
                }
                else
                {
                    environment = hostingContext.HostingEnvironment.EnvironmentName;
                }
                Console.WriteLine($"HostingEnvironment:{environment}");
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(x =>
                {
                    x.AddServerHeader = false;
                    x.ConfigureHttpsDefaults(httpOptions =>
                    {
                        httpOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                    });
                }).UseStartup<Startup>();
            });
    }
}
