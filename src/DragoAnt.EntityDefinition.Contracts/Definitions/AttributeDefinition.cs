namespace DragoAnt.EntityDefinition.Contracts.Definitions;

public sealed class AttributeDefinition<T, TAttr> : AttributeDefinitionBase<T, TAttr>
    where TAttr : DefinitionAttribute
{
    /// <inheritdoc />
    public AttributeDefinition(string name, bool copyParentValueIfUndefined = false, Func<T, string>? convertToString = null)
        : base(name, copyParentValueIfUndefined, convertToString)
    {
    }

    /// <inheritdoc />
    protected override T? GetValue(TAttr attr, DefinitionContext context)
    {
        return (T?)attr.Value;
    }
}