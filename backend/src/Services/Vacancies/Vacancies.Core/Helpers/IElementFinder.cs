using HtmlAgilityPack;
using OpenQA.Selenium;

namespace Vacancies.Core.Helpers;

public interface IElementFinder
{
    HtmlDocument LoadHtmlByXpathAndAttribute(IWebDriver driver, ref string xpath, ref string attribute);
    List<HtmlNode>? FindChildNodesByElement(HtmlDocument document, string element);
    string? FindSingleNodeByXpathAndElement(HtmlNode? node, ref string xpath, string element);
    string? FindSingleNodeInnerTextByXpath(HtmlNode? node, ref string xpath);
    IWebElement? FindElementsByXpath(IWebDriver driver, ref string xpath);
    string? FindElementsByXpathAndAttribute(IWebDriver driver, ref string xpath, ref string attribute);
}

public class ElementFinder : IElementFinder
{
    public HtmlDocument LoadHtmlByXpathAndAttribute(IWebDriver driver, ref string xpath, ref string attribute)
    {
        var htmlBlock = new HtmlDocument();

        htmlBlock.LoadHtml(driver.FindElements(By.XPath(xpath))
                .FirstOrDefault()?.GetAttribute(attribute));

        return htmlBlock;
    }

    public List<HtmlNode>? FindChildNodesByElement(HtmlDocument document, string element)
        => document.DocumentNode.FirstChild.ChildNodes.Where(node =>
                node.EndNode.Name == element).ToList();

    public string? FindSingleNodeByXpathAndElement(HtmlNode? node, ref string xpath, string element)
        => node?.SelectSingleNode(xpath).Attributes.FirstOrDefault(node =>
                    node.Name == element)?.Value;

    public string? FindSingleNodeInnerTextByXpath(HtmlNode? node, ref string xpath)
        => node?.SelectSingleNode(xpath).InnerText;

    public IWebElement? FindElementsByXpath(IWebDriver driver, ref string xpath)
        => driver.FindElements(By.XPath(xpath))?.FirstOrDefault();

    public string? FindElementsByXpathAndAttribute(IWebDriver driver, ref string xpath, ref string attribute)
        => driver.FindElements(By.XPath(xpath))?
                    .FirstOrDefault()?.GetAttribute(attribute);
}
