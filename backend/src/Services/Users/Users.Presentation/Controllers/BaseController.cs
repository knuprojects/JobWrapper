using Mediator;
using Microsoft.AspNetCore.Mvc;
using Users.Core.Helpers;

namespace Users.Presentation.Controllers;

[Route("api/")]
[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly ITokenStorage _tokenStorage;

    protected BaseController(
        IMediator mediator,
        ITokenStorage tokenStorage)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
    }
}
