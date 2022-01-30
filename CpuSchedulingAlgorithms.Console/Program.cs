using CpuSchedulingAlgorithms;
using System.Text;

using static System.Console;

const string ProcessId = "Process ID";
const string ArrivalTime = "Arrival Time";
const string BurstTime = "Burst Time";
const string TurnaroundTime = "Turnaround Time";
const string WaitTime = "Wait Time";
const string CompletionTime = "Completion Time";
const int Alignment = -15;
int processCount, maxArrivalTime = 10, maxBurstTime = 10;

var scheduleArray = new List<(int Id, int ArrivalTime, int BurstTime)> 
{ 
    (1, 0, 18), 
    (2, 1, 4), 
    (3, 2, 7), 
    (4, 3, 2) 
};

WriteTitle("FIRST COME FIRST SERVE SCHEDULING");

Console.Write("Enter number of processes: ");
if (!int.TryParse(Console.ReadLine(), out processCount))
{
    processCount = 5;
    Console.WriteLine("Invalid input. Using default value of 5.");
}
//var arrivalSchedule = P.CreateSchedule();
//var arrivalSchedule = ArrivalScheduleGenerator.GenerateRandomArrivalSchedule(processCount, maxArrivalTime, maxBurstTime);
//var arrivalSchedule = new ArrivalSchedule() { { 0, new Process { BurstTime = 1, Id = 100 } }, { 2, new Process { Id = 42, BurstTime = 1 } } };
var arrivalSchedule = new ArrivalSchedule();
//scheduleArray.ForEach(e => arrivalSchedule.Add(e.ArrivalTime, new Process { Id = e.Id, BurstTime = e.BurstTime }));
WriteTitle("Generated Processes");

Console.WriteLine($"{GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)}");
Console.WriteLine($"{ProcessId,Alignment} | {ArrivalTime,Alignment} | {BurstTime,Alignment}");
Console.WriteLine($"{GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)}");

foreach (var (arrivalTime, processes) in arrivalSchedule)
{
    foreach (var process in processes)
    {
        Console.WriteLine($"{process.Id,Alignment} | {arrivalTime,Alignment} | {process.BurstTime,Alignment}");
    }
}

IProcessScheduler scheduler = new ShortestRemainingTimeScheduler(arrivalSchedule);
Console.WriteLine();
Console.WriteLine("Press any key to start simulator...");
Console.ReadKey(false);
while (scheduler.Proceed())
{
    Console.WriteLine();
    Console.WriteLine($"Current Time Quantum: {scheduler.Now}");
    Console.WriteLine($"Current Process: {scheduler.CurrentProcess?.Process.Id.ToString() ?? "None"}");

    if (scheduler.CompletedProcesses.Count is > 0 and int count)
    {
        string completedProcessesString = string.Join(", ", scheduler.CompletedProcesses.Select(p => p.Process.Id.ToString()));
        WriteGreenLine($"Completed Processes: {count} [{completedProcessesString}]");
    }
    Console.WriteLine($"Gantt Chart:");
    Console.WriteLine(CreateGanttChart(scheduler));
    Console.WriteLine("Press any key to continue");
    Console.WriteLine();
    Console.WriteLine(GetLine());
    Console.ReadKey(false);
}

Console.WriteLine();
WriteTitle("Completed. Statistics:");

WriteStatisticsTable(scheduler.CompletedProcesses);

WriteGreenLine($"Average Turnaround Time: {scheduler.CompletedProcesses.Average(p => p.TurnaroundTime)}");
WriteGreenLine($"Average Wait Time: {scheduler.CompletedProcesses.Average(p => p.WaitTime)}");

Console.WriteLine();

string CreateGanttChart(IProcessScheduler scheduler)
{
    StringBuilder builder = new();
    builder.AppendLine();
    builder.Append(" | ");
    foreach (var (time, pcb) in scheduler.Timeline)
    {
        builder.Append($"{pcb?.Process.Id.ToString() ?? "Idle"} [{time}] | ");
    }
    builder.AppendLine();
    return builder.ToString();
}


void WriteGreenLine(string value)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(value);
    Console.ResetColor();
}

void WriteTitle(string value)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine();
    Console.WriteLine(GetLine());
    Console.WriteLine(value);
    Console.WriteLine(GetLine());
    Console.WriteLine();
    Console.ResetColor();
}

string GetLine(int length = 100, char character = '=')
{
    return new string(character, Math.Abs(length));
}

void WriteStatisticsTable(IReadOnlyList<CompletedProcess> completedProcesses)
{
    Console.WriteLine($"{GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)}");
    Console.WriteLine($"{ProcessId,Alignment} | {ArrivalTime,Alignment} | {BurstTime,Alignment} | {CompletionTime,Alignment} | {TurnaroundTime,Alignment} | {WaitTime,Alignment}");
    Console.WriteLine($"{GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)} | {GetLine(Alignment)}");


    foreach (var process in scheduler.CompletedProcesses)
    {
        Console.WriteLine($"{process.Process.Id,Alignment} | {process.ArrivalTime,Alignment} | {process.Process.BurstTime,Alignment} | {process.CompletionTime,Alignment} | {process.TurnaroundTime,Alignment} | {process.WaitTime,Alignment}");
    }


    Console.WriteLine();
}
//string CreateTable()
//{
//    var x = Enumerable.Repeat(new Process(), 10);
//    new TableBuilder<Process>(x)
//        .AddColumn("ID", process => process.Id.ToString())
//        .AddColumn("Name", process => process.Name);
//}

//class TableBuilder<TModel>
//{
//    public IEnumerable<TModel> Model { get; set; }
//    private IList<RowConfig> Rows { get; set; } = new List<RowConfig>();

//    public TableBuilder(IEnumerable<TModel> model)
//    {
//        Model = model;
//    }

//    public TableBuilder<TModel> AddColumn(string name, Func<TModel, string> valueSelector)
//    {
//        Rows.Add(new RowConfig(name, valueSelector));
//        return this;
//    }

//    public TableBuilder<TModel> AddAggregateRow(stri)

//    private record RowConfig(string Name, Func<TModel, string> ValueSelector);
//}




