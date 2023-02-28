using HtmlAgilityPack;
using OpenQA.Selenium;
using System.ComponentModel;
using System.IO;
using Vacancies.Application.Drivers.Services;
using Vacancies.Application.Vacancies.Djinni.Interfaces;
using Vacancies.Application.Vacancies.Dto;

namespace Vacancies.Application.Vacancies.Djinni.Services
{
    public class ScrapingVacanciesDjinni : IScrapingVacanciesDjinni
    {
        private bool isFinish = false;
        private string _baseUrl = "https://djinni.co";
        private readonly IActivateDriver _activateDriver;

        public ScrapingVacanciesDjinni(IActivateDriver activateDriver)
        {
            _activateDriver = activateDriver;
        }

        public async Task<List<VacancyResponseModel>> ScrapVacanciesByDefaultUrl(string path, IWebDriver driver)
        {
            var vacancyReaponseModels = new List<VacancyResponseModel>();

            driver.Navigate().GoToUrl(path);
            await Task.Delay(2000);

            var allVacancies = await ScrollVacancies(driver);

            return vacancyReaponseModels;
        }

        public async Task<List<HtmlNode>> ScrollVacancies(IWebDriver driver)
        {
            List<HtmlNode>? vacancies = null;
            var vacancyResponseModel = new VacancyResponseModel();

            var paginationButton = driver.FindElements(By.XPath("//div[@class='d-md-none mb-3 text-center']"))?.FirstOrDefault();

            var htmlBlock = new HtmlDocument();
            htmlBlock.LoadHtml(driver.FindElements(By.XPath("//div[@class='row']/div[1]/ul"))
                    .FirstOrDefault()?.GetAttribute("outerHTML"));

            vacancies = htmlBlock.DocumentNode.FirstChild.ChildNodes.Where(node =>
                    node.EndNode.Name == "li").ToList();

            /// TODO Refactor it and replace it in ScrapVacanciesByDefaultUrl method
            foreach (var vacancy in vacancies)
            {
                var vacancyDjinniUrl = vacancy.SelectSingleNode("div[1]/div[2]/a").Attributes.FirstOrDefault(node =>
                        node.Name == "href")?.Value;
                if (vacancyDjinniUrl != null)
                {
                    var currentVacancyDriver = await _activateDriver.ActivateScrapingDriver();
                    currentVacancyDriver.Navigate().GoToUrl(_baseUrl + vacancyDjinniUrl);
                    await Task.Delay(500);

                    var currentDouUrl = currentVacancyDriver
                        .FindElements(By.XPath("//*[@class='container job-post-page']/div[2]/div[1]/div[2]/a[2]"))?
                        .FirstOrDefault()?.GetAttribute("href");

                    currentVacancyDriver.Navigate().GoToUrl(currentDouUrl);
                    await Task.Delay(500);

                    var currentOficeUrl = currentVacancyDriver
                            .FindElements(By.XPath("//*[@class='g-company-wrapper']/div[1]/ul/li[3]/a"))?
                            .FirstOrDefault()?.GetAttribute("href");

                    if (currentOficeUrl is null)
                    {
                        currentVacancyDriver.Quit();
                        continue;
                    }

                    currentVacancyDriver.Navigate().GoToUrl(currentOficeUrl);

                    var currentMapUrl = currentVacancyDriver
                            .FindElements(By.XPath("//*[@class='g-company-wrapper']/div[2]/div[2]/div[1]/div/div/div/div[2]/div[1]/div/div/div[1]/span/a"))?
                            .FirstOrDefault().GetAttribute("href");

                    currentVacancyDriver.Navigate().GoToUrl(currentMapUrl);
                    await Task.Delay(500);

                    var metaUrl = currentVacancyDriver
                        .FindElements(By.XPath("/html/head/meta[10]"))?
                        .FirstOrDefault().GetAttribute("content");

                    var firstPart = metaUrl.Substring(metaUrl.IndexOf("center=") + 7);
                    var coordinates = firstPart.Substring(0, firstPart.IndexOf("&zoom"));

                    var coordinatesWithoutDots = coordinates.Replace("%2C", "");

                    currentVacancyDriver.Quit();
                }

                var vacancyName = vacancy.SelectSingleNode("div[1]/div[2]/a/span").InnerText ??
                    vacancy.SelectSingleNode("div[1]/div[2]/a[2]/span").InnerText;

                var descriptionBlock = vacancy.SelectSingleNode("div[2]/div[1]").InnerText;

                var vacancyLocation = vacancy.SelectSingleNode("div[3]/div/div/span[3]").InnerText;

                vacancyResponseModel.Gid = Guid.NewGuid();
                vacancyResponseModel.Name = vacancyName;
                vacancyResponseModel.Description = descriptionBlock;
                vacancyResponseModel.Location = vacancyLocation;
            }

            //while (!isFinish)
            //{
            //    if (paginationButton != null)
            //    {
            //        paginationButton.Click();
            //        await Task.Delay(2500);
            //        paginationButton = driver.FindElements(By.XPath("//div[@class='d-md-none mb-3 text-center']"))?.FirstOrDefault();

            //        htmlBlock.LoadHtml(driver.FindElements(By.XPath("//div[@class='row']/div[1]/ul"))
            //            .FirstOrDefault()?.GetAttribute("outerHTML"));

            //        var currentVacanciesPull = htmlBlock.DocumentNode.FirstChild.ChildNodes.Where(node =>
            //            node.EndNode.Name == "li").ToList();

            //        foreach (var item in currentVacanciesPull)
            //        {
            //            vacancies.Add(item);
            //        }
            //    }
            //    else
            //    {
            //        isFinish = true;
            //    }
            //    var test = vacancies;
            //}
            driver.Quit();

            return vacancies;
        }
    }
}
