using System.Reflection;
using System.ServiceProcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Serilog;

namespace Mag.VisualizationLocation.Adapter
{
    internal class CustomWebHostService : WebHostService
    {
        private readonly ILogger _logger;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            _logger = (ILogger)host.Services.GetService(typeof(ILogger));
        }

        protected override void OnStarting(string[] args)
        {
            _logger.Information($"------- Запуск (версия {Assembly.GetExecutingAssembly().GetName().Version}) -------");
            base.OnStarting(args);
        }

        protected override void OnStopping()
        {
            _logger.Information("------- Закрытие -------");
            base.OnStopping();
        }
    }

    public static class CustomWebHostWindowsServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }
}
