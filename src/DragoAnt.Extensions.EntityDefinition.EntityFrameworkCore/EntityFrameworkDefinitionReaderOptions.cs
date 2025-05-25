namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;

[Flags]
public enum EntityFrameworkDefinitionReaderOptions
{
    None = 0,
    ExcludeIgnoredProperties = 0x01,
    ExcludeScalarProperties = 0x02,
    ExcludeRelationNavigationProperties = 0x04,
    ExcludeOwnedNavigationProperties = 0x08,
    ExcludeAbstractEntities = 0x10,
    ExcludeShadowProperties = 0x20,
    ExcludeOwnedTypeIgnoredProperties = 0x40,
    ExcludeOwnedTypeShadowProperties = 0x80
}