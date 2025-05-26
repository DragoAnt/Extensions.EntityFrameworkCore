using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.EntityDefinition.Flowchart;

public class FlowchartGraphGroup
{
    private readonly Action<object?, FlowchartStyleClass>? _fillStyle;
    private readonly Func<object?, string> _extractItemId;
    private readonly Func<object?, string?> _extractCaption;

    public static FlowchartGraphGroup Create<T>(DefinitionInfo<T> info,
        Action<T?, FlowchartStyleClass>? fillStyle,
        Func<T?, string>? extractItemId, Func<T?, string?>? extractCaption, bool skipDuringClean)
    {
        Action<object?, FlowchartStyleClass>? fillStyleAction = fillStyle == null
            ? null
            : (v, styleClass) =>
            {
                if (v is not null)
                {
                    fillStyle((T?)v, styleClass);
                }
                else
                {
                    fillStyle(default, styleClass);
                }
            };
        Func<object?, string>? extractItemIdFunc = extractItemId == null
            ? null
            : v => v is not null ? extractItemId((T?)v) : extractItemId(default);

        Func<object?, string?>? extractCaptionFunc = extractCaption == null
            ? null
            : v => v is not null ? extractCaption((T?)v) : extractCaption(default);

        return new FlowchartGraphGroup(info, fillStyleAction, extractItemIdFunc, extractCaptionFunc, skipDuringClean);
    }

    private FlowchartGraphGroup(DefinitionInfo info, Action<object?, FlowchartStyleClass>? fillStyle,
        Func<object?, string>? extractItemId, Func<object?, string?>? extractCaption, bool skipDuringClean)
    {
        Info = info ?? throw new ArgumentNullException(nameof(info));

        _fillStyle = fillStyle;
        SkipDuringClean = skipDuringClean;

        _extractItemId = extractItemId ??
                         (v => Info.ConvertToString(v) ??
                               throw new ApplicationException($"Item Id is null. Can't extract group item id for '{Info.Name}'"));

        _extractCaption = extractCaption ?? Info.ConvertToString;
    }

    public DefinitionInfo Info { get; }
    public bool SkipDuringClean { get; }

    public bool HasStyle => _fillStyle != null;

    public void FillStyle(object? value, FlowchartStyleClass styleClass)
    {
        _fillStyle?.Invoke(value, styleClass);
    }

    public string ExtractItemId(object? value)
    {
        return _extractItemId(value);
    }

    public string? ExtractCaption(object? value)
    {
        return _extractCaption(value);
    }
}