namespace CpuSchedulingAlgorithms;

public class RoundRobinScheduler : IProcessScheduler
{
    private readonly ArrivalSchedule schedule;

    private readonly Queue<ProcessControlBlock> readyQueue = new();

    private readonly List<CompletedProcess> completedProcesses = new();

    private bool HasNextProcess => readyQueue.Count > 0;

    /// <summary>
    /// Tracks the units of time elapsed under the current time quantum. 
    /// </summary>
    /// <remarks>
    /// When this value is the same as <see cref="TimeQuantum"/> or when the current process is switched, it is reset to 0.
    /// </remarks>
    private int timeQuantumOffset;

    public IReadOnlyList<CompletedProcess> CompletedProcesses => completedProcesses.AsReadOnly();

    public RoundRobinScheduler(ArrivalSchedule schedule, int timeQuantum = 1)
    {
        this.schedule = schedule;
        TimeQuantum = timeQuantum;
    }


    public ProcessTimeline Timeline { get; } = new();

    public ProcessControlBlock? CurrentProcess { get; private set; }

    public int Now { get; private set; }


    public int TimeQuantum { get; set; }

    /// <inheritdoc/>
    public bool Proceed()
    {
        // Return false because no more processes will arrive if:
        // * no current process exists(null) or if the current process is complete and
        // * no next process (ready queue is empty) and
        // * current time quantum is more than the schedule end time/maximum arrival time of all processes) 
        if (CurrentProcess is null or { IsComplete: true } && !HasNextProcess && Now > schedule.EndTime)
        {
            return false;
        }

        // Add processes arrived at current time(Now) to the ready queue
        if (Now <= schedule.EndTime)
        {
            foreach (var process in schedule.GetProcessesArrivedAt(Now))
            {
                readyQueue.Enqueue(new ProcessControlBlock(process, arrivalTime: Now));
            }
        }

        // Add CurrentProcess back to ready queue if:
        // it exists(not null) and
        // is not complete and
        // it is the end of the current time quantum
        if (CurrentProcess is { IsComplete: false } && timeQuantumOffset == TimeQuantum)
        {
            readyQueue.Enqueue(CurrentProcess);
        }

        // Get the next process in the queue if it exists if:
        // * CurrentProcess does not exist or
        // * CurrentProcess is complete or
        // * It is the end of the current time quantum 
        if (CurrentProcess is null or { IsComplete: true } || timeQuantumOffset == TimeQuantum)
        {
            CurrentProcess = HasNextProcess ? readyQueue.Dequeue() : null;
            // Reset time quantum offset since process has been switched
            timeQuantumOffset = 0;
        }

        // Execute the current process and increment to the next time quantum
        CurrentProcess?.Execute(Now);
        Now++;
        timeQuantumOffset++;

        // Add the process that consumed the current time quantum to the timeline 
        Timeline.Add(Now, CurrentProcess);

        // If process is complete, add process to the list of completed processes
        if (CurrentProcess is { IsComplete: true })
        {
            completedProcesses.Add(CompletedProcess.FromProcessControlBlock(CurrentProcess, Now));
        }
        return true;
    }
}