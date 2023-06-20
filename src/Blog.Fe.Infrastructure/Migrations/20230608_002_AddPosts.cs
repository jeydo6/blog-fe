using Blog.Fe.Domain.Entities;
using FluentMigrator;

namespace Blog.Fe.Infrastructure.Migrations;

[Migration(20230608_002)]
public sealed class AddPosts : Migration
{
	public override void Up()
	{
		Create.Table(nameof(Post))
			.WithDescription("Posts")
			.WithColumn(nameof(Post.Id)).AsInt32().PrimaryKey().Identity()
			.WithColumn(nameof(Post.Url)).AsString().Unique()
			.WithColumn(nameof(Post.Title)).AsString().NotNullable()
			.WithColumn(nameof(Post.Description)).AsString().NotNullable()
			.WithColumn(nameof(Post.State)).AsInt32().NotNullable()
			.WithColumn(nameof(Post.CreatedAt)).AsInt64().NotNullable()
			.WithColumn(nameof(Post.ChangedAt)).AsInt64().NotNullable();
	}

	public override void Down()
	{
		Delete.Table(nameof(Post));
	}
}
