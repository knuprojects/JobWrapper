namespace Vacancies.Core.Responses;

public record VacancyResponse(Guid Gid, string Name, string Description, string Location, string Salary);