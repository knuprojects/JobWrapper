namespace Vacancies.Presentation.Requests;

public record DefaultRequest(int PageNumber, int PageSize);

public record GetBySkillsRequest(int PageNumber, int PageSize, List<string> Skills);

public record GetBySalaryRequest(int PageNumber, int PageSize, string Salary);