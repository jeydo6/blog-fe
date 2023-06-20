using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blog.Fe.Infrastructure.Extensions;

public static class HostExtension
{
	public static void MigrateUp(this IHost host)
	{
		using var scope = host.Services.CreateScope();
		scope.ServiceProvider
			.GetRequiredService<IMigrationRunner>()
			.MigrateUp();
	}
}
