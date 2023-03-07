using Shared.Messaging;

namespace JobWrapper.Messages.Users;

public record SignedUp(Guid UserGid, string Email, string ConfirmationToken) : IMessage;