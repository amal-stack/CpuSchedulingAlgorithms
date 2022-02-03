namespace TextTableCreator;

public class TableColumn<T>
{
    public string Name { get; set; }

    public int Width { get; set; }

    public Func<T, string> Selector { get; set; }

    public List<string> Cells { get; } = new();

    public string this[int index] => Cells[index];

    public TableColumn(string name, Func<T, string> selector)
    {
        Name = name;
        Selector = selector;
        Width = Name.Length;
    }

    public void Add(T item)
    {
        string value = Selector(item);
        if (value.Length > Width)
        {
            Width = value.Length;
        }
        Cells.Add(value);
    }
}
