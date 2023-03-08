using Mediator;

namespace Users.Core.Commands;

public record RevokeToken(Guid UserGid) : ICommand;
