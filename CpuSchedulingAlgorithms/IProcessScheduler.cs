namespace CpuSchedulingAlgorithms;

/// <summary>
/// Describes the members of a process scheduler.
/// </summary>
public interface IProcessScheduler
{
    ProcessControlBlock? CurrentProcess { get; }

    /// <summary>
    /// The current time quantum.
    /// </summary>
    int Now { get; }

    /// <summary>
    /// Move to the next time quantum.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the scheduler has completed execution of all processes in the <see cref="ArrivalSchedule"/> else <see langword="false"/>.
    /// </returns>
    bool Proceed();

    /// <summary>
    /// A read-only list of processes that has completed execution that the scheuler maintains.
    /// </summary>
    IReadOnlyList<CompletedProcess> CompletedProcesses { get; }

    ProcessTimeline Timeline { get; }

    ProcessQueueView RunQueue { get; }
}

