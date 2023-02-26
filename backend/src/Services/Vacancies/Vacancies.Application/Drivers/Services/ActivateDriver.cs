using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Vacancies.Application.Drivers.Services
{
    public class ActivateDriver : IActivateDriver
    {
        public Task<IWebDriver> ActivateScrapingDriver()
        {
            var pathWebDriver = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\Vacancies.Core\\Utils";
            var options = new ChromeOptions();

            //options.AddArguments("--disable-gpu");
            //options.AddArguments("start-maximized");
            //options.AddArguments("--allow-insecure-localhost");
            //options.AddArguments("--allow-running-insecure-content");
            //options.AddArguments("--ignore-certificate-errors");
            //options.AddArguments("--no-sandbox");
            options.AddArguments("--window-size=1280,1000");

            IWebDriver driver = new ChromeDriver(pathWebDriver, options);

            return Task.FromResult(driver);
        }
    }
}
