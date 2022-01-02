using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20191407075208)]
	public class AddRoleTable: BaseMigration
	{
		public override void Up()
		{
			this.CreateTableBase(TableNames.Roles)
				.WithColumn("Name").AsString(255).NotNullable()
				.WithColumn("NormalizedName").AsString(255).Nullable();
		}

		public override void Down()
		{
			Delete.Table(TableNames.Roles);
		}
	}
}