namespace Vacancies.Core.Responses;

public record VacancyResponse(Guid Gid, string Name, List<string> Skills, string Location, string Salary);