using Mediator;
using Microsoft.AspNetCore.Mvc;
using Users.Core.Commands;

namespace Users.Presentation.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("sign-up")]
        public async ValueTask<IActionResult> SignUp([FromBody] SignUp command)
            => Ok(await _mediator.Send(command));

        [HttpPost("sign-in")]
        public async ValueTask<IActionResult> SignIn([FromBody] SignIn command)
            => Ok(await _mediator.Send(command));
    }
}
