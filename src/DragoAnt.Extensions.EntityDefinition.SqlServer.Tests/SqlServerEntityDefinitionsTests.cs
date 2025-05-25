using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Relational;
using DragoAnt.Extensions.EntityDefinition.Model.Definitions;
// ReSharper disable UnusedVariable

namespace DragoAnt.Extensions.EntityDefinition.SqlServer.Tests;

public class SqlServerEntityDefinitionsTests : SqlServerEntityTestsBase
{
    [Fact]
    public void TestCsvEntities()
    {
        var csv = DbContext.Model.GenerateCsv(InitEntitiesReaderOptions);
    }

    [Fact]
    public void TestCsvProperties()
    {
        var csv = DbContext.Model.GenerateCsv(InitPropertiesReaderOptions);
    }

    [Fact]
    public void TestDefTableEntities()
    {
        var table = DbContext.Model.GenerateEntityDefinitionTable(InitEntitiesReaderOptions);
    }

    [Fact]
    public void TestDefTableProperties()
    {
        var table = DbContext.Model.GenerateEntityDefinitionTable(InitPropertiesReaderOptions);
    }

    private static void InitEntitiesReaderOptions(IEntityFrameworkCoreDefinitionOptions options)
    {
        options.AddCommonConvert<bool>(CommonDefinitions.Converts.BoolToX);

        options.AddEntityColumn(CustomDefinitions.Domain.ToEntity());
        options.AddEntityColumn(EFCommonDefinitions.Entities.Name);
        options.AddEntityColumn(EFCommonDefinitions.Entities.BaseEntityName);
        options.AddEntityColumn(EFRelationalDefinitions.Entities.DbName);
        options.AddEntityColumn(EFRelationalDefinitions.Entities.Type);
        options.AddEntityColumn(EFCommonDefinitions.Entities.Remark);
        options.AddEntityColumn(EFCommonDefinitions.Entities.IsObsolete);
        options.AddEntityColumn(EFCommonDefinitions.Entities.GetXmlDescription());
    }

    private void InitPropertiesReaderOptions(IEntityFrameworkCoreDefinitionOptions options)
    {
        options.AddCommonConvert<bool>(CommonDefinitions.Converts.BoolToX);

        options.AddEntityColumn(CustomDefinitions.Domain.ToEntity(), "Entity:Domain");
        options.AddPropertyColumn(CustomDefinitions.Domain.ToProperty());
        options.AddPropertyColumn(CustomDefinitions.IsDomainDifferent.ToProperty());

        options.AddEntityColumn(EFCommonDefinitions.Entities.Name, "Entity:Name");
        options.AddEntityColumn(EFRelationalDefinitions.Entities.DbName, "Entity:DbName");
        options.AddEntityColumn(EFRelationalDefinitions.Entities.Type, "Entity:Type");

        options.AddPropertyColumn(EFCommonDefinitions.Properties.Name);
        options.AddPropertyColumn(EFCommonDefinitions.Properties.ClrType);
        options.AddPropertyColumn(EFRelationalDefinitions.Properties.ColumnName);
        options.AddPropertyColumn(EFRelationalDefinitions.Properties.DbColumnType);
        options.AddPropertyColumn(EFRelationalDefinitions.Properties.IsColumnNullable);
        options.AddPropertyColumn(EFCommonDefinitions.Properties.Remark);
        options.AddPropertyColumn(EFCommonDefinitions.Properties.IsNavigation);
        options.AddPropertyColumn(EFCommonDefinitions.Properties.IsObsolete);
        options.AddPropertyColumn(EFCommonDefinitions.Properties.IsShadow);
        options.AddPropertyColumn(EFRelationalDefinitions.Properties.IsComputed);
        options.AddPropertyColumn(EFRelationalDefinitions.Properties.ComputedSql);

        options.AddPropertyColumn(EFCommonDefinitions.Properties.GetXmlDescription());
    }
}