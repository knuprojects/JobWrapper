using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dal.Pagination;
using Shared.Dal.Repositories;
using Vacancies.Core.Common.Queries.Vacancies;
using Vacancies.Core.Entities;
using Vacancies.Presentation.Queries;

namespace Vacancies.Presentation.Controllers;

[Authorize]
[Route("api/vacancies")]
[ApiController]
//[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "latest" })]
public class VacanciesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBaseRepository _baseRepository;

    public VacanciesController(
        IMediator mediator,
        IBaseRepository baseRepository
        )
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    [HttpGet("filters")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> GetVacanciesByFilters([FromQuery] GetVacanciesByFilters request, CancellationToken token)
    {
        var filter = new PaginationFilter(request.PageNumber, request.PageSize);

        var query = new GetVacanciesByFiltersQuery(filter, request.Skills, request.Salary);

        var vacancies = await _mediator.Send(query, token);

        return vacancies is null ? NoContent() : Ok(vacancies);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> GetVacancies([FromQuery] PaginationQuery query)
    {
        var filter = new PaginationFilter(query.PageNumber, query.PageSize);

        var result = await _baseRepository.GetListAsync<Vacancy>(filter);

        return result is null ? NoContent() : Ok(result);
    }
}