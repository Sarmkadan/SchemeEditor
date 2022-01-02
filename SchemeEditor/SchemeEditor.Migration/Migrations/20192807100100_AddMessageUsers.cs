using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20192807100100)]
	public class AddMessageUsers: BaseMigration
	{

		public override void Up()
		{
			this.CreateTableBase(TableNames.MessageUsers)
				.WithColumn("UserId").AsInt64().NotNullable()
				.WithColumn("MessageId").AsInt64().NotNullable()
				.WithColumn("IsRead").AsBoolean().NotNullable();

			Alter.Table(TableNames.Users)
				.AlterColumn("Id").AsInt64().ReferencedBy(TableNames.MessageUsers, "UserId");
			Alter.Table(TableNames.Messages)
				.AlterColumn("Id").AsInt64().ReferencedBy(TableNames.MessageUsers, "MessageId");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_MessageUsers_UserId_Users_Id")
				.OnTable(TableNames.MessageUsers);

			Delete.ForeignKey("FK_MessageUsers_MessageId_Messages_Id")
				.OnTable(TableNames.MessageUsers);
			
			Delete.Table(TableNames.MessageUsers);
		}
	}
}