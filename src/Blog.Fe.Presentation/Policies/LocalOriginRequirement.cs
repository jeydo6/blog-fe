using Microsoft.AspNetCore.Authorization;

namespace Blog.Fe.Presentation.Policies;

internal sealed record LocalOriginRequirement : IAuthorizationRequirement
{
    public const string Name = "LocalOrigin";
}