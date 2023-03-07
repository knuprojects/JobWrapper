using Microsoft.AspNetCore.Mvc;
using Vacancies.Application.Drivers.Services;
using Vacancies.Application.Vacancies.Djinni.Interfaces;

namespace Vacancies.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VacanciesController : ControllerBase
{
    private readonly IActivateDriver _activateDriver;
    private readonly IScrapingVacanciesDjinni _scrapingVacanciesDjinni;
    public VacanciesController(IActivateDriver activateDriver,
        IScrapingVacanciesDjinni scrapingVacanciesDjinni)
    {
        _activateDriver = activateDriver;
        _scrapingVacanciesDjinni = scrapingVacanciesDjinni;
    }

    [HttpGet("scrape")]
    public async Task<IActionResult> ScrapeVacancies(string path)
    {
        var driver = await _activateDriver.ActivateScrapingDriver();
        var result = await _scrapingVacanciesDjinni.ScrapVacanciesByUrl(path, driver);
        return Ok(result);
    }
}