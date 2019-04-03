using AutoMapper;
using Diff.Core.Integration.Config;
using Diff.Core.Interfaces;
using Diff.Data;
using Diff.Data.Repositories;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Diff.Core.AnalysisService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddCustomDbContext(hostContext.Configuration)
                        .AddCustomConfiguration(hostContext.Configuration)
                        .AddAutoMapper()
                        .AddEventBus(hostContext.Configuration)
                        .AddScoped<IDiffAnalysisRepository, DiffAnalysisRepository>()
                        .AddScoped<IDiffAnalyzer, DiffAnalyzer>()
                        .AddSingleton<IHostedService, AnalysisService>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBus>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<EventBusConfig>>();
                var serviceBus = RabbitHutch.CreateBus(settings.Value.RabbitMqConnectionString);

                return serviceBus;
            });

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<EventBusConfig>(configuration.GetSection("EventBusConfig"));

            return services;
        }
    }
}
