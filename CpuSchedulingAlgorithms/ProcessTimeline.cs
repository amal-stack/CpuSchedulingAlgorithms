namespace CpuSchedulingAlgorithms;

public class ProcessTimeline : IEnumerable<KeyValuePair<int, ProcessControlBlock?>>
{
    public readonly Dictionary<int, ProcessControlBlock?> _timeline = new();

    public void Add(int time, ProcessControlBlock? process)
    {
        _timeline.Add(time, process);
    }

    public ProcessControlBlock? GetProcessExecutedAt(int time) => _timeline[time];

    public ProcessControlBlock? this[int time] => GetProcessExecutedAt(time);

    public IEnumerator<KeyValuePair<int, ProcessControlBlock?>> GetEnumerator() => _timeline.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

