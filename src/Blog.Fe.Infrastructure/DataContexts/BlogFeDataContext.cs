using System;
using Blog.Fe.Domain.Entities;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Blog.Fe.Infrastructure.DataContexts;

internal sealed class BlogFeDataContext : DataConnection
{
	private static readonly MappingSchema _mappingSchema = CreateMappingSchema();
	
	public BlogFeDataContext(string connectionString) : base(CreateDataOptions(connectionString))
	{
		InlineParameters = true;
	}

	public ITable<Post> Posts => this.GetTable<Post>();
	
	private static MappingSchema CreateMappingSchema()
	{
		var mappingSchema = new MappingSchema();
		var builder = new FluentMappingBuilder(mappingSchema);

		builder.Entity<Post>()
			.HasTableName(nameof(Post))
			.HasIdentity(e => e.Id)
			.Property(e => e.CreatedAt).HasConversion(v => v.ToUnixTimeSeconds(), v => DateTimeOffset.FromUnixTimeSeconds(v))
			.Property(e => e.ChangedAt).HasConversion(v => v.ToUnixTimeSeconds(), v => DateTimeOffset.FromUnixTimeSeconds(v));

		builder.Build();

		return mappingSchema;
	}

	private static DataOptions CreateDataOptions(string connectionString)
		=> new DataOptions()
			.UseSQLite(connectionString)
			.UseMappingSchema(_mappingSchema);
}
