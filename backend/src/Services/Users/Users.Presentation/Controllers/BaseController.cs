using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Users.Presentation.Controllers;

[Route("api/")]
[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
        => _mediator = mediator;
}
