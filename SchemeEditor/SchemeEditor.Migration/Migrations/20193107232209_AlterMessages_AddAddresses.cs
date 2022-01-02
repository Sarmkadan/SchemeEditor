using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20193107232209)]
	public class AlterMessages_AddAddresses: BaseMigration
	{

		public override void Up()
		{
			Alter.Table(TableNames.Messages)
				.AddColumn("Addresses")
				.AsString(255)
				.Nullable();
		}

		public override void Down()
		{
			Delete.Column("Addresses")
				.FromTable(TableNames.Messages);
		}
	}
}