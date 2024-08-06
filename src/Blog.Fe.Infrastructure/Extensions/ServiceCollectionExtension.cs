using System;
using Blog.Fe.Domain.Repositories;
using Blog.Fe.Infrastructure.DbContexts;
using Blog.Fe.Infrastructure.Migrations;
using Blog.Fe.Infrastructure.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Fe.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
	private const string ConnectionStringName = "DefaultConnection";

	public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
		=> services
			.AddRepositories(configuration)
			.AddMigrations(configuration);

	public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString(ConnectionStringName);
		ArgumentException.ThrowIfNullOrEmpty(connectionString);
		
		return services
			.AddFluentMigratorCore()
			.ConfigureRunner(builder => builder
				.AddSQLite()
				.WithGlobalConnectionString(connectionString)
				.ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
			.AddLogging(builder => builder.AddFluentMigratorConsole());
	}

	public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString(ConnectionStringName);
		ArgumentException.ThrowIfNullOrEmpty(connectionString);
		
		return services
			.AddScoped(_ => new DbContext(connectionString))
			.AddScoped<IPostRepository, PostRepository>()
			.AddScoped<ILogItemRepository, LogItemRepository>();
	}
}
