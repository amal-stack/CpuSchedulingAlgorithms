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
