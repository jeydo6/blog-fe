using Microsoft.AspNetCore.Authentication;

namespace Blog.Fe.Presentation.Authentication;

internal sealed class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}