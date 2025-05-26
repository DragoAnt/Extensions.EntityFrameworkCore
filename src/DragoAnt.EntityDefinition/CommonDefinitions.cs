using System.Reflection;
using System.Xml.XPath;
using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Definitions;
using DragoAnt.EntityDefinition.Definitions;

namespace DragoAnt.EntityDefinition;

/// <summary>
/// Common model definitions
/// </summary>
public static class CommonDefinitions
{
    /// <summary>
    /// Name definition based on <see cref="MemberInfo.Name"/>
    /// </summary>
    public static readonly MemberInfoDefinition<string> Name = new NameMemberInfoDefinition();

    /// <summary>
    /// Remark based on <see cref="DefinitionRemarkAttribute"/>
    /// </summary>
    public static readonly MemberInfoDefinition<string> Remark = new AttributeDefinition<string, DefinitionRemarkAttribute>("Remark");

    /// <summary>
    /// IsObsolete or not based on <see cref="ObsoleteAttribute"/>
    /// </summary>
    public static readonly MemberInfoDefinition<bool> IsObsolete = new CustomAttributeDefinition<bool, ObsoleteAttribute>("IsObsolete", (_, _) => true);

    /// <summary>
    /// Obsolete message or not based on <see cref="ObsoleteAttribute"/>
    /// </summary>
    public static readonly MemberInfoDefinition<string> ObsoleteMessage =
        new CustomAttributeDefinition<string, ObsoleteAttribute>("IsObsolete", (attr, _) => attr.Message);

    /// <summary>
    /// Description from xml documentation
    /// </summary>
    /// <returns></returns>
    public static readonly DefinitionInfo<string> XmlDescription = new("Description");

    /// <summary>
    /// Gets definition for <see cref="CommonDefinitions.XmlDescription"/>
    /// </summary>
    /// <returns></returns>
    public static MemberInfoDefinition<string> GetXmlDescription(Func<Assembly, XPathDocument?>? getCommentDoc = null)
        => new XmlDescriptionMemberInfoDefinition(XmlDescription, getCommentDoc);
        
    /// <summary>
    /// Clr type of a property
    /// </summary>
    public static readonly MemberInfoDefinition<TypeInfo> PropertyClrType =
        new PropertyClrTypeDefinition("ClrType");

    public static class Converts
    {
        /// <summary>
        /// Converts true to X and false to null
        /// </summary>
        /// <returns></returns>
        public static string? BoolToX(bool v) => v ? "X" : null;

        /// <summary>
        /// Converts true to X and false to null
        /// </summary>
        /// <returns></returns>
        public static string? TrueFalse(bool v) => v ? "True" : "False";
    }
}