using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;

namespace DragoAnt.Extensions.EntityDefinition.AspNetCore;

public interface ICsvEntityFrameworkCoreDefinitionOptions : IEntityFrameworkCoreDefinitionOptions
{
    char Delimiter { get; set; }
}

public sealed class CsvEntityFrameworkCoreDefinitionOptions : EntityFrameworkCoreDefinitionOptions, ICsvEntityFrameworkCoreDefinitionOptions
{
    public char Delimiter { get; set; } = ',';
}