using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacancies.Application.Vacancies.Dto;

namespace Vacancies.Application.Vacancies.Djinni.Interfaces
{
    public interface IScrapingVacanciesDjinni
    {
        Task<List<VacancyResponseModel>> ScrapVacanciesByDefaultUrl(string path, IWebDriver driver);

        Task<List<VacancyResponseModel>> ScrollVacancies(IWebDriver driver);
    }
}
