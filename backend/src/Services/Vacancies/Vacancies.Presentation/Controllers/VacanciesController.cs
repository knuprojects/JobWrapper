using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dal.Pagination;
using Vacancies.Core.Common.Queries.Vacancies;

namespace Vacancies.Presentation.Controllers;

[Authorize]
[Route("api/vacancies")]
[ApiController]
//[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "latest" })]
public class VacanciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VacanciesController(IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> GetVacancies([FromQuery] GetVacanciesByFiltersQuery query, CancellationToken token)
    {
        var vacancies = await _mediator.Send(query, token);

        return vacancies is null ? NoContent() : Ok(vacancies);
    }
}