using Microsoft.AspNetCore.Authorization;

namespace Blog.Fe.Presentation.Authorization;

internal sealed record LocalOriginRequirement : IAuthorizationRequirement
{
    public const string Name = "LocalOrigin";
}
