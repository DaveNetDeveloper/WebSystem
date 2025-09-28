using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(202509252200)]
    public class Migration_202509252200_DropTestTable : Migration
    {
        public override void Up()
        {
            Delete.Table("Test");
        }

        public override void Down()
        {
            Create.Table("Test")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("nombre").AsString().Nullable()
            .WithColumn("estado").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("total").AsInt32().NotNullable().WithDefaultValue(0);
        }
    }
}
