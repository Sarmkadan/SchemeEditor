using System;
using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20191407103309)]
	public class UserRolesAddRelationsAndData: BaseMigration
	{
		
		public override void Up()
		{
			Insert.IntoTable(TableNames.UserRoles)
				.Row(new
				{
					CreatedAt = DateTime.UtcNow,
					CreatedBy = 1,
					ModifiedAt = DateTime.UtcNow,
					ModifiedBy = 1,
					RoleId = 1,
					UserId = 1
				});
		}

		public override void Down()
		{
			Delete.FromTable(TableNames.UserRoles).AllRows();
		}
	}
}