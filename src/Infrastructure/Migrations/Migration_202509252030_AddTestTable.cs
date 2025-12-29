using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(202509252030)]
    public class Migration_202509252030_AddTestTable : Migration
    {
        public override void Up()
        {
            Create.Table("Test")
                 .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                 .WithColumn("nombre").AsString().Nullable()
                 .WithColumn("estado").AsBoolean().NotNullable().WithDefaultValue(false)
                 .WithColumn("total").AsInt32().NotNullable().WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Table("Test");

        }
    }
}
