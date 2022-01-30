using System.Collections.Immutable;

namespace CpuSchedulingAlgorithms;

/// <summary>
/// Represents a conceptual CPU process with a predetermined burst time.  
/// </summary>

public record struct Process
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int BurstTime { get; set; }
}

public class ShortestRemainingTimeScheduler : IProcessScheduler
{
    private readonly ArrivalSchedule schedule;

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
        // If current process exists(not null) and is not complete, add it back to ready queue
        else if (CurrentProcess is not null)
        {
            readyQueue.Enqueue(CurrentProcess, CurrentProcess.TimeLeft);
        }

        return true;
    }
}


public class ShortestJobFirstScheduler : IProcessScheduler
{
    private readonly ArrivalSchedule schedule;

    private readonly PriorityQueue<ProcessControlBlock, int> readyQueue = new();

    private readonly List<CompletedProcess> completedProcesses = new();

    private bool HasNextProcess => readyQueue.Count > 0;

    public IReadOnlyList<CompletedProcess> CompletedProcesses => completedProcesses.AsReadOnly();

    //public IImmutableQueue<ProcessControlBlock> ReadyQueue => ImmutableQueue.CreateRange(readyQueue);

    public ProcessTimeline Timeline { get; } = new();

    public ProcessControlBlock? CurrentProcess { get; private set; }

    public int Now { get; private set; }

    public ShortestJobFirstScheduler(ArrivalSchedule schedule)
    {
        this.schedule = schedule;
    }

    public bool Proceed()
    {
        // Return false because no more processes will arrive if:
        // * no current process exists or if the current process is complete and
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

        // If no current process exists or if the current process is complete, get next process if exists
        if (CurrentProcess is null or { IsComplete: true })
        {
            CurrentProcess = HasNextProcess ? readyQueue.Dequeue() : null;
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


public class FirstComeFirstServeScheduler : IProcessScheduler
{
    private readonly ArrivalSchedule schedule;

    private readonly Queue<ProcessControlBlock> readyQueue = new();

    private readonly List<CompletedProcess> completedProcesses = new();

    private bool HasNextProcess => readyQueue.Count > 0;

    private ProcessControlBlock GetNextProcess() => readyQueue.Dequeue();

    public IReadOnlyList<CompletedProcess> CompletedProcesses => completedProcesses;

    public int Now { get; private set; }

    public ProcessControlBlock? CurrentProcess { get; private set; }

    public IImmutableQueue<ProcessControlBlock> ReadyQueue => ImmutableQueue.CreateRange(readyQueue);

    public ProcessTimeline Timeline { get; } = new();

    public FirstComeFirstServeScheduler(ArrivalSchedule schedule)
    {
        this.schedule = schedule;
    }

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

    public bool Proceed()
    {
        if (CurrentProcess is null or { IsComplete: true } && !HasNextProcess && Now > schedule.EndTime)
        {
            // CurrentProcess has completed execution, ready queue is empty, no more processes to arrive 
            return false;
        }

        UpdateReadyQueue();

        if (CurrentProcess is null or { IsComplete: true })
        {
            CurrentProcess = HasNextProcess ? GetNextProcess() : null;
        }

        CurrentProcess?.Execute(Now);
        Now++;
        Timeline.Add(Now, CurrentProcess);

        if (CurrentProcess is { IsComplete: true })
        {
            completedProcesses.Add(CompletedProcess.FromProcessControlBlock(CurrentProcess, Now));
        }

        return true;
    }
}

public class P
{
    public static ArrivalSchedule CreateSchedule() => new ArrivalSchedule
    {
        { 0, new Process() { Id = 0, Name = "Web Browser", BurstTime = 2 } },
        { 1, new Process() { Id = 1, Name = "Audio Service", BurstTime = 6 } },
        { 2, new Process() { Id = 2, Name = "Media Player", BurstTime = 4 } },
        { 3, new Process() { Id = 3, Name = "Game", BurstTime = 9 } },
        { 4, new Process() { Id = 4, Name = "Antivirus", BurstTime = 12 } }
    };

    public static void Start()
    {
        var arrivalSchedule = CreateSchedule();
        IProcessScheduler scheduler = new FirstComeFirstServeScheduler(arrivalSchedule);
        Console.WriteLine("Press any key to start");
        Console.ReadKey(false);
        while (scheduler.Proceed())
        {
            Console.WriteLine($"Current Time: {scheduler.Now}");
            Console.WriteLine($"Current Process: {scheduler.CurrentProcess.Process}");
            //Console.WriteLine($"Ready Queue: {string.Join(", ", scheduler.ReadyQueue.Select(q => q.Process))}");
            Console.ReadKey(false);
        }
        Console.WriteLine("Completed");
    }
}

