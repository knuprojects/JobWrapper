using Shared.Abstractions.Primitives;
using Shared.Api.Exceptions;
using static Shared.Consts;

namespace Users.Core.Entities;

public sealed class Role : Entity
{
    private readonly List<User> _users = new();

    private Role(
        string name) : base()
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;

    public ICollection<User>? Users => _users;

    public static string DefaultRole => "user";

    public static string Admin => "admin";

    public static Role Init(
        string name,
        bool isNameUnique)
    {
        if (!isNameUnique)
            throw new BaseException(ExceptionCodes.ValueAlreadyExist,
                $"Role: {name} is already exist");

        var role = new Role(name);

        return role;
    }

    public static Role ChangeRoleName(
        Role role,
        string name,
        bool isNameUnique)
    {
        if (!isNameUnique)
            throw new BaseException(ExceptionCodes.ValueAlreadyExist,
                $"Role: {name} is already exist");

        role.Name = name;

        return role;
    }
}