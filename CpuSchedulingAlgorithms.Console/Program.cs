using CpuSchedulingAlgorithms;
using TextTableCreator;
using static System.Console;

// Display title
ForegroundColor = ConsoleColor.Green;
WriteLine("CPU Scheduling Algorithms".ToUpper());
ResetColor();

WriteLine(new string('-', 100));
WriteLine();
WriteLine();

// Input number of processes
Write("Enter number of processes > ");
if (!int.TryParse(ReadLine(), out int processCount) || processCount <= 0)
{
    ForegroundColor = ConsoleColor.DarkYellow;
    WriteLine("Invalid input. Using default process count of 5");
    ResetColor();

    processCount = 5;
}
WriteLine();

// Input process source
WriteLine("Please enter your choice");
ForegroundColor = ConsoleColor.DarkCyan;
WriteLine("1. Input processes manually");
WriteLine("2. Generate random processes");
ResetColor();
Write("> ");
int choice = ReadInteger();
ArrivalSchedule schedule = choice switch
{
    1 => ReadProcesses(processCount),
    2 => ArrivalScheduleGenerator.GenerateRandomArrivalSchedule(processCount),
    _ => throw new ArgumentOutOfRangeException("Invalid input")
};


// Display input processes
WriteLine();
ForegroundColor = ConsoleColor.Green;
WriteLine("Input Processes");
ResetColor();
WriteLine(new string('-', 100));
WriteLine();

TableBuilder
    .For(schedule)
    .AddColumn("Process ID", p => p.Process.Id.ToString())
    .AddColumn("Arrival Time", p => p.ArrivalTime.ToString())
    .AddColumn("Burst Time", p => p.Process.BurstTime.ToString())
    .Build()
    .WriteToConsole(c => c.HeaderColor = ConsoleColor.DarkCyan);

WriteLine();

// Input scheduling algorithm

WriteLine("Please enter the preferred scheduling algorithm");
ForegroundColor = ConsoleColor.DarkCyan;
WriteLine("1. First Come First Serve (FCFS)");
WriteLine("2. Shortest Job First");
WriteLine("3. Shortest Remaining Time First");
ResetColor();
Write("> ");
int algo = ReadInteger();

// Create scheduler based on input
IProcessScheduler scheduler = algo switch
{
    1 => new FirstComeFirstServeScheduler(schedule),
    2 => new ShortestJobFirstScheduler(schedule),
    _ => new ShortestRemainingTimeScheduler(schedule)
};

// Wait for key press to start
WriteLine("Press any key to start simulator...");
ReadKey(intercept: false);

// Flag that indicates whether to wait after each time quantum;
bool wait = true;

// Continue until no more processes left
while (scheduler.Proceed())
{
    WriteLine();
    ForegroundColor = ConsoleColor.Green;
    WriteLine("Current State");
    ResetColor();
    WriteLine(new string('-', 50));

    WriteLine();
    WriteLine($"Current Time Quantum: {scheduler.Now}");
    WriteLine($"Current Process: {scheduler.CurrentProcess?.Process.Id.ToString() ?? "<Idle>"}");

    string completedProcessesString = string.Join(",",
        from p in scheduler.CompletedProcesses
        select p.Process.Id);

    WriteLine($"Completed Processes: {completedProcessesString}");

    WriteLine();
    ForegroundColor = ConsoleColor.Green;
    WriteLine("Gantt Chart");
    ResetColor();
    WriteLine(new string('-', 50));
    
    WriteLine();
    WriteLine(CreateGanttChart(scheduler));

    WriteLine();
    WriteLine(new string('=', 100));
    WriteLine();

    WriteLine("Press X to jump to statistics.");
    WriteLine("Press any other key to continue");

    

    // Wait for key press before proceeding to the next time quantum
    if (wait)
    {
        var key = ReadKey(intercept: false);
        if (key.KeyChar is 'x' or 'X')
        {
            wait = false;
        }
    }
}

// Display statistics table after completion
ForegroundColor = ConsoleColor.Green;
WriteLine();
WriteLine("Completed. Statistics:".ToUpper());
WriteLine(new string('-', 100));
WriteLine();
ResetColor();

TableBuilder
    .For(scheduler.CompletedProcesses)
    .AddColumn("Process Id", p => p.Process.Id.ToString())
    .AddColumn("Arrival Time", p => p.ArrivalTime.ToString())
    .AddColumn("Burst Time", p => p.Process.BurstTime.ToString())
    .AddColumn("Completion Time", p => p.CompletionTime.ToString())
    .AddColumn("Turnaround Time", p => p.TurnaroundTime.ToString())
    .AddColumn("Wait Time", p => p.WaitTime.ToString())
    .AddColumn("", p => p.ResponseTime.ToString())
    .Build()
    .WriteToConsole(c => c.HeaderColor = ConsoleColor.DarkCyan);

ForegroundColor = ConsoleColor.White;
WriteLine();
WriteLine($"Average Turnaround Time: {scheduler.CompletedProcesses.Average(p => p.TurnaroundTime)}");
WriteLine($"Average Wait Time: {scheduler.CompletedProcesses.Average(p => p.WaitTime)}");
ResetColor();

/// <summary>
/// Keeps reading input until not a valid integer.
/// </summary>
static int ReadInteger()
{
    int value;
    while (!int.TryParse(ReadLine(), out value))
    {
        ForegroundColor = ConsoleColor.Red;
        WriteLine("Invalid input. Try again");
        ResetColor();
    }
    return value;
}

/// <summary>
/// Reads the details of the specified number of processes from the console.
/// </summary>
static ArrivalSchedule ReadProcesses(int count)
{
    ArrivalSchedule schedule = new();

    for (int i = 0; i < count; i++)
    {
        ForegroundColor = ConsoleColor.DarkCyan;
        WriteLine($"Process {i + 1}");
        ResetColor();

        Write("... Enter process ID: ");
        int processId = ReadInteger();

        Write("... Enter arrival time: ");
        int arrivalTime = ReadInteger();

        Write("... Enter burst time: ");
        int burstTime = ReadInteger();

        WriteLine();

        schedule.Add(
            arrivalTime,
            new Process { Id = processId, BurstTime = burstTime });
    }
    return schedule;
}


/// <summary>
/// Creates a Gantt chart using the scheduler's <see cref="ProcessTimeline"/>.
/// </summary>
static string CreateGanttChart(IProcessScheduler scheduler)
{
    System.Text.StringBuilder builder = new();
    builder.AppendLine();
    builder.Append(" | ");
    foreach (var (time, pcb) in scheduler.Timeline)
    {
        builder.Append($"{pcb?.Process.Id.ToString() ?? "<Idle>"} [{time}] | ");
    }
    builder.AppendLine();
    return builder.ToString();
}

