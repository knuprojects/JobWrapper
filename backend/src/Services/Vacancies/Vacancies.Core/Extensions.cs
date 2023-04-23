using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Vacancies.Core.Common.BackgroundServices;
using Vacancies.Core.Common.Helpers;
using Vacancies.Core.Services;

namespace Vacancies.Core;

public static class Extensions
{
    private const string WordOfDay = "сьогодні";
    private const string WordOfYesterdayDay = "сьогодні";

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
        string monthNameText = null;

        if (text is null) return null;

        if (text.Contains(WordOfDay)) return DateTime.UtcNow;

        if (text.Contains(WordOfYesterdayDay)) return DateTime.Today.AddDays(-1);

        var monthDayText = text.Substring(0, 3);

        var monthNumber = monthNameText?.GetMonthNumberFromMonthName();

        var monthDay = Convert.ToInt32(monthDayText);

        return new DateTime(DateTime.Now.Year, monthNumber.GetValueOrDefault(), monthDay);
    }

    internal static int GetMonthNumberFromMonthName(this string monthName)
    {
        var monthNumber = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
        return monthNumber;
    }
}
