using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;

public interface IEntityFrameworkCoreDefinitionOptions
{
    void AddCommonConvert<T>(Func<T?, string?> convertToString);
    EntityFrameworkDefinitionReaderOptions ReaderOptions { get; set; }
    void SetEntitiesFilter(Func<IEntityType, bool> filter);
    void SetPropertiesFilter(Func<IEntityType, IPropertyBase, bool> filter);
        
    /// <summary>
    /// Add entity column if it doesn't exists
    /// </summary>
    /// <param name="definition"></param>
    /// <param name="columnName"></param>
    /// <param name="convertToString"></param>
    /// <typeparam name="T"></typeparam>
    void TryAddEntityColumn<T>(EFEntityDefinition<T> definition, string? columnName = null, Func<T?, string?>? convertToString = null);
        
    /// <summary>
    /// Add property column if it doesn't exists
    /// </summary>
    /// <param name="definition"></param>
    /// <param name="columnName"></param>
    /// <param name="convertToString"></param>
    /// <typeparam name="T"></typeparam>
    void TryAddPropertyColumn<T>(EFPropertyDefinition<T> definition, string? columnName = null, Func<T?, string?>? convertToString = null);
        
    /// <summary>
    /// Add entity column
    /// </summary>
    /// <param name="definition"></param>
    /// <param name="columnName"></param>
    /// <param name="convertToString"></param>
    /// <typeparam name="T"></typeparam>
    void AddEntityColumn<T>(EFEntityDefinition<T> definition, string? columnName = null, Func<T?, string?>? convertToString = null);
        
    /// <summary>
    /// Add property column
    /// </summary>
    /// <param name="definition"></param>
    /// <param name="columnName"></param>
    /// <param name="convertToString"></param>
    /// <typeparam name="T"></typeparam>
    void AddPropertyColumn<T>(EFPropertyDefinition<T> definition, string? columnName = null, Func<T?, string?>? convertToString = null);
}