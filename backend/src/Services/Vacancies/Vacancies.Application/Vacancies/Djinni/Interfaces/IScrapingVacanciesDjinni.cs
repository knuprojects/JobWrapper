using OpenQA.Selenium;
using Vacancies.Application.Vacancies.Dto;

namespace Vacancies.Application.Vacancies.Djinni.Interfaces
{
    public interface IScrapingVacanciesDjinni
    {
        Task<List<VacancyResponseModel>> ScrapVacanciesByUrl(string path, IWebDriver driver);

        Task<List<VacancyResponseModel>> ScrollVacancies(string path, IWebDriver driver);
    }
}
