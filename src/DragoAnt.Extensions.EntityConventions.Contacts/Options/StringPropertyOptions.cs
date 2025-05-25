using System.ComponentModel;

namespace DragoAnt.Extensions.EntityConventions.Contacts;

public abstract class StringPropertyOptions : BasePropertyOptions
{
    /// <summary>
    /// Fixed length if string. Default:false 
    /// </summary>
    [DefaultValue(false)]
    public bool FixedLength { get; set; } = false;

    /// <summary>
    /// Max length if string. Default:50 
    /// </summary>
    [DefaultValue(50)]
    public int MaxLength { get; set; } = 50;

    /// <summary>
    /// Is unicode if string. Default:false 
    /// </summary>
    [DefaultValue(false)]
    public bool IsUnicode { get; set; }
}