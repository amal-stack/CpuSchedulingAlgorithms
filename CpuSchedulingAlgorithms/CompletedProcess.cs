namespace CpuSchedulingAlgorithms;

public struct CompletedProcess
{
    public Process Process { get; init; }

    public int ArrivalTime { get; init; }

    public int CompletionTime { get; init; }

    public int TurnaroundTime => CompletionTime - ArrivalTime;

    public int WaitTime => TurnaroundTime - Process.BurstTime;

    public static CompletedProcess FromProcessControlBlock(ProcessControlBlock pcb, int completionTime)
        => pcb.IsComplete
            ? new CompletedProcess
            {
                Process = pcb.Process,
                ArrivalTime = pcb.ArrivalTime,
                CompletionTime = completionTime
            }
            : throw new InvalidOperationException("Process is incomplete");
}

