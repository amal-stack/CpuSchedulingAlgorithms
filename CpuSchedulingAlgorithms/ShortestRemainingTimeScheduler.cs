namespace CpuSchedulingAlgorithms;

/// <summary>
/// Preemptive version of the shortest job first scheduling algorithm.
/// </summary>
public class ShortestRemainingTimeScheduler : IProcessScheduler
{
    private readonly ArrivalSchedule schedule;

    /// <summary>
    /// Priority queue with burst time as priority
    /// </summary>
    private readonly PriorityQueue<ProcessControlBlock, int> readyQueue = new();

    private readonly List<CompletedProcess> completedProcesses = new();

    private bool HasNextProcess => readyQueue.Count > 0;

    public IReadOnlyList<CompletedProcess> CompletedProcesses => completedProcesses.AsReadOnly();

    public ShortestRemainingTimeScheduler(ArrivalSchedule schedule)
    {
        this.schedule = schedule;
    }

    public ProcessTimeline Timeline { get; } = new();

    public ProcessControlBlock? CurrentProcess { get; private set; }

    public int Now { get; private set; }

    public ProcessQueueView RunQueue => new(readyQueue.UnorderedItems.Select(i => i.Element));

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
                readyQueue.Enqueue(new ProcessControlBlock(process, arrivalTime: Now), process.BurstTime);
            }
        }

        // If current process exists(not null) and is not complete, add it back to ready queue
        if (CurrentProcess is { IsComplete: false })
        {
            readyQueue.Enqueue(CurrentProcess, CurrentProcess.TimeLeft);
        }

        // Get the process with the shortest remaining time
        CurrentProcess = HasNextProcess ? readyQueue.Dequeue() : null;

        // Execute the current process and increment to the next time quantum
        CurrentProcess?.Execute(Now);
        Now++;

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
