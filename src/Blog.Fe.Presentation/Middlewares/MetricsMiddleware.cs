using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Blog.Fe.Presentation.Middlewares;

internal sealed class MetricsMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<MetricsMiddleware> _logger;

	public MetricsMiddleware(RequestDelegate next, ILogger<MetricsMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}
	
	public async Task InvokeAsync(HttpContext context)
	{
		_logger.LogInformation("'{Address}': '{Endpoint}'", context.Connection.RemoteIpAddress, context.Request.Path);
		_logger.LogInformation("{Guid}", Guid.NewGuid().ToString("N"));
		await _next(context);
	}
}
