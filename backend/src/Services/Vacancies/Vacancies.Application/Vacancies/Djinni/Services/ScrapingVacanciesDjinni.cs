using HtmlAgilityPack;
using OpenQA.Selenium;
using Vacancies.Application.Vacancies.Djinni.Interfaces;
using Vacancies.Application.Vacancies.Dto;

namespace Vacancies.Application.Vacancies.Djinni.Services
{
    public class ScrapingVacanciesDjinni : IScrapingVacanciesDjinni
    {
        private bool isFinish = false;

        public async Task<List<VacancyResponseModel>> ScrapVacanciesByDefaultUrl(string path, IWebDriver driver)
        {
            var vacancyReaponseModels = new List<VacancyResponseModel>();

            driver.Navigate().GoToUrl(path);
            await Task.Delay(2000);

            var allVacancies = await ScrollVacancies(driver);

            //foreach (var vacancy in allVacancies)
            //{
            //    var test = vacancy.SelectSingleNode("div[1]/div[2]/a");
            //    var second = test.Attributes.FirstOrDefault(node =>
            //            node.Name == "href")?.Value;

            //    var vacancyName = vacancy.SelectSingleNode("div[1]/div[2]/a[2]/span").InnerText;

            //    var descriptionBlock = vacancy.SelectSingleNode("div[2]/div[1]").InnerText;

            //    var vacancyResponseModel = new VacancyResponseModel
            //    {
            //        Gid = Guid.NewGuid(),
            //        Name = vacancy.Name
            //    };
            //}

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

            /// TODO Refactor it and replace it in ScrapVacanciesByDefaultUrl method
            foreach (var vacancy in vacancies)
            {
                var test = vacancy.SelectSingleNode("div[1]/div[2]/a");
                var second = test.Attributes.FirstOrDefault(node =>
                        node.Name == "href")?.Value;

                var vacancyName = vacancy.SelectSingleNode("div[1]/div[2]/a/span").InnerText ??
                    vacancy.SelectSingleNode("div[1]/div[2]/a[2]/span").InnerText;

                var descriptionBlock = vacancy.SelectSingleNode("div[2]/div[1]").InnerText;

                var vacancyLocation = vacancy.SelectSingleNode("div[3]/div/div/span[3]").InnerText;

                var vacancyResponseModel = new VacancyResponseModel
                {
                    Gid = Guid.NewGuid(),
                    Name = vacancyName,
                    Description = descriptionBlock,
                    Location = vacancyLocation
                };
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
