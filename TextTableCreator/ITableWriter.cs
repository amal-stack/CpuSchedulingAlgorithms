namespace TextTableCreator;

public interface ITableWriter<T>
{
    Table<T> Table { get; }

    void Write();
}


