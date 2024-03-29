﻿using HtmlAgilityPack;
using OpenQA.Selenium;
using Shared.Dal.Repositories;
using Shared.Dal.Utils;
using Vacancies.Core.Common.Helpers;
using Vacancies.Core.Consts;
using Vacancies.Core.Entities;

namespace Vacancies.Core.Services;

public interface IScrapperService
{
    ValueTask<List<Vacancy>> ScrapVacanciesByUrl(string path, IWebDriver driver, CancellationToken cancellationToken);
}

public class ScrapperService : IScrapperService
{
    private bool isFinish = false;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivateDriver _activateDriver;
    private readonly IElementFinder _elementFinder;
    private readonly IBaseRepository _repository;

    public ScrapperService(
        IUnitOfWork unitOfWork,
        IActivateDriver activateDriver,
        IElementFinder elementFinder,
        IBaseRepository repository
        )
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _activateDriver = activateDriver ?? throw new ArgumentNullException(nameof(activateDriver));
        _elementFinder = elementFinder ?? throw new ArgumentNullException(nameof(elementFinder));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async ValueTask<List<Vacancy>> ScrapVacanciesByUrl(string path, IWebDriver driver, CancellationToken cancellationToken)
    {
        driver.Navigate().GoToUrl(path);

        await Task.Delay(200);

        return await ScrollVacancies(driver, cancellationToken);
    }

    public async ValueTask<List<Vacancy>> ScrollVacancies(IWebDriver driver, CancellationToken cancellationToken)
    {
        var paginationButton = _elementFinder.FindElementsByXpath(driver, XpathConsts.Xpath.PaginationButton);

        if (paginationButton is null)
            return await FillVacanciesAsync(driver, cancellationToken);

        var items = await FillVacanciesAsync(driver, cancellationToken);

        items = await FillWithPaginationAsync(driver, paginationButton, cancellationToken);

        return items;
    }

    private async ValueTask<List<Vacancy>> FindVacancies(List<HtmlNode>? vacancies, CancellationToken cancellationToken)
    {
        var vacanciesResponse = new List<Vacancy>();
        string coordinates = "";
        string salary = null;

        foreach (var vacancy in vacancies)
        {

            var skills = new List<string>();
            var additionalDjinniPath = _elementFinder.FindSingleNodeByXpathAndElement(vacancy, XpathConsts.Xpath.DjinniCurrentVacancy, XpathConsts.ElementsToFind.AttributeToFind);

            if (additionalDjinniPath is not null)
            {
                var driver = await ActivateScrappingAsync(VacanciesConsts.DjinniUrl, additionalDjinniPath);

                var addtitionalDjiniInfoElements = _elementFinder.FindFirstElementByByDjinniAdditionalInfo(driver, XpathConsts.Xpath.DjinniAdditionalInfo);

                var douUri = _elementFinder.FindElementsByXpathAndAttribute(driver, XpathConsts.Xpath.DouCurrentUri, XpathConsts.ElementsToFind.AttributeToFind) ??
                             _elementFinder.FindElementsByXpathAndAttribute(driver, XpathConsts.Xpath.DouAdditionalCurrentUri, XpathConsts.ElementsToFind.AttributeToFind);

                if (douUri is null) { driver.Quit(); continue; }

                var skillsListElements = _elementFinder.FindListOfElementsByByDjinniAdditionalInfo(addtitionalDjiniInfoElements, XpathConsts.Xpath.DjinniAdditionalInfoElement);


                foreach (var skillElement in skillsListElements)
                {
                    var skill = _elementFinder.FindElementByClassWithAttribute(skillElement, XpathConsts.Xpath.DjinniAdditionalInfoElementText, XpathConsts.Xpath.DjinniInnerText);
                    skills.Add(skill.ToString());
                }

                salary = _elementFinder.FindElementsByXpathAndAttribute(driver, XpathConsts.Xpath.DjinniAdditionalSalary, XpathConsts.Xpath.DjinniInnerText);


                await GoToUriAsync(driver, douUri);

                var douDocument = _elementFinder.LoadHtmlByXpathAndAttribute(driver, XpathConsts.Xpath.DouNavBlockUri, XpathConsts.ElementsToFind.HtmlDocAttributeToFind);

                var douNavBlockElementsList = _elementFinder.FindChildNodesByElement(douDocument, XpathConsts.ElementsToFind.ListElementToFind);

                foreach (var navNode in douNavBlockElementsList)
                {
                    var innerTextOfElementInNavBlock = _elementFinder.FindSingleNodeInnerTextByXpath(navNode, XpathConsts.ElementsToFind.ElementInnerTextToFind);

                    if (innerTextOfElementInNavBlock is "Офіси")
                    {
                        var currentOfficeUri = _elementFinder.FindSingleNodeByXpathAndElement(navNode, XpathConsts.ElementsToFind.ElementInnerTextToFind, XpathConsts.ElementsToFind.AttributeToFind);

                        if (currentOfficeUri is null) { driver.Quit(); continue; }

                        await GoToUriAsync(driver, currentOfficeUri);

                        break;
                    }
                }

                var currentMapUri = _elementFinder.FindElementsByXpathAndAttribute(driver, XpathConsts.Xpath.DouMapDefaultUri, XpathConsts.ElementsToFind.AttributeToFind) ??
                                    _elementFinder.FindElementsByXpathAndAttribute(driver, XpathConsts.Xpath.DouMapAdditionalUri, XpathConsts.ElementsToFind.AttributeToFind);

                if (currentMapUri is null)
                {
                    driver.Quit();
                    continue;
                }

                await GoToUriAsync(driver, currentMapUri);

                var metaUri = _elementFinder.FindElementsByXpathAndAttribute(driver, XpathConsts.Xpath.DouMapMetaContent, XpathConsts.ElementsToFind.AttributeContentToFind) ??
                              _elementFinder.FindElementsByXpathAndAttribute(driver, XpathConsts.Xpath.DouMapAdditionalMetaContent, XpathConsts.ElementsToFind.AttributeContentToFind);

                coordinates = SplitUri(ref metaUri);

                driver.Quit();
            }

            var vacancyName = _elementFinder.FindSingleNodeInnerTextByXpath(vacancy, XpathConsts.Xpath.DjinniVacancyName) ??
                              _elementFinder.FindSingleNodeInnerTextByXpath(vacancy, XpathConsts.Xpath.DjinniAdditionalVacancyName);

            var vacancyToDb = new Vacancy(
                vacancyName,
                skills,
                coordinates,
                salary ?? ""
                );

            vacanciesResponse.Add(vacancyToDb);
        }

        _repository.AddMany<Vacancy>(vacanciesResponse);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return vacanciesResponse;
    }

