using System.Security.Cryptography;

namespace Shared.Security.Cryptography;

public interface IPasswordManager
{
    string Secure(string password);
    bool Validate(string password, string passwordHash);
}

public class PasswordManager : IPasswordManager
{
    public const int _saltSize = 16;
    public const int _keySize = 32;
    public const int _iterations = 100000;
    public static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
    public const char segmentDelimiter = ':';

    public string Secure(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            _iterations,
            _algorithm,
            _keySize
        );

        return string.Join(
            segmentDelimiter,
            Convert.ToHexString(key),
            Convert.ToHexString(salt),
            _iterations,
            _algorithm
        );
    }

    public bool Validate(string password, string passwordHash)
    {
        var segments = passwordHash.Split(segmentDelimiter);
        var key = Convert.FromHexString(segments[0]);
        var salt = Convert.FromHexString(segments[1]);
        var iterations = int.Parse(segments[2]);
        var algorithm = new HashAlgorithmName(segments[3]);

        var inputSecretKey = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            algorithm,
            key.Length
        );

        return key.SequenceEqual(inputSecretKey);
    }
}
