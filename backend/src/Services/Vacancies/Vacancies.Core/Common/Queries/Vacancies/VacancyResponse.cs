namespace Vacancies.Core.Common.Queries.Vacancies;

public record VacancyResponse(Guid Gid, string Name, List<string> Skills, string Location, string Salary);