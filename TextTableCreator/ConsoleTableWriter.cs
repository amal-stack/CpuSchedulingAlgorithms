using static System.Console;

namespace TextTableCreator;


public class ConsoleTableWriter<T> : ITableWriter<T>
{
    private readonly string _divider;

    public Table<T> Table { get; }

    public TextWriter Writer { get; }

    public ConsoleColorOptions ColorOptions { get; }


    public ConsoleTableWriter(
        Table<T> table,
        ConsoleColorOptions? options = null,
        TextWriter? writer = null)
    {
        Table = table;
        ColorOptions = options ?? new();
        Writer = writer ?? Out;
        _divider = $" {Table.Options.DividerCharacter} ";
    }

    public void Write()
    {
        if (Table.Options.Rule.HasFlag(TableRule.HeaderTop))
        {
            WriteRuleString();
        }

        WriteHeaderString();

        if (Table.Options.Rule.HasFlag(TableRule.HeaderBottom))
        {
            WriteRuleString();
        }

        for (int i = 0; i < Table.Count; i++)
        {
            WriteRowStringForIndex(i);
        }

        if (Table.Options.Rule.HasFlag(TableRule.Footer))
        {
            WriteRuleString();
        }

        ResetColor();
    }

    public void ConfigureColorOptions(Action<ConsoleColorOptions> action)
    {
        action?.Invoke(ColorOptions);
    }

    public class ConsoleColorOptions
    {
        public ConsoleColor RuleColor { get; set; }

        public ConsoleColor RuleDividerColor { get; set; }

        public ConsoleColor DividerColor { get; set; }

        public ConsoleColor HeaderColor { get; set; }

        public ConsoleColor ContentColor { get; set; }

        public ConsoleColorOptions()
        {
            ContentColor
                = DividerColor
                = HeaderColor
                = RuleColor
                = RuleDividerColor
                = ConsoleColor.Gray;
        }
    }

    protected void WriteRowStringForIndex(int index)
    {
        ForegroundColor = ColorOptions.DividerColor;
        Writer.Write(_divider);
        foreach (var column in Table.Columns)
        {
            string alignedCell = Align(column[index], column.Width);

            ForegroundColor = ColorOptions.ContentColor;
            Writer.Write(alignedCell);

            ForegroundColor = ColorOptions.DividerColor;
            Writer.Write(_divider);
        }
        Writer.WriteLine();
    }

    protected string Align(string value, int width)
        => Table.Options.TableAlignment switch
        {
            TableAlignment.Left => value.PadRight(width),
            _ => value.PadLeft(width)
        };

    protected void WriteHeaderString()
    {
        ForegroundColor = ColorOptions.DividerColor;
        Writer.Write(_divider);

        foreach (var column in Table.Columns)
        {
            ForegroundColor = ColorOptions.HeaderColor;
            Writer.Write(Align(column.Name, column.Width));

            ForegroundColor = ColorOptions.DividerColor;
            Writer.Write(_divider);
        }
        Writer.WriteLine();
    }

    protected void WriteRuleString()
    {
        ForegroundColor = ColorOptions.RuleDividerColor;
        Writer.Write($" {Table.Options.RuleDividerCharacter}");

        for (int i = 0; i < Table.Columns.Count; i++)
        {
            ForegroundColor = ColorOptions.RuleColor;
            Writer.Write(new string(Table.Options.RuleCharacter, Table.Columns[i].Width + 2));

            ForegroundColor = ColorOptions.RuleDividerColor;
            Writer.Write(Table.Options.RuleDividerCharacter);
        }

        Writer.WriteLine();
    }
}
