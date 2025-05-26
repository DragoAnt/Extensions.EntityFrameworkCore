namespace DragoAnt.EntityFrameworkCore.EntityConventions;

public static class ConventionsAnnotationNames
{
    /// <summary>
    /// Set default as <see cref="DateTime.Now"/> for specific provider
    /// </summary>
    public const string SqlDefault_CurrentDateTime = "SqlDefault_CurrentDateTime";
    /// <summary>
    ///  Set current default as update trigger action
    /// </summary>
    public const string ColumnTriggerUpdate_SqlDefault = "ColumnTriggerUpdate_Default";
        
    /// <summary>
    ///  Set expression as insert trigger action
    /// </summary>
    public const string ColumnTriggerInsert = "ColumnTriggerInsert";
    /// <summary>
    ///  Set expression as update trigger action
    /// </summary>
    public const string ColumnTriggerUpdate = "ColumnTriggerUpdate";
        
    /// <summary>
    /// Use column as <see cref="DateTime"/> column of soft delete in 'instead of delete' trigger.
    /// </summary>
    public const string SoftDelete = "ColumnTriggerSoftDelete";
}