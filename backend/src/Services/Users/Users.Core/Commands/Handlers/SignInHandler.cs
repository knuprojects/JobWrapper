using Azure.Core;
using Mediator;
using Shared.Api.Exceptions;
using Shared.Security.Cryptography;
using Shared.Security.Providers;
using Users.Core.Dto;
using Users.Core.Repositories;
using static Shared.Consts;

namespace Users.Core.Commands.Handlers;

public class SignInHandler : ICommandHandler<SignIn, JwtDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IJwtProvider _jwtProvider;

    public SignInHandler(
        IUserRepository userRepository,
        IPasswordManager passwordManager,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _jwtProvider = jwtProvider;
    }

    public async ValueTask<JwtDto> Handle(SignIn command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByUserNameAsync(command.UserName);

        if (existingUser is null)
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty,
                $"User with: {command.UserName} not found");

        bool isCorrectPassword = _passwordManager.Validate(command.Password, existingUser.Password);

        if (!isCorrectPassword)
            throw new BaseException(ExceptionCodes.ValueMissmatch,
                $"Password: {command.Password} is incorrect");

        var jwt = _jwtProvider.CreateToken(existingUser.Gid.ToString(), existingUser.Email.Value, existingUser.RoleGid.ToString(), null);

        return new JwtDto(existingUser.Gid, jwt.AccessToken);
    }
}
