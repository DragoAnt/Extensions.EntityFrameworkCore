using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.EntityFrameworkCore.PostgreSQL;

public sealed class EnumsStaticMigrationFactoryPostgreSQL : IEnumsStaticMigrationFactory
{
    /// <inheritdoc />
    public IStaticSqlMigration Create(DbContext context, string schemaName = "enum")
    {
        return new EnumsStaticMigrationPostgreSQL(context, schemaName);
    }
}

public sealed class EnumsStaticMigrationPostgreSQL : EnumsStaticMigration
{
    /// <inheritdoc />
    public EnumsStaticMigrationPostgreSQL(DbContext context, string schemaName = "enum") 
        : base(context, schemaName)
    {
    }

    /// <inheritdoc />
    public override IEnumerable<MigrationOperation> GetRevertOperations()
    {
        yield return new SqlOperation
        {
            Sql = $@"
DO $$
DECLARE
    r RECORD;
BEGIN
    -- Drop foreign key constraints
    FOR r IN (
        SELECT tc.table_schema, tc.table_name, tc.constraint_name
        FROM information_schema.table_constraints tc
        JOIN information_schema.constraint_table_usage ccu 
            ON tc.constraint_name = ccu.constraint_name 
            AND tc.table_schema = ccu.table_schema
        WHERE tc.constraint_type = 'FOREIGN KEY'
        AND ccu.table_schema = '{SchemaName}'
    ) LOOP
        EXECUTE format('ALTER TABLE %I.%I DROP CONSTRAINT %I',
            r.table_schema, r.table_name, r.constraint_name);
    END LOOP;

    -- Drop tables in the enum schema
    FOR r IN (
        SELECT table_name
        FROM information_schema.tables
        WHERE table_schema = '{SchemaName}'
        AND table_type = 'BASE TABLE'
    ) LOOP
        EXECUTE format('DROP TABLE IF EXISTS %I.%I CASCADE',
            '{SchemaName}', r.table_name);
    END LOOP;
END $$;
"
        };
    }
} 