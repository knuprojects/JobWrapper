using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vacancies.Core.Common.Helpers;
using Vacancies.Core.Common.Options;
using Vacancies.Core.Services;

namespace Vacancies.Core.Common.BackgroundServices;

public class ScrapperBackgroundService : BackgroundService
{
    private readonly ILogger<ScrapperBackgroundService> _logger;
    private readonly DjinniOptions _djinniOptions;
    private readonly IServiceProvider _serviceProvider;

    public ScrapperBackgroundService(
        ILogger<ScrapperBackgroundService> logger,
        DjinniOptions djinniOptions,
        IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _djinniOptions = djinniOptions ?? throw new ArgumentNullException(nameof(djinniOptions));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            try
            {
                var activateDriver = scope.ServiceProvider.GetRequiredService<IActivateDriver>();

                var driver = await activateDriver.ActivateScrapingDriver();

                var scrapperService = scope.ServiceProvider.GetRequiredService<IScrapperService>();

                await scrapperService.ScrapVacanciesByUrl(_djinniOptions.DjinniUrl, driver);

                _logger.LogInformation("The scrapping was done!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"The scrapping was failed: {ex.Message}");
            }
        }
    }
}
