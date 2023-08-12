using System;
using Microsoft.Extensions.Configuration;

namespace Blog.Fe.Presentation.Extensions;

internal static class ConfigurationExtension
{
	public static T Get<T>(this IConfiguration configuration)
	{
		var value = ConfigurationBinder.Get<T>(
			configuration.GetSection(typeof(T).Name)
		);

		ArgumentNullException.ThrowIfNull(value);
		return value;
	}
}