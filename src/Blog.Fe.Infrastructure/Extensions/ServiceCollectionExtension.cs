using System;
using Blog.Fe.Domain.Repositories;
using Blog.Fe.Infrastructure.DataContexts;
using Blog.Fe.Infrastructure.Migrations;
using Blog.Fe.Infrastructure.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Fe.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
	private const string ConnectionStringName = "DbConnectionString";

	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString(ConnectionStringName);
		ArgumentException.ThrowIfNullOrEmpty(connectionString);
		
		services
			.AddFluentMigratorCore()
			.ConfigureRunner(builder => builder
				.AddSQLite()
				.WithGlobalConnectionString(connectionString)
				.ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
			.AddLogging(builder => builder.AddFluentMigratorConsole());
		
		services
			.AddScoped(_ => new BlogFeDataContext(connectionString));

		services.AddScoped<IPostRepository, PostRepository>();
	}
}
