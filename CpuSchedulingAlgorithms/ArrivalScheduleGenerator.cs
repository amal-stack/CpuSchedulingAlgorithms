namespace CpuSchedulingAlgorithms;


public class ArrivalScheduleGenerator
{
    public static ArrivalSchedule GenerateRandomArrivalSchedule(
        int processCount, 
        int arrivalTimeLimit, 
        int burstTimeLimit)
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
