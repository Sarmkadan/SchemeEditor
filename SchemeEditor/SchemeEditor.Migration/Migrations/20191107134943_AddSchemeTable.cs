using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
    [Migration(20191107134943)]
    public class AddSchemeTable: BaseMigration
    {
        public override void Up()
        {
	        this.CreateTableBase(TableNames.Schemes)
                .WithColumn("Name").AsString(255).NotNullable();
        }

        public override void Down()
        {
	        Delete.Table(TableNames.Schemes);
        }
    }
}