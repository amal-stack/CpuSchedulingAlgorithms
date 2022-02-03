namespace TextTableCreator;

public class Table<T>
{
    public IReadOnlyList<TableColumn<T>> Columns { get; }

    public TableOptions Options { get; }

    public int Count { get; }

    public Table(IReadOnlyList<TableColumn<T>> columns, TableOptions options, int count)
    {
        Columns = columns;
        Options = options;
        Count = count;
    }
}
