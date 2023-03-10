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
        public static string DjinniCurrentVacancy = "div[1]/div[2]/a";
        public static string DjinniVacancyName = "div[1]/div[2]/a/span";
        public static string DjinniAdditionalVacancyName = "div[1]/div[2]/a[2]/span";
        public static string DjinniJobAdditionalInfo = "div[1]/div[1]/div[3]/div[2]/div/ul[1]/li[1]";

        // DOU
        public static string DouCurrentUri = "//*[@class='container job-post-page']/div[2]/div[1]/div[2]/a[2]";
        public static string DouAdditionalCurrentUri = "//*[@class='container job-post-page']/div[3]/div[1]/div[2]/a[2]";
        public static string DouNavBlockUri = "//*[@class='company-nav']";

        //DOU MAP
        public static string DouMapAdditionalUri = "//*[@class='g-company-wrapper']/div[2]/div[2]/div[1]/div/div/div[1]/div[2]/div/div/div[1]/span/a";
        public static string DouMapDefaultUri = "//*[@class='g-company-wrapper']/div[2]/div[2]/div[1]/div/div/div/div[2]/div[1]/div/div/div[1]/span/a";
        public static string DouMapMetaContent = "/html/head/meta[10]";
        public static string DouMapAdditionalMetaContent = "/html/head/meta[11]";
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