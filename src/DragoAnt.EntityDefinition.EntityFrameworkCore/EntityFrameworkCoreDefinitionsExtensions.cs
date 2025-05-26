using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Definitions;
using DragoAnt.EntityDefinition.Contracts.Table;
using DragoAnt.EntityDefinition.Writer;
using DragoAnt.Shared.Tables;

namespace DragoAnt.EntityDefinition.EntityFrameworkCore;

public static class EntityFrameworkCoreDefinitionsExtensions
{
    public static DefinitionMap GenerateMap(this IModel model,
        Action<IEntityFrameworkCoreDefinitionOptions> readerOptionsInit)
    {
        var options = new EntityFrameworkCoreDefinitionOptions();
        readerOptionsInit(options);

        return model.GenerateMap(options.ReaderOptions);
    }

    public static DefinitionMap GenerateMap(this IModel model, EntityFrameworkCoreDefinitionReaderOptions options)
    {
        var reader = new EntityFrameworkCoreDefinitionReader(model, options);
        var map = reader.Read();

        return map;
    }

    public static string GenerateCsv(this IModel model,
        Action<IEntityFrameworkCoreDefinitionOptions> readerOptionsInit,
        char delimiter = ',')
    {
        var options = new EntityFrameworkCoreDefinitionOptions();
        readerOptionsInit(options);

        return model.GenerateCsv(options, delimiter);
    }

    public static string GenerateCsv(this IModel model,
        EntityFrameworkCoreDefinitionOptions options,
        char delimiter = ',')
    {
        var map = GenerateMap(model, options.ReaderOptions);

        var writer = new EntityDefinitionWriter(options.WriterOptions);
        var tableWriter = new CsvTableWriter(delimiter);

        writer.Write(map, tableWriter);
        return tableWriter.Build();
    }

    public static EntityDefinitionTable GenerateEntityDefinitionTable(this IModel model,
        Action<IEntityFrameworkCoreDefinitionOptions> readerOptionsInit)
    {
        var options = new EntityFrameworkCoreDefinitionOptions();
        readerOptionsInit(options);

        return model.GenerateEntityDefinitionTable(options);
    }

    public static EntityDefinitionTable GenerateEntityDefinitionTable(this IModel model,
        EntityFrameworkCoreDefinitionOptions options)
    {
        var map = GenerateMap(model, options.ReaderOptions);

        var writer = new EntityDefinitionWriter(options.WriterOptions);

        var table = writer.Write(map);
        return table;
    }

    /// <summary>
    /// Add entity column
    /// </summary>
    /// <param name="options"></param>
    /// <param name="definition"></param>
    /// <param name="columnName"></param>
    /// <param name="convertToString"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddEntityColumn<T>(this IEntityFrameworkCoreDefinitionOptions options, 
        MemberInfoDefinition<T> definition, string? columnName = null,
        Func<T?, string?>? convertToString = null)
    {
        options.AddEntityColumn(definition, columnName, convertToString);
    }

    /// <summary>
    /// Add property column
    /// </summary>
    /// <param name="options"></param>
    /// <param name="definition"></param>
    /// <param name="columnName"></param>
    /// <param name="convertToString"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddPropertyColumn<T>(this IEntityFrameworkCoreDefinitionOptions options,
        MemberInfoDefinition<T> definition, string? columnName = null, Func<T?, string?>? convertToString = null)
    {
        options.AddPropertyColumn(definition, columnName, convertToString);
    }
}