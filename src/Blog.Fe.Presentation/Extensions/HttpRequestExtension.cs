using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Blog.Fe.Presentation.Extensions;

internal static class HttpRequestExtension
{
    public static bool TryGetHeaderValue(this HttpRequest request, string headerName, out string headerValue)
    {
        if (
            request.Headers.TryGetValue(headerName, out var headerStringValues) &&
            headerStringValues.TryGetValue(out var value)
        )
        {
            headerValue = value;
            return true;
        }
        
        headerValue = string.Empty;
        return false;
    }

    private static bool TryGetValue(this StringValues values, out string value)
    {
        var result = values.ToString();
        if (!string.IsNullOrEmpty(result))
        {
            value = result;
            return true;
        }

        value = string.Empty;
        return false;
    }
}
