using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacancies.Core.Enums;

public static class Djinni
{
    const string _baseDjinniUrl = "https://djinni.co/";
    public enum Keywords
    {
        primary_keyword,
        exp_level,
        region
    }
}

public static class Dou
{
    const string _baseDouUrl = "https://jobs.dou.ua/";
    public enum Keywords
    {
        category,
        exp,
        city,
        remote,
        relocation
    }
}
