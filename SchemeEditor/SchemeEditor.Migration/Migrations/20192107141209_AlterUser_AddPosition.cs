using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20192107141209)]
	public class AlterUser_AddPosition: BaseMigration
	{

		public override void Up()
		{
			Alter.Table(TableNames.Users)
				.AddColumn("Position")
				.AsString(255)
				.Nullable();
		}

		public override void Down()
		{
			Delete.Column("Position")
				.FromTable(TableNames.Users);
		}
	}
}