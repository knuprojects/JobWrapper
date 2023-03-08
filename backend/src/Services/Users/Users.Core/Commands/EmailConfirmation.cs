using Mediator;

namespace Users.Core.Commands;

public record EmailCofirmation(string Token) : ICommand;