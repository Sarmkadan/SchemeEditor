using FluentMigrator;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Migration.Migrations
{
    [Migration(20191607192944)]
    public class AlterSchemeTableAddColumnBody : BaseMigration
    {
        public override void Up()
        {
            Alter.Table(TableNames.Schemes)
                .AddColumn("Body")
                .AsString()
                .Nullable();
        }

        public override void Down()
        {
            Delete.Column("Body").FromTable(TableNames.Schemes);
        }
    }
}