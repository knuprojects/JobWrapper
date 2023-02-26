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
        Task<List<VacancyReaponseModel>> ScrapVacanciesByDefaultUrl(string path, IWebDriver driver);

        Task<List<HtmlNode>> ScrollVacancies(IWebDriver driver);
    }
}
