using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Blog.Fe.Presentation.Policies;

internal sealed class LocalOriginHandler : AuthorizationHandler<LocalOriginRequirement>
{
    private const string RealIpHeaderName = "X-Real-IP";
    private readonly ILogger<LocalOriginHandler> _logger;
    public LocalOriginHandler(ILogger<LocalOriginHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LocalOriginRequirement requirement)
    {
        if (
            context.User.Identity is not null &&
            context.User.Identity.IsAuthenticated &&
            context.Resource is HttpContext httpContext
        )
        {
            var remoteIpAddress =
                httpContext.Request.Headers.TryGetValue(RealIpHeaderName, out var realIpHeaderValue) &&
                IPAddress.TryParse(realIpHeaderValue, out var realIpAddress) ?
                realIpAddress : httpContext.Connection.RemoteIpAddress;

            if (IsLocalNetwork(httpContext.Connection.LocalIpAddress, remoteIpAddress))
            {
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation(
                    "An unsuccessful attempt to process the {RequirementName} occurred: User: '{UserName}', IpAddress: '{RemoteIpAddress}'",
                    nameof(LocalOriginRequirement), context.User.Identity?.Name, remoteIpAddress);
            }
        }

        return Task.CompletedTask;
    }

    private static bool IsLocalNetwork(IPAddress? local, IPAddress? remote, int count = 3)
    {
        if (local is null && remote is null)
        {
            return true;
        }
        else if (local is null || remote is null)
        {
            return false;
        }

        var localBytes = local.MapToIPv4().GetAddressBytes();
        var remoteBytes = remote.MapToIPv4().GetAddressBytes();
        for (var i = 0; i < count; i++)
        {
            if (localBytes[i] != remoteBytes[i]) return false;
        }

        return true;
    }
}