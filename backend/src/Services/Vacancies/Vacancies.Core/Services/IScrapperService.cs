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
    private readonly IElementFinder _elementFinder;

    public ScrapperService(
        IActivateDriver activateDriver,
        IElementFinder elementFinder)
    {
        _activateDriver = activateDriver ?? throw new ArgumentNullException(nameof(activateDriver));
        _elementFinder = elementFinder ?? throw new ArgumentNullException(nameof(elementFinder));
    }

    public async ValueTask<List<VacancyResponse>> ScrapVacanciesByUrl(string path, IWebDriver driver)
    {
        driver.Navigate().GoToUrl(path);

        await Task.Delay(500);

        var allVacancies = await ScrollVacancies(path, driver);

        return allVacancies;
    }

    public async ValueTask<List<VacancyResponse>> ScrollVacancies(string path, IWebDriver driver)
    {
        List<VacancyResponse> response = new List<VacancyResponse>();
        List<HtmlNode>? vacancies = new List<HtmlNode>();

        IWebElement? paginationButton = _elementFinder.FindElementsByXpath(ref XpathConsts.Xpath.PaginationButton);

        if (paginationButton is null)
            return await FillVacancies();

        while (!isFinish)
        {
            if (paginationButton != null)
            {
                paginationButton.Click();

                await Task.Delay(2500);

                paginationButton = _elementFinder.FindElementsByXpath(ref XpathConsts.Xpath.PaginationButton);

                HtmlDocument document = _elementFinder.LoadHtmlByXpathAndAttribute(ref XpathConsts.Xpath.HtmlDoc, ref XpathConsts.ElementsToFind.HtmlDocAttributeToFind);

                List<HtmlNode>? vacanciesPull = _elementFinder.FindChildNodesByElement(document, XpathConsts.ElementsToFind.ListElementToFind);

                if (vacanciesPull.Any())
                {
                    foreach (var item in vacanciesPull)
                        vacancies.Add(item);
                }

                response = await FindVacancies(vacancies);
            }
            else
                isFinish = true;
        }

        driver.Quit();

        return response;
    }

    private async ValueTask<List<VacancyResponse>> FindVacancies(List<HtmlNode>? vacancies)
    {
        var vacancyResponse = new List<VacancyResponse>();
        string coordinates = "";
        //string salary;

        foreach (var vacancy in vacancies)
        {
            var additionalDjinniPath = _elementFinder.FindSingleNodeByXpathAndElement(vacancy, ref XpathConsts.Xpath.CurrentDjinniVacancy, XpathConsts.ElementsToFind.AttributeToFind);

            if (additionalDjinniPath != null)
            {
                var driver = await ActivateScrapping(VacanciesConsts.DjinniUrl, additionalDjinniPath);

                //var salaryBlock = _elementFinder.FindElementsByXpath("//*[@class='public-salary-item']");

                var douUri = _elementFinder.FindElementsByXpathAndAttribute(driver, ref XpathConsts.Xpath.CurrentDouUri, ref XpathConsts.ElementsToFind.AttributeToFind);

                if (douUri is null) continue;

                await GoToUri(driver, douUri);

                var douDocument = _elementFinder.LoadHtmlByXpathAndAttribute(ref XpathConsts.Xpath.DouNavBlockUri, ref XpathConsts.ElementsToFind.HtmlDocAttributeToFind);

                var douNavBlockElementsList = _elementFinder.FindChildNodesByElement(douDocument, XpathConsts.ElementsToFind.ListElementToFind);

                foreach (var navNode in douNavBlockElementsList)
                {
                    var innerTextOfElementInNavBlock = _elementFinder.FindSingleNodeInnerTextByXpath(navNode, ref XpathConsts.ElementsToFind.ElementInnerTextToFind);

                    if (innerTextOfElementInNavBlock is "Офіси")
                    {
                        var currentOfficeUri = _elementFinder.FindSingleNodeByXpathAndElement(navNode, ref XpathConsts.ElementsToFind.ElementInnerTextToFind, XpathConsts.ElementsToFind.AttributeToFind);

                        if (currentOfficeUri is null)
                        {
                            driver.Quit();
                            continue;
                        }

                        await GoToUri(driver, currentOfficeUri);

                        break;
                    }
                }

                var currentMapUri = _elementFinder.FindElementsByXpathAndAttribute(driver, ref XpathConsts.Xpath.DefaultDouMapUri, ref XpathConsts.ElementsToFind.AttributeToFind) ??
                                    _elementFinder.FindElementsByXpathAndAttribute(driver, ref XpathConsts.Xpath.AdditionalDouMapUri, ref XpathConsts.ElementsToFind.AttributeToFind);

                if (currentMapUri is null)
                {
                    driver.Quit();
                    continue;
                }

                await GoToUri(driver, currentMapUri);

                var metaUri = _elementFinder.FindElementsByXpathAndAttribute(driver, ref XpathConsts.Xpath.MapMetaContent, ref XpathConsts.ElementsToFind.AttributeContentToFind);

                coordinates = SplitUri(ref metaUri);

                driver.Quit();
            }

            var vacancyName = _elementFinder.FindSingleNodeInnerTextByXpath(vacancy, ref XpathConsts.Xpath.DjinniVacancyName) ??
                              _elementFinder.FindSingleNodeInnerTextByXpath(vacancy, ref XpathConsts.Xpath.AdditionalDjinniVacancyName);

            var description = _elementFinder.FindSingleNodeInnerTextByXpath(vacancy, ref XpathConsts.Xpath.DjinniVacancyDescription);

            var response = new VacancyResponse(
                Guid.NewGuid(),
                vacancyName,
                description,
                coordinates,
                null);

            vacancyResponse.Add(response);
        }

        return vacancyResponse;
    }

    private async ValueTask<List<VacancyResponse>> FillVacancies()
    {
        var document = _elementFinder.LoadHtmlByXpathAndAttribute(ref XpathConsts.Xpath.HtmlDoc, ref XpathConsts.ElementsToFind.HtmlDocAttributeToFind);

        var vacancies = _elementFinder.FindChildNodesByElement(document, XpathConsts.ElementsToFind.ListElementToFind);

        return await FindVacancies(vacancies);
    }

    private async ValueTask<IWebDriver?> ActivateScrapping(string path, string additionalPath)
    {
        var driver = await _activateDriver.ActivateScrapingDriver();

        driver.Navigate().GoToUrl(path + additionalPath);

        await Task.Delay(500);

        return driver;
    }

    private async ValueTask GoToUri(IWebDriver? driver, string uri)
    {
        driver?.Navigate().GoToUrl(uri);

        await Task.Delay(500);
    }

    private string SplitUri(ref string uri)
    {
        var firstPart = uri.Substring(uri.IndexOf("center=") + 7);

        return firstPart.Substring(0, firstPart.IndexOf("&zoom"));
    }
}
