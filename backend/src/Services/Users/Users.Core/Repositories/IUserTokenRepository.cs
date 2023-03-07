namespace Users.Core.Repositories;

public interface IUserTokenRepository
{
    ValueTask<bool?> CheckUserTokenAsync(string token);
}