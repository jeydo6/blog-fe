using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blog.Fe.Presentation.Logging;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddHttpContextAccessor()
            .AddSerilog(cfg => cfg
                .ReadFrom.Configuration(configuration)
                // .WriteTo.Console()
                .WriteTo.SQLite(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithRequestHeader(Constants.RealIpHeaderName)
                .Enrich.WithRequestHeader(Constants.ForwardedForHeaderName)
            );
}
