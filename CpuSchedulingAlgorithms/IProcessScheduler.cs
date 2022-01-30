using System.Collections.Immutable;

namespace CpuSchedulingAlgorithms;

public interface IProcessScheduler
{
    IReadOnlyList<CompletedProcess> CompletedProcesses { get; }
    
    //IImmutableQueue<ProcessControlBlock> ReadyQueue { get; }

    ProcessTimeline Timeline { get; }
    ProcessControlBlock? CurrentProcess { get; }
    int Now { get; }
    bool Proceed();
}

