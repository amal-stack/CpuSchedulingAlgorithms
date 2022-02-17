namespace CpuSchedulingAlgorithms;


public class ArrivalScheduleGenerator
{
    /// <summary>
    /// Generates an arrival schedule consisting of <see cref="Process"/> objects with random arrival times and burst times.
    /// </summary>
    /// <param name="processCount">The number of processes to be generated.</param>
    /// <param name="arrivalTimeLimit">The maximum arrival time that a process can have.</param>
    /// <param name="burstTimeLimit">The maximum burst time that a process can have.</param>
    /// <returns>The generated <see cref="ArrivalSchedule"/></returns>
    public static ArrivalSchedule GenerateRandomArrivalSchedule(
        int processCount, 
        int arrivalTimeLimit = 10, 
        int burstTimeLimit = 10)
    {
        ArrivalSchedule arrivalSchedule = new();
        Random random = new();
        foreach (var id in Enumerable.Range(0, processCount))
        {
            arrivalSchedule.Add(
                random.Next(0, arrivalTimeLimit),
                new Process
                {
                    Id = id,
                    BurstTime = random.Next(1, burstTimeLimit)
                });
        }
        return arrivalSchedule;
    }
}
