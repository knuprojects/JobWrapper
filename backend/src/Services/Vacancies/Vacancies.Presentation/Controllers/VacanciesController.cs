using Microsoft.AspNetCore.Mvc;
using Vacancies.Core.Helpers;
using Vacancies.Core.Services;

namespace Vacancies.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VacanciesController : ControllerBase
{
    private readonly IActivateDriver _activateDriver;
    private readonly IScrapperService _scrapperService;
    public VacanciesController(
        IActivateDriver activateDriver,
        IScrapperService scrapperService)
    {
        _activateDriver = activateDriver ?? throw new ArgumentNullException(nameof(activateDriver));
        _scrapperService = scrapperService ?? throw new ArgumentNullException(nameof(scrapperService));
    }

    [HttpGet("scrape")]
    public async Task<IActionResult> ScrapeVacancies(string path)
    {
        var driver = await _activateDriver.ActivateScrapingDriver();

        var result = await _scrapperService.ScrapVacanciesByUrl(path, driver);

        return Ok(result);
    }
}