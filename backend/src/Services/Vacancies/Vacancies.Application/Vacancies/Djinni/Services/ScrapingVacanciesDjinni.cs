using HtmlAgilityPack;
using OpenQA.Selenium;
using Vacancies.Application.Vacancies.Djinni.Interfaces;
using Vacancies.Application.Vacancies.Dto;

namespace Vacancies.Application.Vacancies.Djinni.Services
{
    public class ScrapingVacanciesDjinni : IScrapingVacanciesDjinni
    {
        private bool isFinish = false;

        public async Task<List<VacancyReaponseModel>> ScrapVacanciesByDefaultUrl(string path, IWebDriver driver)
        {
            var vacancyReaponseModels = new List<VacancyReaponseModel>();

            driver.Navigate().GoToUrl(path);
            await Task.Delay(2000);

            var allVacancies = ScrollVacancies(driver);

            return vacancyReaponseModels;
        }

        public async Task<List<HtmlNode>> ScrollVacancies(IWebDriver driver)
        {
            List<HtmlNode>? vacancies = null;

            var paginationButton = driver.FindElements(By.XPath("//div[@class='d-md-none mb-3 text-center']"))?.FirstOrDefault();

            var htmlBlock = new HtmlDocument();
            htmlBlock.LoadHtml(driver.FindElements(By.XPath("//div[@class='row']/div[1]/ul"))
                    .FirstOrDefault()?.GetAttribute("outerHTML"));

            vacancies = htmlBlock.DocumentNode.FirstChild.ChildNodes.Where(node =>
                    node.EndNode.Name == "li").ToList();

            foreach( var node in vacancies )
            {
                var test = node.SelectSingleNode("div[1]/div[2]/a");
                var second = test.Attributes.FirstOrDefault(node =>
                        node.Name == "href")?.Value;
            }

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
                }
                else
                {
                    isFinish = true;
                }
                var test = vacancies;
            }
            driver.Quit();

            return vacancies;
        }
    }
}
