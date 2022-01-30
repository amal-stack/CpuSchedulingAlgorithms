namespace CpuSchedulingAlgorithms;

public class ProcessControlBlock
{
    public int TimeLeft { get; private set; }

    public Process Process { get; }

    public int ArrivalTime { get; }

    public int ResponseTime { get; private set; }

    public bool IsComplete => TimeLeft == 0;

    public ProcessControlBlock(Process process, int arrivalTime)
    {
        Process = process;
        ArrivalTime = arrivalTime;
        TimeLeft = process.BurstTime;
    }

    public void Execute(int time)
    {
        if (TimeLeft == Process.BurstTime)
        {
            ResponseTime = time;
        }
        if (IsComplete)
        {
            throw new InvalidOperationException("Cannot execute completed process");
        }
        TimeLeft--;
    }
}

