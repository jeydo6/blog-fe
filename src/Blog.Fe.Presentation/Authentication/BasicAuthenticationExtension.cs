using System;
using Microsoft.AspNetCore.Authentication;

namespace Blog.Fe.Presentation.Authentication;

internal static class BasicAuthenticationExtension
{
    public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions>? configureOptions)
        => builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(
            BasicAuthenticationDefaults.AuthenticationScheme,
            BasicAuthenticationDefaults.DisplayName,
            configureOptions
        );
}