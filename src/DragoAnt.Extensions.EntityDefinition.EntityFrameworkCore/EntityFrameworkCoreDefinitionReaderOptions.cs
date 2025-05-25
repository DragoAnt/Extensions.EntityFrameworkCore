using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;
using static DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.EntityFrameworkDefinitionReaderOptions;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;

public sealed class EntityFrameworkCoreDefinitionReaderOptions
{
    private readonly List<IEFEntityDefinition> _entityDefinitions = new();
    private readonly List<IEFPropertyDefinition> _propertyDefinitions = new();
    private Func<IEntityType, bool>? _filterEntities;
    private Func<IEntityType, IPropertyBase, bool>? _filterProperties;

    public EntityFrameworkDefinitionReaderOptions ReaderOptions { get; set; } = ExcludeOwnedTypeShadowProperties |
                                                                                ExcludeOwnedTypeIgnoredProperties;

    public void SetEntitiesFilter(Func<IEntityType, bool> filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));
        }
        if (_filterEntities == null)
        {
            _filterEntities = filter;
            return;
        }
            
        var tmp = _filterEntities;
        _filterEntities = t => tmp(t) && filter(t);
    }

    public void SetPropertiesFilter(Func<IEntityType, IPropertyBase, bool> filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));
        }
        if (_filterProperties == null)
        {
            _filterProperties = filter;
            return;
        }
            
        var tmp = _filterProperties;
        _filterProperties = (t, p) => tmp(t, p) && filter(t, p);
    }

    internal bool TryAddEntityDefinition(IEFEntityDefinition definition)
    {
        if (_entityDefinitions.Any(d => d.Info == definition.Info))
        {
            return false;
        }
        _entityDefinitions.Add(definition);
        return true;
    }

    internal void AddEntityDefinition(IEFEntityDefinition definition)
    {
        _entityDefinitions.Add(definition);
    }

    internal bool TryAddPropertyDefinition(IEFPropertyDefinition definition)
    {
        if (_propertyDefinitions.Any(d => d.Info == definition.Info))
        {
            return false;
        }
        _propertyDefinitions.Add(definition);
        return true;
    }

    internal void AddPropertyDefinition(IEFPropertyDefinition definition)
    {
        _propertyDefinitions.Add(definition);
    }

    internal Func<IEntityType, bool> GetEntitiesFilter()
    {
        var filter = _filterEntities ?? (_ => true);
        if (ReaderOptions.HasFlag(ExcludeAbstractEntities))
        {
            var tempFilter = filter;
            filter = type => !type.IsAbstract() && tempFilter(type);
        }
        return filter;
    }

    internal Func<IEntityType, IPropertyBase, bool> GetPropertiesFilter()
    {
        var filter = _filterProperties ?? ((_, _) => true);
        if (ReaderOptions.HasFlag(ExcludeScalarProperties))
        {
            var tempFilter = filter;
            filter = (t, p) => p is not IProperty && tempFilter(t, p);
        }
        if (ReaderOptions.HasFlag(ExcludeRelationNavigationProperties))
        {
            var tempFilter = filter;
            filter = (t, p) => (p is not INavigation n || n.TargetEntityType.IsOwned()) && tempFilter(t, p);
        }
        if (ReaderOptions.HasFlag(ExcludeOwnedNavigationProperties))
        {
            var tempFilter = filter;
            filter = (t, p) => (p is not INavigation n || !n.TargetEntityType.IsOwned()) && tempFilter(t, p);
        }
        if (ReaderOptions.HasFlag(ExcludeShadowProperties))
        {
            var tempFilter = filter;
            filter = (t, p) => !p.IsShadowProperty() && tempFilter(t, p);
        }
        return filter;
    }

    public Func<IEntityType, IPropertyBase, bool> GetOwnedTypePropertiesFilter()
    {
        var filter = GetPropertiesFilter();
        if (ReaderOptions.HasFlag(ExcludeOwnedTypeShadowProperties))
        {
            var tempFilter = filter;
            filter = (t, p) => !p.IsShadowProperty() && tempFilter(t, p);
        }
        return filter;
    }

    internal IEFEntityDefinition[] GetEntityDefinitions()
    {
        return _entityDefinitions.OrderBy(d => d.ReadOrder).ToArray();
    }

    internal IEFPropertyDefinition[] GetPropertyDefinitions()
    {
        return _propertyDefinitions.OrderBy(d => d.ReadOrder).ToArray();
    }
}