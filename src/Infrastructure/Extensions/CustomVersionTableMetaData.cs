using FluentMigrator.Runner.VersionTableInfo;

namespace Infrastructure.Extensions
{
    [VersionTableMetaData]
    public class CustomVersionTableMetaData : DefaultVersionTableMetaData
    {
        public override string TableName => "MigrationHistory";
    }
}