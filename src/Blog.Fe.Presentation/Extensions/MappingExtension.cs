using System.Collections.Generic;
using System.Text.Json;
using Blog.Fe.Domain.Entities;
using Blog.Fe.Domain.Enums;
using Serilog.Events;

namespace Blog.Fe.Presentation.Extensions;

internal static class MappingExtension
{
    public static IEnumerable<LogItem> Map(this IReadOnlyCollection<LogEvent> source)
    {
        foreach (var item in source)
            yield return item.Map();
    }

    private static LogItem Map(this LogEvent source)
        => new LogItem
        {
            CreatedAt = source.Timestamp,
            ChangedAt = source.Timestamp,
            Message = source.RenderMessage(),
            LogLevel = source.Level.Map(),
            Properties = source.Properties.Map()
        };

    private static LogLevel Map(this LogEventLevel source)
        => source switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => LogLevel.None
        };

    private static string Map(this IReadOnlyDictionary<string, LogEventPropertyValue> source)
    {
        var properties = new Dictionary<string, string>();
        foreach (var (propertyKey, propertyValue) in source)
        {
            properties[propertyKey] = propertyValue
                .ToString()
                .Trim('"');
        }

        return JsonSerializer.Serialize(properties);
    }
}
