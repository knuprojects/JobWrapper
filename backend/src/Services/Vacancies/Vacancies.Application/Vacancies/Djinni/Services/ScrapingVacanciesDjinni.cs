using HtmlAgilityPack;
using OpenQA.Selenium;
using Vacancies.Application.Vacancies.Djinni.Interfaces;
using Vacancies.Application.Vacancies.Dto;

namespace Vacancies.Application.Vacancies.Djinni.Services
{
    public class ScrapingVacanciesDjinni : IScrapingVacanciesDjinni
    {
        public async Task<List<VacancyReaponseModel>> ScrapVacanciesByDefaultUrl(string path, IWebDriver driver)
        {
            var vacancyReaponseModels = new List<VacancyReaponseModel>();

            driver.Navigate().GoToUrl(path);
            await Task.Delay(4000);

            var allVacancies = ScrollVacancies(driver);

            return vacancyReaponseModels;
        }

        public async Task<List<HtmlNode>> ScrollVacancies(IWebDriver driver)
        {
            List<HtmlNode>? vacancies = null;

            var htmlBlock = new HtmlDocument();
            htmlBlock.LoadHtml(driver.FindElements(By.XPath("//*[@class='list-unstyled list-jobs']"))
                    .FirstOrDefault()?.GetAttribute("outerHTML"));

            vacancies = htmlBlock.DocumentNode.FirstChild.ChildNodes.Where(node =>
                    node.Attributes.FirstOrDefault(attr => attr.Name == "ul").Value
                   .Contains("DivMoreContainer")).ToList();

            return vacancies;
        }
    }
}
