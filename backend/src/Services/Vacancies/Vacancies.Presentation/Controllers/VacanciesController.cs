using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using Vacancies.Application.Drivers.Services;
using Vacancies.Application.Vacancies.Djinni.Interfaces;

namespace Vacancies.Presentation.Controllers
{
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
        public async Task<IActionResult> ScrapeVacancies()
        {
            var defaultUrl = "https://djinni.co/jobs/?exp_level=1y";
            var driver = await _activateDriver.ActivateScrapingDriver();
            var result = _scrapingVacanciesDjinni.ScrapVacanciesByDefaultUrl(defaultUrl, driver);
            return Ok(result);
        }
    }
}
