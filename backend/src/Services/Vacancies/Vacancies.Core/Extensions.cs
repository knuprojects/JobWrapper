using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Text.RegularExpressions;
using Vacancies.Core.Common.BackgroundServices;
using Vacancies.Core.Common.Helpers;
using Vacancies.Core.Services;

namespace Vacancies.Core;

public static class Extensions
{
    private const string WordOfDay = "сьогодні";
    private const string WordOfYesterdayDay = "вчора";

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IElementFinder, ElementFinder>();
        services.AddScoped<IActivateDriver, ActivateDriver>();
        services.AddScoped<IScrapperService, ScrapperService>();

        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddHostedService<ScrapperBackgroundService>();

        return services;
    }

    public static DateTime? ParseDate(string text)
    {
        if (text is null) return null;

        if (text.Contains(WordOfDay)) return DateTime.UtcNow;

        if (text.Contains(WordOfYesterdayDay)) return DateTime.Today.AddDays(-1);

        string pattern = @"\d+\s\p{L}+";
        Match match = Regex.Match(text, pattern);

        if (match.Success)
        {
            string result = match.Value.Trim();

            var monthDayText = result.Substring(0, 3);

            var monthNameText = result.Substring(3);

            var monthNumber = monthNameText?.GetMonthNumberFromMonthName();

            var monthDay = Convert.ToInt32(monthDayText);

            return new DateTime(DateTime.Now.Year, monthNumber.GetValueOrDefault(), monthDay);
        }

        return null;
    }

    internal static int GetMonthNumberFromMonthName(this string monthName)
    {
        var monthNumber = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
        return monthNumber;
    }
}
