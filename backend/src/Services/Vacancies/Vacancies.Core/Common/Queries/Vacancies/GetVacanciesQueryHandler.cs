using Mediator;
using Shared.Dal.Pagination;
using Shared.Dal.Repositories;
using Vacancies.Core.Entities;

namespace Vacancies.Core.Common.Queries.Vacancies;

public record GetVacanciesByFiltersQuery(PaginationFilter PaginationFilter, List<string> Skills, string Salary) : IQuery<PaginationResponse<Vacancy>>;

public class GetVacanciesQueryHandler : IQueryHandler<GetVacanciesByFiltersQuery, PaginationResponse<Vacancy>>
{
    private readonly IBaseRepository _repository;

    public GetVacanciesQueryHandler(IBaseRepository repository)
        => _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async ValueTask<PaginationResponse<Vacancy>> Handle(GetVacanciesByFiltersQuery query, CancellationToken cancellationToken)
    {
        return query switch
        {
            _ when string.IsNullOrWhiteSpace(query.Salary) && query.Skills.Any() => await GetBySkills(query),
            _ when !string.IsNullOrWhiteSpace(query.Salary) && !query.Skills.Any() => await GetBySalary(query),
            _ when !string.IsNullOrWhiteSpace(query.Salary) && query.Skills.Any() => await GetBySalaryAndSkills(query),
            _ => await _repository.GetListAsync<Vacancy>(query.PaginationFilter)
        };
    }

    private async ValueTask<PaginationResponse<Vacancy>> GetBySkills(GetVacanciesByFiltersQuery query)
        => await _repository
                 .GetEntitiesByConditionAsync<Vacancy>(x => x.Skills == query.Skills, query.PaginationFilter);

    private async ValueTask<PaginationResponse<Vacancy>> GetBySalary(GetVacanciesByFiltersQuery query)
    {
        var splittedQuery = query.Salary.Split('-');

        return await _repository
                 .GetEntitiesByConditionAsync<Vacancy>(x =>
                 Convert.ToDouble(x.Salary) >= Convert.ToDouble(splittedQuery[0]) &&
                 Convert.ToDouble(x.Salary) <= Convert.ToDouble(splittedQuery[1]), query.PaginationFilter);
    }

    private async ValueTask<PaginationResponse<Vacancy>> GetBySalaryAndSkills(GetVacanciesByFiltersQuery query)
    {
        var splittedQuery = query.Salary.Split('-');

        return await _repository
                 .GetEntitiesByConditionAsync<Vacancy>(x =>
                 Convert.ToDouble(GetFirstWord(x.Salary)) >= Convert.ToDouble(splittedQuery[0]) &&
                 Convert.ToDouble(GetSecondWord(x.Salary)) <= Convert.ToDouble(splittedQuery[1]) &&
                 x.Skills == query.Skills,
                 query.PaginationFilter);
    }

    private string GetFirstWord(string? salary)
    {
        var splittedQuery = salary?.Split('-');

        return splittedQuery[0];
    }

    private string GetSecondWord(string? salary)
    {
        var splittedQuery = salary?.Split('-');

        return splittedQuery[1];
    }
}
