using System.Collections.Concurrent;
using System.Reflection;
using System.Xml.XPath;
using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;
using DragoAnt.Extensions.EntityDefinition.XmlComments;

namespace DragoAnt.Extensions.EntityDefinition.Definitions;

internal sealed class XmlDescriptionMemberInfoDefinition : MemberInfoDefinition<string>
{
    private static XPathDocument? GetCommentDocDefault(Assembly assembly)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        return !File.Exists(xmlPath) ? null : new XPathDocument(xmlPath);
    }

    private readonly Func<Assembly, XPathDocument?> _getCommentDoc;

    /// <inheritdoc />
    public XmlDescriptionMemberInfoDefinition(DefinitionInfo<string> info,
        Func<Assembly, XPathDocument?>? getCommentDoc = null)
        : base(info)
    {
        _getCommentDoc = getCommentDoc ?? GetCommentDocDefault;
    }

    /// <inheritdoc />
    public override string? Extract(MemberInfo? member, string? parentValue,
        EntityDefinitionRow entityRow, PropertyDefinitionRow? row, DefinitionContext context)
    {
        var extractContext = context.GetOrAdd(Info, () => new ExtractContext(_getCommentDoc));

        if (member is null)
        {
            return null;
        }
        var navigator = extractContext.GetNavigator(member.Module.Assembly);
        if (navigator is null)
        {
            return null;
        }

        //based on https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/95cb4d370e08e54eb04cf14e7e6388ca974a686e/src/Swashbuckle.AspNetCore.SwaggerGen/XmlComments/XmlCommentsParameterFilter.cs 

        var memberName = member switch
        {
            TypeInfo typeInfo => XmlCommentsNodeNameHelper.GetMemberNameForType(typeInfo),
            PropertyInfo => XmlCommentsNodeNameHelper.GetMemberNameForFieldOrProperty(member),
            _ => throw new NotSupportedException($"Member type '{member.MemberType}' doesn't supported.")
        };

        var propertyNode = navigator.SelectSingleNode($"/doc/members/member[@name='{memberName}']");

        var summaryNode = propertyNode?.SelectSingleNode("summary");
        return summaryNode != null ? XmlCommentsTextHelper.Humanize(summaryNode.InnerXml) : null;
    }

    private sealed class ExtractContext
    {
        private readonly ConcurrentDictionary<Assembly, XPathNavigator?> _docs = new();
        private readonly Func<Assembly, XPathDocument?> _getCommentDoc;

        public ExtractContext(Func<Assembly, XPathDocument?> getCommentDoc)
        {
            _getCommentDoc = getCommentDoc;
        }

        public XPathNavigator? GetNavigator(Assembly assembly)
        {
            return _docs.GetOrAdd(assembly, CreateNavigator);
        }

        private XPathNavigator? CreateNavigator(Assembly assembly)
        {
            var xmlDoc = _getCommentDoc(assembly);
            return xmlDoc?.CreateNavigator();
        }
    }
}