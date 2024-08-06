using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Fe.Presentation.Authentication;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddBasicAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication()
            .AddBasicAuthentication(o =>
            {
                var settings = configuration
                    .GetSection(nameof(BasicAuthenticationSettings))
                    .Get<BasicAuthenticationSettings>()!;

                ArgumentException.ThrowIfNullOrEmpty(settings.UserName);
                ArgumentException.ThrowIfNullOrEmpty(settings.Password);
                o.UserName = settings.UserName;
                o.Password = settings.Password;
            });

        return services;
    }
}
