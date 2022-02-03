namespace TextTableCreator;

public class TableOptions
{
    public char DividerCharacter { get; set; } = '|';

    public char RuleCharacter { get; set; } = '-';

    public char RuleDividerCharacter { get; set; } = '+';

    public TableRule Rule { get; set; } = TableRule.All;

    public TableAlignment TableAlignment { get; set; } = TableAlignment.Right;
}

[Flags]
public enum TableRule
{
    None = 0,
    HeaderTop = 1,
    HeaderBottom = 1 << 2,
    Header = HeaderTop | HeaderBottom,
    Footer = 1 << 3,
    All = Header | Footer,
}

public enum TableAlignment
{
    Left,
    Right
}
