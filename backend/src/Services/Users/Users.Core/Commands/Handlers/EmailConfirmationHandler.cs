using Mediator;
using Shared.Api.Exceptions;
using Shared.Dal.Repositories;
using Shared.Dal.Utils;
using Shared.Messaging;
using Shared.Security.Providers;
using Users.Core.Dto;
using Users.Core.Entities;
using Users.Core.Helpers;
using Users.Core.Repositories;
using static Shared.Consts;

namespace Users.Core.Commands.Handlers;

public class EmailConfirmationHandler : ICommandHandler<EmailCofirmation>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository _baseRepository;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly ITokenStorage _tokenStorage;
    private readonly IJwtProvider _jwtTokenProvider;

    public EmailConfirmationHandler(
        IUnitOfWork unitOfWork,
        IBaseRepository baseRepository,
        IUserTokenRepository userTokenRepository,
        ITokenStorage tokenStorage,
        IJwtProvider jwtTokenProvider)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        _userTokenRepository = userTokenRepository ?? throw new ArgumentNullException(nameof(userTokenRepository));
        _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        _jwtTokenProvider = jwtTokenProvider ?? throw new ArgumentNullException(nameof(jwtTokenProvider));
    }

    public async ValueTask<Unit> Handle(EmailCofirmation command, CancellationToken cancellationToken)
    {
        var existingToken = await _baseRepository.GetByConditionAsync<UserToken>(x => x.ConfirmationToken == command.Token);

        if (existingToken is null)
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty,
                $"Token: {command.Token} is not found!");

        if (existingToken.IsProcessed.GetValueOrDefault())
            throw new BaseException(ExceptionCodes.ValueAlreadyExist,
                $"Token: {command.Token} already processed!");

        var isTokenExpired = await _userTokenRepository.CheckUserTokenAsync(command.Token);

        if (isTokenExpired.GetValueOrDefault())
            return Unit.Value;

        var response = await ProcessTokenCreationAsync(existingToken, cancellationToken);

        return response;
    }

    private async ValueTask<Unit> ProcessTokenCreationAsync(UserToken existingToken, CancellationToken cancellationToken)
    {
        var token = UserToken.ProcessToken(existingToken);

        _baseRepository.Update(token);

        await _unitOfWork.SaveChangesAsync(cancellationToken, new EmptyMessage());

        var existingUser = await _baseRepository.GetByConditionAsync<User>(x => x.Gid == token.UserGid);

        var existingUserRole = await _baseRepository.GetByConditionAsync<Role>(x => x.Gid == existingUser.RoleGid);

        var jwt = _jwtTokenProvider.CreateToken(
            existingUser.Gid.ToString(),
            existingUser.Email,
            existingUserRole.Name,
            null);

        var jwtDto = new JwtDto(existingUser.Gid, jwt.AccessToken, jwt.Expires);

        _tokenStorage.Set(jwtDto);

        return Unit.Value;
    }
}
