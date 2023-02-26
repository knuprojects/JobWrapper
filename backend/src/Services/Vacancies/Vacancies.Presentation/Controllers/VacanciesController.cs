using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vacancies.Application.Drivers.Services;

namespace Vacancies.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacanciesController : ControllerBase
    {
        private readonly IActivateDriver _activateDriver;
        public VacanciesController(IActivateDriver activateDriver)
        {
            _activateDriver = activateDriver;
        }

        [HttpGet("scrape")]
        public async Task<IActionResult> ScrapeVacancies()
        {
            var result = _activateDriver.ActivateScrapingDriver();
            return Ok(result);
        }
    }
}
