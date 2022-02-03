namespace CpuSchedulingAlgorithms;

/// <summary>
/// Simulates an arrival schedule by exposing a list of processes for a specified arrival time. 
/// </summary>
public class ArrivalSchedule : IEnumerable<(int ArrivalTime, Process Process)>
{
    private readonly Dictionary<int, IList<Process>> timeProcessesPair = new();

    private int maxArrivalTime = 0;

    public int EndTime => maxArrivalTime;

    public IEnumerable<Process> this[int time] => GetProcessesArrivedAt(time);

    public void Add(int arrivalTime, Process process)
    {
        // Capture the arrival time of the process with the highest arrival time
        if (arrivalTime > maxArrivalTime)
        {
            maxArrivalTime = arrivalTime;
        }
        if (timeProcessesPair.TryGetValue(arrivalTime, out var processes))
        {
            processes.Add(process);
            return;
        }
        timeProcessesPair[arrivalTime] = new List<Process>() { process };
    }

    public void Add(int arrivalTime, IEnumerable<Process> processes)
    {
        if (arrivalTime > maxArrivalTime)
        {
            maxArrivalTime = arrivalTime;
        }
        if (timeProcessesPair.TryGetValue(arrivalTime, out var existingProcesses))
        {
            (existingProcesses as List<Process>)?.AddRange(processes);
            return;
        }
        timeProcessesPair[arrivalTime] = new List<Process>(processes);
    }

    /// <summary>
    ///  Returns the processes arrived at the arrival time <paramref name="time"/>.
    /// </summary>
    /// <param name="time">Arrival time</param>
    /// <returns>The processes arrived at the time. If no processes arrive, an empty <see cref="IEnumerable{Process}" />.</returns>
    public IEnumerable<Process> GetProcessesArrivedAt(int time)
    {
        if (timeProcessesPair.TryGetValue(time, out var processes))
        {
            return processes;
        }
        return Enumerable.Empty<Process>();
    }

    public IEnumerator<(int ArrivalTime, Process Process)> GetEnumerator()
    {
        for (int i = 0; i <= EndTime; i++)
        {
            foreach (var process in GetProcessesArrivedAt(i))
            {
                yield return (i, process);
            }
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

