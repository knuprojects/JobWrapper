using Microsoft.Extensions.DependencyInjection;
using Shared.Dal;
using Users.Persistence.Initializers;

namespace Users.Persistence;

public static class Extensions
{

    public static IServiceCollection AddPersistence(this IServiceCollection services)
       => services
                  .AddPostgresDatabase<UsersContext>()
                  .AddInitializer<UsersDataInitializer>();
}
