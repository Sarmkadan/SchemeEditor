using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20192107180000)]
	public class AlterUser_AddIsBlocked: BaseMigration
	{

		public override void Up()
		{
			Alter.Table(TableNames.Users)
				.AddColumn("IsBlocked")
				.AsBoolean()
				.Nullable();
			Update.Table(TableNames.Users)
				.Set(new { IsBlocked = false })
				.AllRows();
			Alter.Column("IsBlocked")
				.OnTable(TableNames.Users)
				.AsBoolean()
				.NotNullable();
		}

		public override void Down()
		{
			Delete.Column("IsBlocked")
				.FromTable(TableNames.Users);
		}
	}
}