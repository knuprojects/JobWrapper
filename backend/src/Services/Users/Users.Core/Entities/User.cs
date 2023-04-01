using Shared.Abstractions.Primitives;
using Shared.Api.Exceptions;
using Users.Core.ValueObjects;
using static Shared.Consts;

namespace Users.Core.Entities;

public class User : Entity
{
    private User(
        UserName? userName,
        Email? email,
        string password,
        Guid roleGid) : base()
    {
        UserName = userName;
        Email = email;
        Password = password;
        RoleGid = roleGid;
    }
    public UserName? UserName { get; private set; }
    public Email? Email { get; private set; }
    public string Password { get; private set; } = null!;
    public Guid RoleGid { get; private set; }

    public Role? Role { get; set; }

    public static User Init(
        string userNameRequest,
        string emailRequest,
        string password,
        Role? validRole,
        bool? isUserNameUnique,
        bool? isEmailUnique)
    {
        if (!isUserNameUnique.GetValueOrDefault())
            throw new BaseException(ExceptionCodes.ValueAlreadyExist,
                $"UserName: {userNameRequest} is already exist");

        if (!isEmailUnique.GetValueOrDefault())
            throw new BaseException(ExceptionCodes.ValueAlreadyExist,
                $"Email: {emailRequest} is already exist");

        var userName = UserName.Init(userNameRequest);
        var email = Email.Init(emailRequest);

        var user = new User(userName,
                            email,
                            password,
                            validRole.Gid);

        return user;
    }

    public static User ChangeUserEmail(
        User user,
        string emailRequest,
        bool isEmailUnique)
    {
        if (!isEmailUnique)
            throw new BaseException(ExceptionCodes.ValueAlreadyExist,
                $"Email: {emailRequest} is already exist");

        var email = Email.Init(emailRequest);

        user.Email = email;

        return user;
    }

    public static User ChangeUserName(
        User user,
        string userNameRequest,
        bool isUserNameUnique)
    {
        if (!isUserNameUnique)
            throw new BaseException(ExceptionCodes.ValueAlreadyExist,
                $"UserName: {userNameRequest} is already exist");

        var userName = UserName.Init(userNameRequest);

        user.UserName = userName;

        return user;
    }

    public static User ChangeUserPassword(
        User user,
        string password)
    {
        user.Password = password;

        return user;
    }
}