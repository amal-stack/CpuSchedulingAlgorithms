using static System.Console;

namespace CpuSchedulingAlgorithms.Console;

public static class InputHelpers
{
    /// <summary>
    /// Reads input from the console until a valid integer is entered.
    /// </summary>
    /// <returns>The input parsed as an <see langword="int"/>.</returns>
    public static int ReadInteger()
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
    /// Reads and parses string of space-separated integers from the console
    /// </summary>
    /// <returns>The parsed values as an <see langword="int"/></returns>
    public static int[] ReadIntegers() => ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray() ?? Array.Empty<int>();

    /// <summary>
    /// Reads the details of the specified number of processes one by one from the console.
    /// </summary>
    /// <param name="readPriority">If <see langword="true"></see>, also reads priorities. Default value is <see langword="false"/></param>
    /// <returns>An <see cref="ArrivalSchedule"/> containing the processes with the entered values.</returns>
    public static ArrivalSchedule ReadProcesses(int count, bool readPriority = false)
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

            int priority = 0;
            if (readPriority)
            {
                Write("... Enter priority: ");
                priority = ReadInteger();
            }

            WriteLine();

            schedule.Add(
                arrivalTime,
                new Process
                {
                    Id = processId,
                    BurstTime = burstTime,
                    Priority = priority
                });
        }
        return schedule;
    }

    /// <summary>
    /// Reads the arrival times, burst times and optionally priorities as space-separated integers.
    /// </summary>
    /// <param name="readPriority">If <see langword="true"></see>, also reads priorities. Default value is <see langword="false"/></param>
    /// <returns>An <see cref="ArrivalSchedule"/> containing the processes with the entered values.</returns>
    public static ArrivalSchedule ReadProcessesSingleLine(bool readPriority = false)
    {
        WriteLine("Enter values separated by space:");

        Write("Enter arrival times > ");
        int[] arrivalTimes = ReadIntegers();

        Write("Enter burst times > ");
        int[] burstTimes = ReadIntegers();

        int[]? priorities = null;
        if (readPriority)
        {
            Write("Enter priorities > ");
            priorities = ReadIntegers();
        }

        ArrivalSchedule schedule = new();
        for (int i = 0; i < arrivalTimes.Length; i++)
        {
            schedule.Add(arrivalTimes[i], new Process
            {
                Id = i + 1,
                BurstTime = burstTimes[i],
                Priority = priorities?[i] ?? 0
            });
        }
        return schedule;
    }
}
