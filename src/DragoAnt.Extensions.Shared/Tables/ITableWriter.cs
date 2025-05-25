namespace Stenn.Shared.Tables
{
    public interface ITableWriter<in T>
    {
        void SetColumns(params TableWriterColumn[] columns);
        void WriteRow(T?[] values);
    }
}