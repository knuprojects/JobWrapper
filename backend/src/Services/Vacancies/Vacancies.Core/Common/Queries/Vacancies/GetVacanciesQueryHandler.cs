using Mediator;
using Shared.Dal.Pagination;
using Shared.Dal.Repositories;
using Vacancies.Core.Entities;

namespace Vacancies.Core.Common.Queries.Vacancies;

public record GetVacanciesByFiltersQuery(int PageNumber, int PageSize, List<string> Skills, List<string> Salary, DateTime StartDate, DateTime EndDate, PaginationFilter PaginationFilter) : IQuery<PaginationResponse<Vacancy>>;

public class GetVacanciesQueryHandler : IQueryHandler<GetVacanciesByFiltersQuery, PaginationResponse<Vacancy>>
{
    private readonly IBaseRepository _repository;

    public GetVacanciesQueryHandler(IBaseRepository repository)
        => _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async ValueTask<PaginationResponse<Vacancy>> Handle(GetVacanciesByFiltersQuery query, CancellationToken cancellationToken)
    {
        PaginationResponse<Vacancy> vacancies = new PaginationResponse<Vacancy>();

        if (!query.Skills.Any())
        {
            vacancies = await _repository.GetEntitiesByConditionAsync<Vacancy>(x => x.Skills == query.Skills, query.PaginationFilter);
        }
        else if (query.Salary.Any())
        {
            if (query.Salary.Count == 2)
            {
                vacancies = await _repository
                   .GetEntitiesByConditionAsync<Vacancy>(x =>
                   Convert.ToDouble(x.Salary) >= Convert.ToDouble(query.Salary[0]) &&
                   Convert.ToDouble(x.Salary) <= Convert.ToDouble(query.Salary[1]),
                   query.PaginationFilter);
            }
            else
                vacancies = await _repository
                   .GetEntitiesByConditionAsync<Vacancy>(x =>
                   Convert.ToDouble(x.Salary) >= Convert.ToDouble(query.Salary[0]),
                   query.PaginationFilter);
        }
        else
        {
            vacancies = await _repository.GetListAsync<Vacancy>(query.PaginationFilter);
        }

        return vacancies;
    }
}
