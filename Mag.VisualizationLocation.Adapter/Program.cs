using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Linq;

namespace Mag.VisualizationLocation.Adapter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            var logger = (ILogger)webHost.Services.GetService(typeof(ILogger));
            try
            {
                var runAsService = (args.Contains("s") || args.Contains("service")) && !(Debugger.IsAttached || args.Contains("c"));
                if (runAsService)
                {
                    webHost.RunAsCustomService();
                }
                else
                {
                    webHost.Run();
                }
            }
            catch (Exception err)
            {
                logger.Error(err.Message, err);
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
#if DEBUG
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location);
            var environment = "Development";
#else
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var path = Path.GetDirectoryName(pathToExe);
            var environment = "Production";
#endif
            Directory.SetCurrentDirectory(path);

            var config = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
                .Build();

            var host = WebHost.CreateDefaultBuilder(args)
                .UseEnvironment(environment)
                .UseConfiguration(config)
                .ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    cfg.SetBasePath(path)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .UseStartup<Startup>()
                .UseSerilog(
                    (hostingContext, loggerConfiguration)
                        => loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration))
                .Build();

            return host;
        }
    }
}
