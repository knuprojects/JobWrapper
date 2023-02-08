using Mediator;
using Shared.Dal.Utils;
using Shared.Security.Cryptography;
using Shared.Security.Providers;
using Users.Core.Dto;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.Core.Commands.Handlers;

public class SignUpHandler : ICommandHandler<SignUp, JwtDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IJwtProvider _jwtProvider;

    public SignUpHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordManager passwordManager,
         IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordManager = passwordManager;
        _jwtProvider = jwtProvider;
    }

    public async ValueTask<JwtDto> Handle(SignUp command, CancellationToken cancellationToken)
    {
        bool? isUserNameUnique = await _userRepository.IsUserNameUniqueAsync(command.UserName);

        bool? isEmailUnique = await _userRepository.IsEmailUniqueAsync(command.Password);

        string securePassword = _passwordManager.Secure(command.Password);

        var validRole = command.RoleGid is not null ? await _roleRepository.GetRoleAsync(command.RoleGid.GetValueOrDefault())
                                                    : await _roleRepository.GetRoleByNameAsync(Role.DefaultRole);

        var user = User.Init(command.UserName,
                             command.Email,
                             securePassword,
                             validRole,
                             isUserNameUnique,
                             isEmailUnique,
                             command.IsNotificationsNeeded);

        _userRepository.AddUser(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var jwt = _jwtProvider.CreateToken(
            user.Gid.ToString(),
            user.Email,
            validRole.Name,
            null);

        return new JwtDto(user.Gid, jwt.AccessToken);
    }
}
