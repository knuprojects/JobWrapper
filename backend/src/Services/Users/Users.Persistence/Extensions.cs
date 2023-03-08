using Microsoft.Extensions.DependencyInjection;
using Shared.Dal;
using Users.Core.Repositories;
using Users.Persistence.Initializers;
using Users.Persistence.Repositories;

namespace Users.Persistence;

public static class Extensions
{

    public static IServiceCollection AddPersistence(this IServiceCollection services)
       => services
                  .AddScoped<IUserTokenRepository, UserTokenRepository>()
                  .AddPostgresDatabase<UsersContext>()
                  .AddInitializer<UsersDataInitializer>();
}
