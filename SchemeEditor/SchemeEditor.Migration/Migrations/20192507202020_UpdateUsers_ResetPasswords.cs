using FluentMigrator;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Infrastructure;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20192507202020)]
	public class UpdateUsers_ResetPasswords: BaseMigration
	{

		public override void Up()
		{
			Update.Table(TableNames.Users)
				.Set(new { PasswordHash = PasswordHasher.HashPassword("111") })
				.AllRows();
		}

		public override void Down()
		{
			// Do nothing
		}
	}
}