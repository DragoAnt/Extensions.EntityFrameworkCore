using System.Collections;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;

internal static class RelationCaptionStringBuilderExtensions
{
    public static string? GetRelationCaption(this INavigation? property, bool shortName)
    {
        var foreignKey = property?.ForeignKey;

        if (foreignKey is null)
        {
            return null;
        }

        var builder = new StringBuilder();


        builder.AddRelationSideName(foreignKey.PrincipalEntityType,
            foreignKey.PrincipalToDependent, shortName);

        builder.Append("->");

        builder.AddRelationSideName(foreignKey.DeclaringEntityType,
            foreignKey.DependentToPrincipal, shortName);

        return builder.ToString();
    }

    public static StringBuilder AddRelationSideName(this StringBuilder builder, ITypeBase e,
        IPropertyBase? p, bool shortName)
    {
        const string emptyProperty = "*";

        var property = p?.DisplayNameInRelation();
        if (shortName)
        {
            if (property is not null)
            {
                builder.Append(property);
            }
            else
            {
                builder.AddRelationSideName(e, emptyProperty);
            }
        }
        else
        {
            property ??= emptyProperty;
            builder.AddRelationSideName(e, property);
        }
        return builder;
    }

    public static StringBuilder AddRelationSideName(this StringBuilder builder, ITypeBase e,
        string? property = null)
    {
        builder.Append("<b>");
        builder.Append(e.DisplayName());
        builder.Append("</b>");
        if (property != null)
        {
            builder.Append('.');
        }
        builder.Append(property);
        return builder;
    }

    public static string DisplayNameInRelation(this IPropertyBase p)
    {
        var postfix = string.Empty;
        if (p.ClrType.IsAssignableTo(typeof(IEnumerable)))
        {
            postfix = "[]";
        }
        return $"<i>{p.Name}</i>{postfix}";
    }
}