using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20192807100000)]
	public class AddMessagesTable: BaseMigration
	{
		public override void Up()
		{
			this.CreateTableBase(TableNames.Messages)
				.WithColumn("Title").AsString(255).NotNullable()
				.WithColumn("Body").AsString().NotNullable();
		}

		public override void Down()
		{
			Delete.Table(TableNames.Messages);
		}
	}
}