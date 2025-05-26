using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragoAnt.EntityFrameworkCore.Testing;

public static class CheckDbContextEntityMappingExtensions
{
    public static void CheckEntities(this DbContext dbContext, Func<IEntityType, bool>? filterEntities = null)
    {
        var entities = dbContext.Model.GetEntityTypes().Where(e => !e.IsAbstract() &&
                                                                   !e.IsOwned() &&
                                                                   !e.HasSharedClrType);
        filterEntities ??= _ => true;
        entities = entities.Where(filterEntities);

        foreach (var entity in entities)
        {
            try
            {
                CheckEntity(dbContext, entity);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Entity", entity.Name);
                throw;
            }
        }
    }

    private static readonly MethodInfo CheckEntityMethod =
        typeof(CheckDbContextEntityMappingExtensions).GetMethod(nameof(CheckEntityGeneric), BindingFlags.NonPublic | BindingFlags.Static)!;

    public static void CheckEntity(this DbContext dbContext, IEntityType entity)
    {
        var checkMethodTyped = CheckEntityMethod.MakeGenericMethod(entity.ClrType);
        checkMethodTyped.Invoke(null, [dbContext, entity]);
    }

    public static void CheckEntity<T>(this DbContext dbContext)
        where T : class
    {
        var entityType = dbContext.Model.FindEntityType(typeof(T))!;
        CheckEntityGeneric<T>(dbContext, entityType);
    }

    private static void CheckEntityGeneric<T>(DbContext dbContext, IEntityType entity)
        where T : class
    {
        var query = dbContext.Set<T>().AsQueryable();

        var navigations = entity
            .GetDerivedTypesInclusive()
            .SelectMany(type => type.GetNavigations())
            .Distinct();

        foreach (var property in navigations)
        {
            query = query.Include(property.Name);
        }

        // ReSharper disable once UnusedVariable
        var list = query.Take(1).ToList();
    }
}