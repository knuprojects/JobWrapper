namespace Vacancies.Presentation.Queries;

public record GetVacanciesByFilters(int PageNumber, int PageSize, List<string> Skills, string? Salary);

public record PaginationQuery(int PageNumber, int PageSize);