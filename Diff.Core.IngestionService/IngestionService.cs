using Diff.Core.Integration.Messages;
using Diff.Data.Models;
using Diff.Data.Repositories;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diff.Core.IngestionService
{
    public class IngestionService : IHostedService, IDisposable
    {
        private readonly ILogger<IngestionService> _logger;
        private IBus _serviceBus;
        private readonly IServiceScopeFactory _scopeFactory;

        public IngestionService(IServiceScopeFactory scopeFactory, ILogger<IngestionService> logger, IBus serviceBus)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting the Ingestion Service background job...");

            _serviceBus.SubscribeAsync<AddDiffInputMessage>(typeof(IngestionService).ToString(), HandleIngestionInput);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping the Ingestion Service background job...");

            _serviceBus?.Dispose();

            return Task.CompletedTask;
        }

        private async Task HandleIngestionInput(AddDiffInputMessage input)
        {
            _logger.LogInformation($"Processing Input: {input.Id}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var analysisRepo = scope.ServiceProvider.GetRequiredService<IDiffAnalysisRepository>();
                //var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var analysis = await analysisRepo.GetAnalysis(input.Id);

                var persistChanges = false;

                //If analysis doesnt exists in the DB
                if (analysis == null)
                {
                    _logger.LogInformation($"Analysis Not Found: {input.Id}");

                    analysis = new DiffAnalysis() { Id = input.Id };
                    analysisRepo.Add(analysis);
                    persistChanges = true;
                }

                // if analysis already analyzed we discard the input
                if (analysis.Analyzed)
                {
                    _logger.LogInformation($"Analysis already analyzed: {input.Id}. Discarding...");
                    return;
                }

                // If input is left argument and doesnt already exist
                if (input.IsLeft && analysis.Left == null)
                {
                    _logger.LogInformation($"Persisting Left Argument: {input.Id}");

                    analysis.Left = input.Input;
                    persistChanges = true;
                }
                
                // If input is right argument and doesnt already exist
                if (!input.IsLeft && analysis.Right == null)
                {
                    _logger.LogInformation($"Persisting Right Argument: {input.Id}");

                    analysis.Right = input.Input;
                    persistChanges = true;
                }

                if (persistChanges)
                    await analysisRepo.SaveAll();

                // if we have both left and right arguments we publish a perform Diff Analysis message
                if (analysis.Left != null && analysis.Right != null)
                {
                    _logger.LogInformation($"Analysis ready to be analyzed: {input.Id}. Publishing PerformDiffAnalysisMessage to bus.");

                    var message = new PerformDiffAnalysisMessage(analysis.Id);
                    await _serviceBus.PublishAsync(message);
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