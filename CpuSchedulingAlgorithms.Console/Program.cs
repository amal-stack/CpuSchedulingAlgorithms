using CpuSchedulingAlgorithms;
using CpuSchedulingAlgorithms.Console;
using TextTableCreator;
using static System.Console;

// Display title
ForegroundColor = ConsoleColor.Green;
WriteLine("CPU Scheduling Algorithms".ToUpper());
ResetColor();

WriteLine(new string('-', 100));
WriteLine();
WriteLine();

// Input scheduling algorithm
WriteLine("Please enter the preferred scheduling algorithm");
ForegroundColor = ConsoleColor.DarkCyan;
WriteLine("1. First Come First Serve (FCFS)");
WriteLine("2. Shortest Job First");
WriteLine("3. Shortest Remaining Time First");
WriteLine("4. Round Robin");
WriteLine("5. Priority(Non-preemptive)");
ResetColor();
Write("> ");

int algo = InputHelpers.ReadInteger();

// Read time quantum if algo is Round-robin(4)
int timeQuantum = 1;
if (algo == 4)
{
    Write("Enter time quantum > ");
    timeQuantum = InputHelpers.ReadInteger();
}


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
WriteLine("1. Input processes manually (one by one)");
WriteLine("2. Input processes manually (space-separated on single line)");
WriteLine("3. Generate random processes");
ResetColor();
Write("> ");
int choice = InputHelpers.ReadInteger();
ArrivalSchedule schedule = choice switch
{
    1 => InputHelpers.ReadProcesses(processCount, readPriority: algo == 5),
    2 => InputHelpers.ReadProcessesSingleLine(readPriority: algo == 5),
    3 => ArrivalScheduleGenerator.GenerateRandomArrivalSchedule(processCount),
    _ => throw new InvalidDataException("Invalid input")
};

// Display input processes
WriteLine();
ForegroundColor = ConsoleColor.Green;
WriteLine("Input Processes");
ResetColor();
WriteLine(new string('-', 100));
WriteLine();

var inputTableBuilder = TableBuilder
    .For(schedule.OrderBy(i => i.Process.Id))
    .AddColumn("Process ID", p => p.Process.Id)
    .AddColumn("Arrival Time", p => p.ArrivalTime)
    .AddColumn("Burst Time", p => p.Process.BurstTime);

if (algo == 5)
{
    inputTableBuilder.AddColumn("Priority", p => p.Process.Priority);
}

inputTableBuilder
    .Build()
    .WriteToConsole(c => c.HeaderColor = ConsoleColor.DarkCyan);

WriteLine();


// Create scheduler based on input
IProcessScheduler scheduler = algo switch
{
    5 => new PriorityScheduler(schedule),
    4 => new RoundRobinScheduler(schedule, timeQuantum),
    3 => new ShortestRemainingTimeScheduler(schedule),
    2 => new ShortestJobFirstScheduler(schedule),
    _ => new FirstComeFirstServeScheduler(schedule),
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
    Write($"Ready Queue[Process ID | Time Left]: ");
    OutputHelpers.WriteProcessQueue(scheduler);
    WriteLine();
    WriteLine();

    ForegroundColor = ConsoleColor.Green;
    WriteLine("Gantt Chart");
    ResetColor();
    WriteLine(new string('-', 50));

    WriteLine();
    OutputHelpers.WriteGanttChart(scheduler);

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

var statisticsTableBuilder = TableBuilder
    .For(scheduler.CompletedProcesses.OrderBy(p => p.Process.Id))
    .AddColumn("Process Id", p => p.Process.Id)
    .AddColumn("Arrival", p => p.ArrivalTime)
    .AddColumn("Burst", p => p.Process.BurstTime);

if (algo == 5)
{
    statisticsTableBuilder.AddColumn("Priority", p => p.Process.Priority);
}

statisticsTableBuilder
    .AddColumn("Completion", p => p.CompletionTime)
    .AddColumn("Turnaround", p => p.TurnaroundTime)
    .AddColumn("Wait", p => p.WaitTime)
    .AddColumn("Response", p => p.ResponseTime)
    .Build()
    .WriteToConsole(c => c.HeaderColor = ConsoleColor.DarkCyan);

ForegroundColor = ConsoleColor.DarkCyan;
WriteLine();
WriteLine($"Average Turnaround Time: {scheduler.CompletedProcesses.DefaultIfEmpty().Average(p => p.TurnaroundTime)}");
WriteLine($"Average Wait Time: {scheduler.CompletedProcesses.DefaultIfEmpty().Average(p => p.WaitTime)}");
ResetColor();