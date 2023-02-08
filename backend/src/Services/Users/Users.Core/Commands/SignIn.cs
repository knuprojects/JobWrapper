using Mediator;
using Users.Core.Dto;

namespace Users.Core.Commands;

public record SignIn(string UserName, string Password) : ICommand<JwtDto>;