    private List<HtmlNode?> ReloadDocument(IWebDriver driver)
    {
        var document = _elementFinder.LoadHtmlByXpathAndAttribute(driver, XpathConsts.Xpath.HtmlDoc, XpathConsts.ElementsToFind.HtmlDocAttributeToFind);

        var items = _elementFinder.FindChildNodesByElement(document, XpathConsts.ElementsToFind.ListElementToFind);

        return items;
    }

    private List<HtmlNode?> SetVacancies(IWebDriver driver)
    {
        var document = _elementFinder.LoadHtmlByXpathAndAttribute(driver, XpathConsts.Xpath.HtmlDoc, XpathConsts.ElementsToFind.HtmlDocAttributeToFind);

        return _elementFinder.FindChildNodesByElement(document, XpathConsts.ElementsToFind.ListElementToFind);
    }

    private async ValueTask<List<Vacancy>> FillVacanciesAsync(IWebDriver driver, CancellationToken cancellationToken)
    {
        var vacancies = SetVacancies(driver);

        return await FindVacancies(vacancies, cancellationToken);
    }

    private async ValueTask<List<Vacancy>> FillWithPaginationAsync(IWebDriver driver, IWebElement paginationButton, CancellationToken cancellationToken)
    {
        var vacancies = new List<HtmlNode?>();
        var response = new List<Vacancy>();

        while (!isFinish)
        {
            if (paginationButton is not null)
            {
                paginationButton.Click();
                await Task.Delay(200);

                paginationButton = _elementFinder.FindElementsByXpath(driver, XpathConsts.Xpath.PaginationButton);

                var items = ReloadDocument(driver);

                if (items.Any())
                    foreach (var item in items)
                        vacancies.Add(item);

                response = await FindVacancies(vacancies, cancellationToken);
            }
            else
                isFinish = true;
        }
        driver.Quit();

        return response;
    }

    private async ValueTask<IWebDriver?> ActivateScrappingAsync(string path, string additionalPath)
    {
        var driver = await _activateDriver.ActivateScrapingDriver();

        driver.Navigate().GoToUrl(path + additionalPath);

        await Task.Delay(200);

        return driver;
    }

    private async ValueTask GoToUriAsync(IWebDriver? driver, string uri)
    {
        driver?.Navigate().GoToUrl(uri);

        await Task.Delay(200);
    }

    private string SplitUri(ref string uri)
    {
        var firstPart = uri.Substring(uri.IndexOf("center=") + 7);

        return firstPart.Substring(0, firstPart.IndexOf("&zoom"));
    }
}
