namespace Vacancies.Core.Consts;

public static class VacanciesConsts
{
    public static string DjinniUrl => "https://djinni.co";
}

public static class XpathConsts
{
    public static class Xpath
    {
        // USEFULL XPATH
        public static string PaginationButton = "//div[@class='d-md-none mb-3 text-center']";
        public static string HtmlDoc = "//div[@class='row']/div[1]/ul";

        // DJINNI
        public static string CurrentDjinniVacancy = "div[1]/div[2]/a";
        public static string DjinniVacancyName = "div[1]/div[2]/a/span";
        public static string AdditionalDjinniVacancyName = "div[1]/div[2]/a[2]/span";

        // DOU
        public static string CurrentDouUri = "//*[@class='container job-post-page']/div[2]/div[1]/div[2]/a[2]";
        public static string AdditionalCurrentDouUri = "//*[@class='container job-post-page']/div[3]/div[1]/div[2]/a[2]";
        public static string DouNavBlockUri = "//*[@class='company-nav']";
        public static string DefaultDouMapUri = "//*[@class='g-company-wrapper']/div[2]/div[2]/div[1]/div/div/div/div[2]/div[1]/div/div/div[1]/span/a";

        //DOU MAP
        public static string AdditionalDouMapUri = "//*[@class='g-company-wrapper']/div[2]/div[2]/div[1]/div/div/div[1]/div[2]/div/div/div[1]/span/a";
        public static string MapMetaContent = "/html/head/meta[10]";
        public static string AdditionalMapMetaContent = "/html/head/meta[11]";
    }

    public static class ElementsToFind
    {
        public static string HtmlDocAttributeToFind = "outerHTML";
        public static string ListElementToFind = "li";
        public static string AttributeToFind = "href";
        public static string ElementInnerTextToFind = "a";
        public static string AttributeContentToFind = "content";
    }
}