using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;
using FluentMigrator.SqlServer;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Settings;

namespace SchemeEditor.Migration.Migrations
{
	[Migration(20191407081809)]
	public class AddStartRoles: BaseMigration
	{
		private readonly IList<dynamic> _roles = BasicRoles.Roles.Select(x =>
			new
			{
				CreatedAt = DateTime.UtcNow,
				CreatedBy = 1,
				ModifiedAt = DateTime.UtcNow,
				ModifiedBy = 1,
				Name = x.Value,
				NormalizedName = x.Key
			} as dynamic
		).ToList();

		public override void Up()
		{
			this.AddRows(Insert.IntoTable(TableNames.Roles).WithIdentityInsert(), this._roles);
		}

		public override void Down()
		{
			Delete.FromTable(TableNames.Roles).AllRows();
		}
	}
}