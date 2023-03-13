using HtmlAgilityPack;
using OpenQA.Selenium;
using Vacancies.Core.Consts;

namespace Vacancies.Core.Helpers;

public interface IElementFinder
{
    HtmlDocument LoadHtmlByXpathAndAttribute(IWebDriver driver, string xpath, string attribute);
    List<HtmlNode>? FindChildNodesByElement(HtmlDocument document, string element);
    string? FindSingleNodeByXpathAndElement(HtmlNode? node, string xpath, string element);
    string? FindSingleNodeInnerTextByXpath(HtmlNode? node, string xpath);
    IWebElement? FindElementsByXpath(IWebDriver driver, string xpath);
    string? FindElementsByXpathAndAttribute(IWebDriver driver, string xpath, string attribute);
    IWebElement? FindFirstElementByByDjinniAdditionalInfo(IWebDriver driver, string xpath);
    List<IWebElement>? FindListOfElementsByByDjinniAdditionalInfo(IWebElement element, string className);
    string? FindElementByClassWithAttribute(IWebElement element, string className, string attribute);
}

public class ElementFinder : IElementFinder
{
    public HtmlDocument LoadHtmlByXpathAndAttribute(IWebDriver driver, string xpath, string attribute)
    {
        var htmlBlock = new HtmlDocument();

        htmlBlock.LoadHtml(driver.FindElements(By.XPath(xpath))
                .FirstOrDefault()?.GetAttribute(attribute));

        return htmlBlock;
    }

    public List<HtmlNode>? FindChildNodesByElement(HtmlDocument document, string element)
        => document.DocumentNode.FirstChild.ChildNodes.Where(node =>
                node.EndNode.Name == element).ToList();

    public string? FindSingleNodeByXpathAndElement(HtmlNode? node, string xpath, string element)
        => node?.SelectSingleNode(xpath).Attributes.FirstOrDefault(node =>
                    node.Name == element)?.Value;

    public string? FindSingleNodeInnerTextByXpath(HtmlNode? node, string xpath)
        => node?.SelectSingleNode(xpath).InnerText;

    public IWebElement? FindElementsByXpath(IWebDriver driver, string xpath)
        => driver.FindElements(By.XPath(xpath))?.FirstOrDefault();

    public string? FindElementsByXpathAndAttribute(IWebDriver driver, string xpath, string attribute)
        => driver.FindElements(By.XPath(xpath))?
                    .FirstOrDefault()?.GetAttribute(attribute);

    public IWebElement? FindFirstElementByByDjinniAdditionalInfo(IWebDriver driver, string xpath)
        => driver.FindElements(By.XPath(xpath))?
                    .FirstOrDefault();

    public List<IWebElement>? FindListOfElementsByByDjinniAdditionalInfo(IWebElement element, string className)
        => element.FindElements(By.ClassName(className)).ToList();

    public string? FindElementByClassWithAttribute(IWebElement element, string className, string attribute)
        => element.FindElements(By.ClassName(className)).FirstOrDefault().GetAttribute(attribute);
}