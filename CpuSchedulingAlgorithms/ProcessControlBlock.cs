namespace CpuSchedulingAlgorithms;


/// <summary>
/// Used by schedulers to maintain the state of execution of processes.
/// </summary>
public class ProcessControlBlock
{
    public int TimeLeft { get; private set; }

    public Process Process { get; }

    public int ArrivalTime { get; }

    public int FirstCpuTime { get; private set; }

    public bool IsComplete => TimeLeft == 0;

    public ProcessControlBlock(Process process, int arrivalTime)
    {
        Process = process;
        ArrivalTime = arrivalTime;
        TimeLeft = process.BurstTime;
    }

    /// <summary>
    /// Simulates the execution of the process by decrementing the time left for completion by 1.
    /// </summary>
    /// <param name="time">The time quantum at which the process is being executed.</param>
    /// <exception cref="InvalidOperationException">Thrown when called after completion of execution.</exception>
    public void Execute(int time)
    {
        if (TimeLeft == Process.BurstTime)
        {
            FirstCpuTime = time;
        }
        if (IsComplete)
        {
            throw new InvalidOperationException("Cannot execute completed process");
        }
        TimeLeft--;
    }
}

