using Blog.Fe.Domain.Repositories;
using Blog.Fe.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;

namespace Blog.Fe.Presentation.Logging;

internal static class LoggerConfigurationExtension
{
    public static LoggerConfiguration SQLite(this LoggerSinkConfiguration sinkConfiguration, IConfiguration configuration)
    {
        var serviceProvider = new ServiceCollection()
            .AddRepositories(configuration)
            .BuildServiceProvider();

        var logItemRepository = serviceProvider.GetRequiredService<ILogItemRepository>();

        return sinkConfiguration.Sink(
            new SQLiteSink(logItemRepository),
            new BatchingOptions
            {
                BatchSizeLimit = 100
            });
    }

    public static LoggerConfiguration WithRequestHeader(this LoggerEnrichmentConfiguration enrichmentConfiguration, string headerName)
        => enrichmentConfiguration.With(new RequestHeaderEnricher(headerName));
}
