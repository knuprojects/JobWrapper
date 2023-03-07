using HtmlAgilityPack;
using OpenQA.Selenium;
using Vacancies.Core.Consts;
using Vacancies.Core.Helpers;
using Vacancies.Core.Responses;

namespace Vacancies.Core.Services;

public interface IScrapperService
{
    //TODO: ReadOnly
    ValueTask<List<VacancyResponse>> ScrapVacanciesByUrl(string path, IWebDriver driver);
}

public class ScrapperService : IScrapperService
{
    private bool isFinish = false;
    private readonly IActivateDriver _activateDriver;

    public async ValueTask<List<VacancyResponse>> ScrapVacanciesByUrl(string path, IWebDriver driver)
    {
        driver.Navigate().GoToUrl(path);

        await Task.Delay(500);

        var allVacancies = await ScrollVacancies(path, driver);

        return allVacancies;
    }

    public async ValueTask<List<VacancyResponse>> ScrollVacancies(string path, IWebDriver driver)
    {
        List<HtmlNode>? vacancies = null;

        var response = new List<VacancyResponse>();

        string coordinates;

        var paginationButton = driver.FindElements(By.XPath("//div[@class='d-md-none mb-3 text-center']"))?.FirstOrDefault();

        var htmlBlock = new HtmlDocument();
        htmlBlock.LoadHtml(driver.FindElements(By.XPath("//div[@class='row']/div[1]/ul"))
                .FirstOrDefault()?.GetAttribute("outerHTML"));

        vacancies = htmlBlock.DocumentNode.FirstChild.ChildNodes.Where(node =>
                node.EndNode.Name == "li").ToList();

        response = await FindVacancies(vacancies, path);

        while (!isFinish)
        {
            if (paginationButton != null)
            {
                paginationButton.Click();
                await Task.Delay(2500);
                paginationButton = driver.FindElements(By.XPath("//div[@class='d-md-none mb-3 text-center']"))?.FirstOrDefault();

                htmlBlock.LoadHtml(driver.FindElements(By.XPath("//div[@class='row']/div[1]/ul"))
                    .FirstOrDefault()?.GetAttribute("outerHTML"));

                var currentVacanciesPull = htmlBlock.DocumentNode.FirstChild.ChildNodes.Where(node =>
                    node.EndNode.Name == "li").ToList();

                foreach (var item in currentVacanciesPull)
                {
                    vacancies.Add(item);
                }

                response = await FindVacancies(vacancies, path);
            }
            else
                isFinish = true;
        }

        driver.Quit();

        return response;
    }

    private async ValueTask<List<VacancyResponse>> FindVacancies(List<HtmlNode>? vacancies, string baseSiteUrl)
    {
        var vacancyResponseModels = new List<VacancyResponse>();
        string coordinates = null;
        string salary = null;

        foreach (var vacancy in vacancies)
        {
            var vacancyDjinniUrl = vacancy.SelectSingleNode("div[1]/div[2]/a").Attributes.FirstOrDefault(node =>
                    node.Name == "href")?.Value;

            if (vacancyDjinniUrl != null)
            {
                var currentVacancyDriver = await _activateDriver.ActivateScrapingDriver();
                currentVacancyDriver.Navigate().GoToUrl(VacanciesConsts.DjinniUrl + vacancyDjinniUrl);
                await Task.Delay(500);

                var salaryBlock = currentVacancyDriver.FindElements(By.XPath("//*[@class='public-salary-item']"))?
                    .FirstOrDefault();


                var currentDouUrl = currentVacancyDriver
                    .FindElements(By.XPath("//*[@class='container job-post-page']/div[2]/div[1]/div[2]/a[2]"))?
                    .FirstOrDefault()?.GetAttribute("href");

                if (currentDouUrl is null)
                {
                    continue;
                }

                currentVacancyDriver.Navigate().GoToUrl(currentDouUrl);
                await Task.Delay(500);

                var companyNavBlock = new HtmlDocument();
                companyNavBlock.LoadHtml(currentVacancyDriver.FindElements(By.XPath("//*[@class='company-nav']"))
                        .FirstOrDefault()?.GetAttribute("outerHTML"));

                var companyNavListLi = companyNavBlock.DocumentNode.FirstChild.ChildNodes.Where(node =>
                        node.EndNode.Name == "li").ToList();

                string companyNavInnerText = null;
                string currentOficeUrl = null;

                foreach (var companyNav in companyNavListLi)
                {
                    companyNavInnerText = companyNav.SelectSingleNode("a").InnerText;
                    if (companyNavInnerText == "Офіси")
                    {
                        currentOficeUrl = companyNav.SelectSingleNode("a").Attributes.FirstOrDefault(node =>
                            node.Name == "href")?.Value;

                        if (currentOficeUrl is null)
                        {
                            currentVacancyDriver.Quit();
                            continue;
                        }

                        currentVacancyDriver.Navigate().GoToUrl(currentOficeUrl);
                        await Task.Delay(500);
                        break;
                    }
                }

                var currentMapUrl = currentVacancyDriver
                        .FindElements(By.XPath("//*[@class='g-company-wrapper']/div[2]/div[2]/div[1]/div/div/div/div[2]/div[1]/div/div/div[1]/span/a"))?
                        .FirstOrDefault()?.GetAttribute("href") ??
                        currentVacancyDriver
                        .FindElements(By.XPath("//*[@class='g-company-wrapper']/div[2]/div[2]/div[1]/div/div/div[1]/div[2]/div/div/div[1]/span/a"))?
                        .FirstOrDefault()?.GetAttribute("href");

                if (currentMapUrl is null)
                {
                    currentVacancyDriver.Quit();
                    continue;
                }

                currentVacancyDriver.Navigate().GoToUrl(currentMapUrl);
                await Task.Delay(500);

                var metaUrl = currentVacancyDriver
                    .FindElements(By.XPath("/html/head/meta[10]"))?
                    .FirstOrDefault().GetAttribute("content");

                var firstPart = metaUrl.Substring(metaUrl.IndexOf("center=") + 7);

                coordinates = firstPart.Substring(0, firstPart.IndexOf("&zoom"));

                currentVacancyDriver.Quit();
            }

            var vacancyName = vacancy.SelectSingleNode("div[1]/div[2]/a/span").InnerText ??
                vacancy.SelectSingleNode("div[1]/div[2]/a[2]/span").InnerText;

            var descriptionBlock = vacancy.SelectSingleNode("div[2]/div[1]").InnerText;

            var response = new VacancyResponse(
                Guid.NewGuid(),
                vacancyName,
                descriptionBlock,
                coordinates,
                null);

            vacancyResponseModels.Add(response);
        }

        return vacancyResponseModels;
    }
}
