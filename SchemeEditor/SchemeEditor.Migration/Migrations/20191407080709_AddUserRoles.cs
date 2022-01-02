using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20191407080709)]
	public class AddUserRoles: BaseMigration
	{

		public override void Up()
		{
			this.CreateTableBase(TableNames.UserRoles)
				.WithColumn("UserId").AsInt64().NotNullable()
				.WithColumn("RoleId").AsInt64().NotNullable();

			Alter.Table(TableNames.Users)
				.AlterColumn("Id").AsInt64().ReferencedBy(TableNames.UserRoles, "UserId");
			Alter.Table(TableNames.Roles)
				.AlterColumn("Id").AsInt64().ReferencedBy(TableNames.UserRoles, "RoleId");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_UserRoles_UserId_Users_Id")
				.OnTable(TableNames.UserRoles);

			Delete.ForeignKey("FK_UserRoles_RoleId_Roles_Id")
				.OnTable(TableNames.UserRoles);
			
			Delete.Table(TableNames.UserRoles);
		}
	}
}