using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dal.Pagination;
using Shared.Dal.Repositories;
using Vacancies.Core.Entities;
using Vacancies.Presentation.Requests;

namespace Vacancies.Presentation.Controllers;

[Authorize]
[Route("api/vacancies")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "latest" })]
public class VacanciesController : ControllerBase
{
    private readonly IBaseRepository _repository;

    public VacanciesController(IBaseRepository repository)
        => _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> GetVacancies([FromQuery] DefaultRequest request)
    {
        var paginationFilter = new PaginationFilter(request.PageNumber, request.PageSize);

        var vacancies = await _repository.GetListAsync<Vacancy>(paginationFilter);

        return vacancies is null ? NoContent() : Ok(vacancies);
    }

    [HttpGet("skills")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> GetVacanciesBySkills([FromQuery] GetBySkillsRequest request)
    {
        var paginationFilter = new PaginationFilter(request.PageNumber, request.PageSize);

        var vacancies = await _repository.GetEntitiesByConditionAsync<Vacancy>(x => x.Skills == request.Skills, paginationFilter);

        return vacancies is null ? NoContent() : Ok(vacancies);
    }

    [HttpGet("salary")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> GetVacanciesBySalary([FromQuery] GetBySalaryRequest request)
    {
        var paginationFilter = new PaginationFilter(request.PageNumber, request.PageSize);

        var vacancies = await _repository.GetEntitiesByConditionAsync<Vacancy>(x => x.Salary == request.Salary, paginationFilter);

        return vacancies is null ? NoContent() : Ok(vacancies);
    }
}