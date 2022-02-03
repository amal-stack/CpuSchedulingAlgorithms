using CpuSchedulingAlgorithms;
using TextTableCreator;
using static System.Console;

// Display title
ForegroundColor = ConsoleColor.DarkCyan;
WriteLine("CPU Scheduling Algorithms".ToUpper());
WriteLine();
WriteLine();
ResetColor();

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
ForegroundColor = ConsoleColor.DarkCyan;
WriteLine("Please enter your choice");
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
WriteLine("Input Processes");
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
ForegroundColor = ConsoleColor.DarkCyan;
WriteLine("Please enter the preferred scheduling algorithm");
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

// Continue until no more processes left
while (scheduler.Proceed())
{
    WriteLine();
    TableBuilder
        .ForSingle(scheduler)
        .AddColumn("Current Time Quantum", s => s.Now.ToString())
        .AddColumn("Completed Processes",
            s => string.Join(",", from p in s.CompletedProcesses
                                  select p.Process.Id))
        .AddColumn("Current Process", s => s.CurrentProcess?.Process.Id.ToString() ?? "Idle")
        .Configure(o => o.Rule = TableRule.None)
        .Build()
        .WriteToConsole(c => c.HeaderColor = ConsoleColor.DarkCyan);

    WriteLine(CreateGanttChart(scheduler));

    WriteLine();
    WriteLine();

    // Wait for key press before proceeding to the next time quantum
    ReadKey(intercept: false);
}

// Display statistics table after completion
ForegroundColor = ConsoleColor.DarkGreen;
WriteLine("Completed. Statistics:");
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

ForegroundColor = ConsoleColor.DarkGreen;
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
        WriteLine($"Process {i + 1}");

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
        builder.Append($"{pcb?.Process.Id.ToString() ?? "Idle"} [{time}] | ");
    }
    builder.AppendLine();
    return builder.ToString();
}

