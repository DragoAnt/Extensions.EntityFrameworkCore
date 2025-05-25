namespace DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

public sealed class CustomAttributeDefinition<T, TAttr> : AttributeDefinitionBase<T, TAttr> where TAttr : Attribute
{
    private readonly Func<TAttr, DefinitionContext, T?> _extract;

    /// <inheritdoc />
    public CustomAttributeDefinition(string name, Func<TAttr, DefinitionContext, T?> extract, bool copyParentValueIfUndefined = false,
        Func<T, string>? convertToString = null)
        : base(name, copyParentValueIfUndefined, convertToString)
    {
        _extract = extract ?? throw new ArgumentNullException(nameof(extract));
    }

    /// <inheritdoc />
    protected override T? GetValue(TAttr attr, DefinitionContext context)
    {
        return _extract(attr, context);
    }
}