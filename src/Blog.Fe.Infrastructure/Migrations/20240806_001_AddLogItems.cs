using Blog.Fe.Domain.Entities;
using FluentMigrator;

namespace Blog.Fe.Infrastructure.Migrations;

[Migration(20240806_001)]
public sealed class AddLogItems : Migration
{
	public override void Up()
	{
		Create.Table(nameof(LogItem))
			.WithDescription("LogItems")
			.WithColumn(nameof(LogItem.Id)).AsInt32().PrimaryKey().Identity()
			.WithColumn(nameof(LogItem.CreatedAt)).AsInt64().NotNullable()
			.WithColumn(nameof(LogItem.ChangedAt)).AsInt64().NotNullable()
			.WithColumn(nameof(LogItem.Message)).AsString().NotNullable()
			.WithColumn(nameof(LogItem.Properties)).AsString().NotNullable()
			.WithColumn(nameof(LogItem.LogLevel)).AsInt32().NotNullable();
	}

	public override void Down()
		=> Delete.Table(nameof(LogItem));
}
