namespace CpuSchedulingAlgorithms;

/// <summary>
/// Maintains a record of the processes executed at each time quantum of the scheduler.
/// </summary>
public class ProcessTimeline : IEnumerable<KeyValuePair<int, ProcessControlBlock?>>
{
    public readonly Dictionary<int, ProcessControlBlock?> _timeline = new();

    /// <summary>
    /// Adds a process and time quantum pair to the timeline.
    /// </summary>
    /// <param name="time">The time quantum.</param>
    /// <param name="process">Process executed at the time quantum.</param>
    public void Add(int time, ProcessControlBlock? process)
    {
        _timeline.Add(time, process);
    }

    /// <summary>
    /// Returns the process executed at <paramref name="time"/>
    /// </summary>
    /// <param name="time">The time quantum of the scheduler.</param>
    /// <returns>The process as a <see cref="ProcessControlBlock"/> object.</returns>
    public ProcessControlBlock? GetProcessExecutedAt(int time) => _timeline[time];


    public ProcessControlBlock? this[int time] => GetProcessExecutedAt(time);

    public IEnumerator<KeyValuePair<int, ProcessControlBlock?>> GetEnumerator() => _timeline.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

