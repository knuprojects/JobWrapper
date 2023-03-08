using Mediator;
using Shared.Api.Exceptions;
using Shared.Dal.Repositories;
using Shared.Security.Providers;
using Users.Core.Dto;
using Users.Core.Entities;
using Users.Core.Helpers;
using static Shared.Consts;

namespace Users.Core.Commands.Handlers;

public class RevokeTokenHandler : ICommandHandler<RevokeToken>
{
    private readonly IBaseRepository _baseRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenStorage _tokenStorage;

    public RevokeTokenHandler(
        IBaseRepository baseRepository,
        IJwtProvider jwtProvider,
        ITokenStorage tokenStorage)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
    }

    public async ValueTask<Unit> Handle(RevokeToken command, CancellationToken cancellationToken)
    {
        var existingUser = await _baseRepository.GetByConditionAsync<User>(x => x.Gid == command.UserGid);

        if (existingUser is null)
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty,
                $"User: {command.UserGid} is null or empty");

        var existingRole = await _baseRepository.GetByConditionAsync<Role>(x => x.Gid == existingUser.RoleGid);

        if (existingRole is null)
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty,
                $"Role: {existingUser.RoleGid} is null");

        var jwt = _jwtProvider.CreateToken(
            existingUser.Gid.ToString(),
            existingUser.Email,
            existingRole.Name,
            null
        );

        var jwtDto = new JwtDto(existingUser.Gid, jwt.AccessToken, jwt.Expires);

        _tokenStorage.Set(jwtDto);

        return Unit.Value;
    }
}