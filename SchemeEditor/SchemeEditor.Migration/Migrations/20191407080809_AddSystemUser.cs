using System;
using FluentMigrator;
using FluentMigrator.SqlServer;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20191407080809)]
	public class AddSystemUser: BaseMigration
	{
		private readonly dynamic _user = new
		{
			CreatedAt = DateTime.UtcNow,
			CreatedBy = 1,
			Email = "user@localhost",
			EmailConfirmed = false,
			ModifiedAt = DateTime.UtcNow,
			PhoneConfirmed = false,
			ModifiedBy = 1,
			Phone = "000",
			Name = "SystemUser"
		};

		public override void Up()
		{
			Alter.Table(TableNames.Users).AddColumn("Name").AsString(255).NotNullable();
			Insert.IntoTable(TableNames.Users)
				.WithIdentityInsert()
				.Row(_user);
		}

		public override void Down()
		{
			Delete.FromTable(TableNames.Users).AllRows();
			Delete.Column("Name").FromTable(TableNames.Users);

		}
	}
}