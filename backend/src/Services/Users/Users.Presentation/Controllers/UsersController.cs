using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Core.Commands;
using Users.Core.Helpers;

namespace Users.Presentation.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(
            IMediator mediator,
            ITokenStorage tokenStorage)
            : base(mediator, tokenStorage)
        {
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async ValueTask<IActionResult> SignUp([FromBody] SignUp command)
            => Ok(await _mediator.Send(command));

        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async ValueTask<IActionResult> SignIn([FromBody] SignIn command)
        {
            await _mediator.Send(command);

            var jwt = _tokenStorage.Get();

            return Ok(jwt);
        }

        [HttpPost("revoke-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async ValueTask<IActionResult> RevokeToken([FromBody] RevokeToken command)
        {
            await _mediator.Send(command);

            var jwt = _tokenStorage.Get();

            return Ok(jwt);
        }

        [AllowAnonymous]
        [HttpPost("email-confirmation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async ValueTask<IActionResult> EmailConfirmation([FromBody] EmailCofirmation command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));
    }
}
