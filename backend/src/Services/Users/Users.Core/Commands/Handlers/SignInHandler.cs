using Mediator;
using Shared.Api.Exceptions;
using Shared.Dal.Repositories;
using Shared.Security.Cryptography;
using Shared.Security.Providers;
using Users.Core.Dto;
using Users.Core.Entities;
using Users.Core.Helpers;
using static Shared.Consts;

namespace Users.Core.Commands.Handlers;

public class SignInHandler : ICommandHandler<SignIn>
{
    private readonly IBaseRepository _baseRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenStorage _tokenStorage;

    public SignInHandler(
        IBaseRepository baseRepository,
        IPasswordManager passwordManager,
        IJwtProvider jwtProvider,
        ITokenStorage tokenStorage)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        _passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
    }

    public async ValueTask<Unit> Handle(SignIn command, CancellationToken cancellationToken)
    {
        var existingUser = await _baseRepository.GetByConditionAsync<User>(x => x.UserName == command.UserName);

        if (existingUser is null)
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty,
                $"User with: {command.UserName} not found");

        var existingRole = await _baseRepository.GetByConditionAsync<Role>(x => x.Gid == existingUser.RoleGid);

        if (existingRole is null)
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty,
                $"Role with: {existingUser.RoleGid} is null!");

        bool isCorrectPassword = _passwordManager.Validate(command.Password, existingUser.Password);

        if (!isCorrectPassword)
            throw new BaseException(ExceptionCodes.ValueMissmatch,
                $"Password: {command.Password} is incorrect");

        var jwt = _jwtProvider.CreateToken(
            existingUser.Gid.ToString(),
            existingUser.Email.Value,
            existingRole.Name,
            null);

        var jwtDto = new JwtDto(existingUser.Gid, jwt.AccessToken, jwt.Expires);

        _tokenStorage.Set(jwtDto);

        return Unit.Value;
    }
}
