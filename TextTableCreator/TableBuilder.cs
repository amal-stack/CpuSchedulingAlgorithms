using System.Text;

namespace TextTableCreator;

public class TableBuilder
{
    public static TableBuilder<T> For<T>(IEnumerable<T> elements)
    {
        return new TableBuilder<T>().For(elements);
    }

    public static TableBuilder<T> ForSingle<T>(T element)
    {
        return new TableBuilder<T>().For(new[] { element });
    }
}

public class TableBuilder<T>
{
    private IEnumerable<T> _elements = Enumerable.Empty<T>();

    private List<TableColumn<T>> _columns = new();

    private TableOptions _options = new();

    public TableBuilder<T> For(IEnumerable<T> items)
    {
        _elements = items;
        return this;
    }

    public TableBuilder<T> AddColumn<TColumn>(string name, Func<T, TColumn> selector)
    {
        _columns.Add(new TableColumn<T>(
            name,  
            value => selector?.Invoke(value)?.ToString()));
        return this;
    }

    public TableBuilder<T> AddColumn(TableColumn<T> column)
    {
        _columns.Add(column);
        return this;
    }

    public TableBuilder<T> Configure(Action<TableOptions> action)
    {
        action(_options);
        return this;
    }

    public TableBuilder<T> UseOptions(TableOptions options)
    {
        _options = options;
        return this;
    }

    public Table<T> Build()
    {
        int count = 0;
        foreach (var item in _elements)
        {
            foreach (var column in _columns)
            {
                column.Add(item);
            }
            count++;
        }
        return new Table<T>(_columns, _options, count);
    }

    public override string ToString()
    {
        StringBuilder builder = new();

        const string divider = " | ";


        builder.AppendLine();

        builder.Append(" + ");
        foreach (var column in _columns)
        {
            builder.Append(new string('=', column.Width));
            builder.Append("-+-");
        }
        builder.AppendLine();

        // Add Column Headers
        builder.Append(divider);
        foreach (var column in _columns)
        {
            builder.Append(column.Name.PadLeft(column.Width));
            builder.Append(divider);
        }
        builder.AppendLine();

        builder.Append(" + ");
        foreach (var column in _columns)
        {
            builder.Append(new string('=', column.Width));
            builder.Append("-+-");
        }
        builder.AppendLine();


        for (int i = 0; i < _columns.Count; i++)
        {
            builder.Append(divider);
            foreach (var column in _columns)
            {
                builder.Append(column[i].PadLeft(column.Width));
                builder.Append(divider);
            }
            builder.AppendLine();
        }
        builder.AppendLine();
        return builder.ToString();
    }
}
