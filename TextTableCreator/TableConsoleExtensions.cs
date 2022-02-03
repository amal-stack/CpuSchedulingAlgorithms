namespace TextTableCreator;

public static class TableConsoleExtensions
{
    public static void WriteToConsole<T>(
        this Table<T> table,
        Action<ConsoleTableWriter<T>.ConsoleColorOptions>? action = null,
        TextWriter? writer = null)
    {
        ConsoleTableWriter<T>.ConsoleColorOptions options = new();
        action?.Invoke(options);
        new ConsoleTableWriter<T>(table, options, writer).Write();
    }
}

internal static class TableOptionPresets
{
    public static TableOptions Defaults => new();

    public static TableOptions Markdown => new()
    {
        DividerCharacter = '|',
        RuleCharacter = '-',
        RuleDividerCharacter = '|',
        Rule = TableRule.HeaderBottom
    };
}

public static class TableOptionsExtensions
{
    public static TableBuilder<T> UseMarkdownOptions<T>(this TableBuilder<T> builder)
    {
        builder.UseOptions(TableOptionPresets.Markdown);
        return builder;
    }
}