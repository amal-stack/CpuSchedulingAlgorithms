using static System.Console;

namespace CpuSchedulingAlgorithms.Console;

public static class OutputHelpers
{
    /// <summary>
    /// Creates a Gantt chart using the scheduler's <see cref="ProcessTimeline"/>.
    /// </summary>
    /// <param name="scheduler">The scheduler for which the Gantt chart has to be drawn.</param>
    public static void WriteGanttChart(IProcessScheduler scheduler)
    {
        WriteLine();
        Write("|");
        foreach (var (time, pcb) in scheduler.Timeline)
        {
            ForegroundColor = ConsoleColor.DarkCyan;
            Write($"{pcb?.Process.Id.ToString() ?? "<Idle>"}");
            ResetColor();
            ForegroundColor = ConsoleColor.Magenta;
            Write($"[{time}]");
            ResetColor();
            Write("|");
        }
        WriteLine();
    }

    /// <summary>
    /// Writes the current state of the process run queue using the <see cref="IProcessScheduler"/>'s <see cref="ProcessQueueView"/>
    /// </summary>
    /// <param name="scheduler">The scheduler whose run queue has to be output.</param>
    public static void WriteProcessQueue(IProcessScheduler scheduler)
    {
        foreach (var pcb in scheduler.RunQueue)
        {
            Write("[");
            ForegroundColor = ConsoleColor.DarkYellow;
            Write($"{pcb.Process.Id}|{pcb.TimeLeft}");
            ResetColor();
            Write("]");
        }
    }
}