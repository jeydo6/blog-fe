using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Blog.Fe.Presentation.Policies;

internal sealed class LocalOriginHandler : AuthorizationHandler<LocalOriginRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LocalOriginRequirement requirement)
    {
        if (
            context.Resource is HttpContext httpContext &&
            httpContext.Connection.RemoteIpAddress is not null &&
            IPAddress.IsLoopback(httpContext.Connection.RemoteIpAddress)
        )
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}