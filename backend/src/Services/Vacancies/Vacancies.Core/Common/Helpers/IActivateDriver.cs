using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Vacancies.Core.Common.Helpers;

public interface IActivateDriver
{
    ValueTask<IWebDriver> ActivateScrapingDriver();
}

public class ActivateDriver : IActivateDriver
{
    public ValueTask<IWebDriver> ActivateScrapingDriver()
    {
        var pathWebDriver = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\Vacancies.Core\\Common\\Utils";

        var options = new ChromeOptions();

        //options.AddArguments("--disable-gpu");
        //options.AddArguments("start-maximized");
        //options.AddArguments("--allow-insecure-localhost");
        //options.AddArguments("--allow-running-insecure-content");
        //options.AddArguments("--ignore-certificate-errors");
        //options.AddArguments("--no-sandbox");
        options.AddArguments("--window-size=770,1000");

        IWebDriver driver = new ChromeDriver(pathWebDriver, options);

        return ValueTask.FromResult(driver);
    }
}
