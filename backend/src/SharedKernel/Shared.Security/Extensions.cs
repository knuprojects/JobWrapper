using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Abstractions;
using Shared.Security.Cryptography;
using Shared.Security.Generators;
using Shared.Security.Providers;
using System.Text;

namespace Shared.Security;

public static class Extensions
{
    private static string SectionName = "jwt";

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.BindOptions<JwtOptions>(SectionName);

        services.AddSingleton(jwtOptions);

        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthorization()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

                jwt.SaveToken = true;

                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = issuerSigningKey,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });

        return services;
    }

    public static long ToTimestamp(this DateTime dateTime)
    {
        var centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var expectedDate = dateTime.Subtract(new TimeSpan(centuryBegin.Ticks));

        return expectedDate.Ticks / 10000;
    }
}
