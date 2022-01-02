using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20191407074408)]
	public class AddUserTable: BaseMigration
	{
		public override void Up()
		{
			this.CreateTableBase(TableNames.Users)
				.WithColumn("PasswordHash").AsString(255).Nullable()
				.WithColumn("Email").AsString(255).NotNullable()
				.WithColumn("EmailConfirmed").AsBoolean().NotNullable()
				.WithColumn("NormalizedEmail").AsString(255).Nullable()
				.WithColumn("Phone").AsString(255).NotNullable()
				.WithColumn("PhoneConfirmed").AsBoolean().NotNullable()
				.WithColumn("LastName").AsString(255).Nullable()
				.WithColumn("MiddleName").AsString(255).Nullable()
				.WithColumn("City").AsString(255).Nullable()
				.WithColumn("Company").AsString(255).Nullable();
		}

		public override void Down()
		{
			Delete.Table(TableNames.Users);
		}
	}
}