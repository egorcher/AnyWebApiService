using System;
using System.Net.Http;
using Mag.VisualizationLocation.Adapter.Client.Tests.Builders;
using Mag.VisualizationLocation.Adapter.Client.Tests.TestContext;
using Mag.VisualizationLocation.Adapter.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mag.VisualizationLocation.Adapter.Client.Tests
{
    public class ServiceBaseTest<TStartup> : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
    {
        protected HttpClient HttpClient;
        private readonly TestServer _testServer;
        public ServiceBaseTest()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            Configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(path)
                .UseStartup<TStartup>()
                .UseConfiguration(Configuration)
                .ConfigureServices(servises =>
                    {
                        servises.Configure<Source>(Configuration.GetSection(Source.OptionsName));
                        servises.Configure<TestDbConnection>(Configuration.GetSection(TestDbConnection.OptionName));
                        servises
                            .AddScoped<ITaskBuilder, TaskBuilder>()
                            .AddScoped<ITestMtbContext, TestMtbContext>()
                            .AddScoped<ITaskGrantBuilder, TaskGrantBuilder>()
                            .AddScoped<IAutoTaskBuilder, AutoTaskBuilder>()
                            .AddScoped<IMagAdapterClient>(x=>new MagAdapterClient(HttpClient));
                    });
            
            _testServer = new TestServer(builder);

            HttpClient = _testServer.CreateClient();

            ClearDb();
        }

        private void ClearDb()
        {
            var context = Resolve<ITestMtbContext>();
            context.ClearTables();
        }

        protected T Resolve<T>() => _testServer.Services.GetService<T>();

        public IConfigurationRoot Configuration { get; }

        public new void Dispose()
        {
            HttpClient.Dispose();
            _testServer.Dispose();
            base.Dispose();
        }
    }
}
