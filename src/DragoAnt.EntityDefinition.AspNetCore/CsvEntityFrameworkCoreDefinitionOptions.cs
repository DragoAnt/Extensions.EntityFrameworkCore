using DragoAnt.EntityDefinition.EntityFrameworkCore;

namespace DragoAnt.EntityDefinition.AspNetCore;

public interface ICsvEntityFrameworkCoreDefinitionOptions : IEntityFrameworkCoreDefinitionOptions
{
    char Delimiter { get; set; }
}

public sealed class CsvEntityFrameworkCoreDefinitionOptions : EntityFrameworkCoreDefinitionOptions, ICsvEntityFrameworkCoreDefinitionOptions
{
    public char Delimiter { get; set; } = ',';
}