using System.ComponentModel;

namespace DragoAnt.EntityConventions.Contacts;

public abstract class BasePropertyOptions : Attribute
{
    /// <summary>
    /// Column's name in database. Default: null means 'use property name as column name'
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// Has index. Default:true 
    /// </summary>
    [DefaultValue(true)]
    public bool HasIndex { get; set; } = true;
    /// <summary>
    /// Is index unique
    /// </summary>
    public bool IndexIsUnique { get; set; }
}