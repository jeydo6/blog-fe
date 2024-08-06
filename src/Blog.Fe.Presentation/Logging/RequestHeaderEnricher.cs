using Blog.Fe.Presentation.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Blog.Fe.Presentation.Logging;

internal sealed class RequestHeaderEnricher : ILogEventEnricher
{    
    private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

    private readonly string _headerName;
    private readonly string _propertyName;
    private readonly string _itemKey;

    public RequestHeaderEnricher(string headerName)
    {
        _headerName = headerName;
        _propertyName = headerName.Replace("-", string.Empty);
        _itemKey = "Serilog_" + headerName;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;

        if (httpContext.Items[_itemKey] is LogEventProperty logEventProperty)
        {
            logEvent.AddPropertyIfAbsent(logEventProperty);
            return;
        }

        if (!httpContext.Request.TryGetHeaderValue(_headerName, out var headerValue))
            return;

        logEventProperty = new LogEventProperty(_propertyName, new ScalarValue(headerValue));
        httpContext.Items[_itemKey] = logEventProperty;

        logEvent.AddPropertyIfAbsent(logEventProperty);
    }
}
