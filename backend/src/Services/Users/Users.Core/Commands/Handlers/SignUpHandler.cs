using Mediator;
using Shared.Dal.Repositories;
using Shared.Dal.Utils;
using Shared.Security.Cryptography;
using Users.Core.Entities;

namespace Users.Core.Commands.Handlers;

public class SignUpHandler : ICommandHandler<SignUp>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository _baseRepository;
    private readonly IPasswordManager _passwordManager;

    public SignUpHandler(
        IUnitOfWork unitOfWork,
        IBaseRepository baseRepository,
        IPasswordManager passwordManager)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        _passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
    }

    public async ValueTask<Unit> Handle(SignUp command, CancellationToken cancellationToken)
    {
        bool? isUserNameUnique = await _baseRepository.IsFieldUniqueAsync<User>(x => x.UserName == command.UserName);

        bool? isEmailUnique = await _baseRepository.IsFieldUniqueAsync<User>(x => x.Password == command.Password);

        string securePassword = _passwordManager.Secure(command.Password);

        var validRole = command.RoleGid is not null ? await _baseRepository.GetByConditionAsync<Role>(x => x.Gid == command.RoleGid.GetValueOrDefault())
                                                    : await _baseRepository.GetByConditionAsync<Role>(x => x.Name == Role.DefaultRole);

        var user = User.Init(command.UserName,
                             command.Email,
                             securePassword,
                             validRole,
                             isUserNameUnique,
                             isEmailUnique);

        _baseRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
