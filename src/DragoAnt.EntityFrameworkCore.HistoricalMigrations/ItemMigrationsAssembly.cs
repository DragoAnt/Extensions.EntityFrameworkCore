using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations;

public sealed class ItemMigrationsAssembly : MigrationsAssembly
{
    /// <summary>
    /// Set a private Property Value on a given Object. Uses Reflection.
    /// </summary>
    /// <typeparam name="T">Type of the Property</typeparam>
    /// <param name="obj">Object from where the Property Value is returned</param>
    /// <param name="propName">Propertyname as string.</param>
    /// <param name="val">the value to set</param>
    /// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
    private static void SetPrivateFieldValue<T>(object obj, string propName, T val)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
        var t = obj.GetType();
        FieldInfo? fi = null;
        while (fi == null && t != null)
        {
            fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t = t.BaseType;
        }
        if (fi == null)
        {
            throw new ArgumentOutOfRangeException(nameof(propName), $"Field {propName} was not found in Type {obj.GetType().FullName}");
        }
        fi.SetValue(obj, val);
    }

    /// <inheritdoc />
    public ItemMigrationsAssembly(
        Type dbContextAnchorType,
        ICurrentDbContext currentContext,
        IDbContextOptions options,
        IMigrationsIdGenerator idGenerator,
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
        : base(currentContext, options, idGenerator, logger)
    {
        Assembly = dbContextAnchorType.Assembly;
        SetPrivateFieldValue(this, "_contextType", dbContextAnchorType);
    }

    /// <inheritdoc />
    public override Assembly Assembly { get; }

    /// <inheritdoc />
    public override ModelSnapshot ModelSnapshot
        => throw new NotSupportedException("Can't use snapshot from aggregated migrations assembly");
}