using Mediator;

namespace Users.Core.Commands;

public record SignUp(string UserName, string Email, string Password, Guid? RoleGid) : ICommand;