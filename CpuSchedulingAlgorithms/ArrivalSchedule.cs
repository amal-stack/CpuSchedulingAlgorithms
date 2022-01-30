namespace CpuSchedulingAlgorithms;

/// <summary>
/// Simulates an arrival schedule by exposing a list of processes for a specified arrival time. 
/// </summary>
public class ArrivalSchedule : IEnumerable<KeyValuePair<int, IList<Process>>>
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

    public IEnumerator<KeyValuePair<int, IList<Process>>> GetEnumerator() => timeProcessesPair.GetEnumerator();

    public IEnumerable<Process> GetProcessesArrivedAt(int time)
    {
        if (timeProcessesPair.TryGetValue(time, out var processes))
        {
            return processes;
        }
        return Enumerable.Empty<Process>();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

