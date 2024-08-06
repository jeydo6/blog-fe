using System;
using Blog.Fe.Domain.Entities;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Blog.Fe.Infrastructure.DbContexts;

internal sealed class DbContext : DataConnection
{
	private static readonly MappingSchema Schema = CreateMappingSchema();
	
	public DbContext(string connectionString) : base(CreateDataOptions(connectionString))
	{
		InlineParameters = true;
	}

	public ITable<Post> Posts => this.GetTable<Post>();
	public ITable<LogItem> LogItems => this.GetTable<LogItem>();
	
	private static MappingSchema CreateMappingSchema()
	{
		var mappingSchema = new MappingSchema();
		var builder = new FluentMappingBuilder(mappingSchema);

		builder.Entity<Post>()
			.HasTableName(nameof(Post))
			.HasIdentity(e => e.Id)
			.Property(e => e.CreatedAt).HasConversion(v => v.ToUnixTimeSeconds(), v => DateTimeOffset.FromUnixTimeSeconds(v))
			.Property(e => e.ChangedAt).HasConversion(v => v.ToUnixTimeSeconds(), v => DateTimeOffset.FromUnixTimeSeconds(v));

		builder.Entity<LogItem>()
			.HasTableName(nameof(LogItem))
			.HasIdentity(e => e.Id)
			.Property(e => e.CreatedAt).HasConversion(v => v.ToUnixTimeSeconds(), v => DateTimeOffset.FromUnixTimeSeconds(v))
			.Property(e => e.ChangedAt).HasConversion(v => v.ToUnixTimeSeconds(), v => DateTimeOffset.FromUnixTimeSeconds(v));

		builder.Build();

		return mappingSchema;
	}

	private static DataOptions CreateDataOptions(string connectionString)
		=> new DataOptions()
			.UseSQLite(connectionString)
			.UseMappingSchema(Schema);
}
