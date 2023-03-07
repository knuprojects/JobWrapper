using Mediator;

namespace Users.Core.Commands;

public record SignIn(string UserName, string Password) : ICommand;
