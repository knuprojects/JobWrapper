using Shared.Abstractions.Primitives;
using Shared.Api.Exceptions;
using static Shared.Consts;

namespace Users.Core.ValueObjects;

public sealed class UserName : ValueObject
{
    private const int MinLength = 0;
    private const int MaxLength = 20;

    public string Value { get; }

    private UserName(string value)
    {
        Value = value;
    }

    public static UserName Init(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BaseException(ExceptionCodes.ValueIsNullOrEmpty,
                $"UserName: {value} is null or empty");

        if (value.Length is MinLength or < MinLength)
            throw new BaseException(ExceptionCodes.ValueIsIncorrectRange,
                $"UserName: {value} is {MinLength} or < {MinLength}");

        if (value.Length > MaxLength)
            throw new BaseException(ExceptionCodes.ValueIsIncorrectRange,
                $"UserName: {value} is > {MaxLength}");

        return new UserName(value);
    }

    public static implicit operator UserName(string value) => value is null ? null : new UserName(value);
    public static implicit operator string(UserName value) => value.Value;

    public override string ToString() => $"UserName: {Value}";

    public override IEnumerable<object> GetValues()
    {
        yield return Value;
    }
}