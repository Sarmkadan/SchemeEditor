using System.Collections.Generic;
using System.Linq;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Insert;

namespace SchemeEditor.Migration.Migrations
{
	public abstract class BaseMigration : FluentMigrator.Migration
	{
		protected ICreateTableWithColumnSyntax CreateTableBase(string name)
		{
			return Create
				.Table(name)
				.WithColumn("Id").AsInt64().Identity().PrimaryKey().NotNullable()
				.WithColumn("CreatedAt").AsDateTime().NotNullable()
				.WithColumn("CreatedBy").AsInt64().NotNullable()
				.WithColumn("ModifiedAt").AsDateTime().NotNullable()
				.WithColumn("ModifiedBy").AsInt64().NotNullable()
				.WithColumn("DeletedAt").AsDateTime().Nullable()
				.WithColumn("DeletedBy").AsInt64().Nullable();
		}

		protected IInsertDataSyntax AddRows<T>(IInsertDataSyntax insert, IEnumerable<T> data)
		{
			return data.Aggregate(insert, (current, item) => current.Row(item));
		}
	}
}