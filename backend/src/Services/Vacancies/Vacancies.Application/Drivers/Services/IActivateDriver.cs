using OpenQA.Selenium;

namespace Vacancies.Application.Drivers.Services
{
    public interface IActivateDriver
    {
        Task<IWebDriver> ActivateScrapingDriver();
    }
}
