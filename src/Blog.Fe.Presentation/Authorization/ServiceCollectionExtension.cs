using Blog.Fe.Presentation.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Fe.Presentation.Authorization;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddLocalOriginPolicy(this IServiceCollection services)
    {
        services
            .AddSingleton<IAuthorizationHandler, LocalOriginHandler>()
            .AddAuthorizationBuilder()
            .AddPolicy(LocalOriginRequirement.Name, p => p
                .AddRequirements(new LocalOriginRequirement())
                .AddAuthenticationSchemes(BasicAuthenticationDefaults.AuthenticationScheme));

        return services;
    }
}
