using Diff.Core.Integration.Messages;
using Diff.Core.Interfaces;
using Diff.Data.Repositories;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diff.Core.AnalysisService
{
    public class AnalysisService : IHostedService, IDisposable
    {
        private readonly ILogger<AnalysisService> _logger;
        private IBus _serviceBus;
        private readonly IServiceScopeFactory _scopeFactory;

        public AnalysisService(IServiceScopeFactory scopeFactory, ILogger<AnalysisService> logger, IBus serviceBus)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting the Analysis Service background job...");

            _serviceBus.SubscribeAsync<PerformDiffAnalysisMessage>(typeof(AnalysisService).ToString(), HandleDiffAnalysis);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping the Analysis Service background job...");

            _serviceBus?.Dispose();

            return Task.CompletedTask;
        }

        private async Task HandleDiffAnalysis(PerformDiffAnalysisMessage input)
        {
            _logger.LogInformation($"Processing Analysis Id: {input.Id}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var analysisRepo = scope.ServiceProvider.GetRequiredService<IDiffAnalysisRepository>();
                var analizer = scope.ServiceProvider.GetRequiredService<IDiffAnalyzer>();

                var analysis = await analysisRepo.GetAnalysis(input.Id);

                //If analysis doesnt exists in the DB
                if (analysis == null)
                {
                    _logger.LogWarning($"Analysis Not Found: {input.Id}");
                    return;
                }

                // if analysis already analyzed we discard the input
                if (analysis.Analyzed)
                {
                    _logger.LogWarning($"Analysis already processed: {input.Id}");
                    return;
                }

                // if we have both left and right arguments we publish a perform Diff Analysis message
                if (analysis.Left != null && analysis.Right != null)
                {
                    _logger.LogInformation($"Performing Analysis: {input.Id}");

                    // Generate the diff for this analysis
                    var result = analizer.GenerateDiff(analysis.Left, analysis.Right);

                    _logger.LogInformation($"Found {result.Segments.Count} diff segments");

                    // Map analized Diff Segments to the existing Analysis DB model
                    result.Segments.ForEach(s => 
                        analysis.Segments.Add(
                            new Data.Models.DiffSegment() { Offset = s.Offset, Length = s.Length }));

                    // Mark the analysis as completed.
                    analysis.Analyzed = true;

                    // Save the resulting analysis entity to the DB.
                    await analysisRepo.SaveAll();

                    _logger.LogInformation($"Analysis completed: {input.Id}");
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _serviceBus?.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}