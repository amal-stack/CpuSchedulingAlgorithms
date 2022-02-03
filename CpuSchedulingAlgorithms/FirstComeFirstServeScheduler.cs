using System.Collections.Immutable;

namespace CpuSchedulingAlgorithms;


public class FirstComeFirstServeScheduler : IProcessScheduler
{
    private readonly ArrivalSchedule schedule;

    private readonly Queue<ProcessControlBlock> readyQueue = new();

    private readonly List<CompletedProcess> completedProcesses = new();

    private bool HasNextProcess => readyQueue.Count > 0;

    private ProcessControlBlock GetNextProcess() => readyQueue.Dequeue();

    public IReadOnlyList<CompletedProcess> CompletedProcesses => completedProcesses.AsReadOnly();

    public int Now { get; private set; }

    public ProcessControlBlock? CurrentProcess { get; private set; }

    public ProcessTimeline Timeline { get; } = new();

    public FirstComeFirstServeScheduler(ArrivalSchedule schedule)
    {
        this.schedule = schedule;
    }

    /// <summary>
    /// Adds processes in the <see cref="schedule"/> to the ready queue that have arrived at the current time(<see cref="Now"/>).
    /// </summary>
    private void UpdateReadyQueue()
    {
        if (Now <= schedule.EndTime)
        {
            foreach (var process in schedule.GetProcessesArrivedAt(Now))
            {
                readyQueue.Enqueue(new ProcessControlBlock(process, arrivalTime: Now));
            }
        }
    }

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

        UpdateReadyQueue();

        // If no current process exists(idle) or if the current process is complete, get next process if exists
        if (CurrentProcess is null or { IsComplete: true })
        {
            CurrentProcess = HasNextProcess ? GetNextProcess() : null;
        }

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

