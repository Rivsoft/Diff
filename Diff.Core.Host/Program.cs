using AutoMapper;
using Diff.Data;
using Diff.Data.Repositories;
using Diff.Integration.Config;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Diff.Core.IngestionService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true);
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
                        .AddSingleton<IHostedService, IngestionService>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
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
