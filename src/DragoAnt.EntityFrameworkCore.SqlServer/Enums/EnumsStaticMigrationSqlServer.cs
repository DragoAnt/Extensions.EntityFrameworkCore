using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.EntityFrameworkCore.SqlServer.Enums;

public sealed class EnumsStaticMigrationSqlServer : EnumsStaticMigration
{
    /// <inheritdoc />
    public EnumsStaticMigrationSqlServer(DbContext context, string schemaName = "enum") 
        : base(context, schemaName)
    {
            
    }

    /// <inheritdoc />
    public override IEnumerable<MigrationOperation> GetRevertOperations()
    {
        yield return new SqlOperation
        {
            Sql = $@"
DECLARE enums_keys CURSOR 
FOR
SELECT SCHEMA_NAME(t.schema_id), t.name as table_name, fk.name AS fk_name 
FROM sys.foreign_keys fk
INNER JOIN sys.tables t ON t.object_id = fk.parent_object_id
INNER JOIN sys.tables t_ref ON t_ref.object_id = fk.referenced_object_id
WHERE SCHEMA_NAME(t_ref.schema_id) = '{SchemaName}'

DECLARE @schema_name sysname, @table_name sysname, @fk_name sysname

OPEN enums_keys
FETCH NEXT FROM enums_keys INTO @schema_name, @table_name, @fk_name

WHILE @@FETCH_STATUS = 0
BEGIN
  EXEC('ALTER TABLE ['+@schema_name+'].['+@table_name+'] DROP CONSTRAINT ['+@fk_name+']')
  FETCH NEXT FROM enums_keys INTO @schema_name, @table_name, @fk_name
END 
CLOSE enums_keys;
DEALLOCATE enums_keys;

DECLARE enums_tables CURSOR 
FOR
SELECT SCHEMA_NAME(t.schema_id), t.name as table_name 
FROM sys.tables t
WHERE SCHEMA_NAME(t.schema_id) = '{SchemaName}'

OPEN enums_tables
FETCH NEXT FROM enums_tables INTO @schema_name, @table_name

WHILE @@FETCH_STATUS = 0
BEGIN
  EXEC('DROP TABLE ['+@schema_name+'].['+@table_name+']')
  FETCH NEXT FROM enums_tables INTO @schema_name, @table_name
END 
CLOSE enums_tables;
DEALLOCATE enums_tables;
"
        };
    }
}