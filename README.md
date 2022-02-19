[![.NET](https://github.com/amal-stack/CpuSchedulingAlgorithms/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/amal-stack/CpuSchedulingAlgorithms/actions/workflows/dotnet.yml)

# CPU Scheduling Algorithms
* This project attempts to implement CPU scheduling algorithms used by operating systems in C#. 
* The implementation attempts to simulate a CPU scheduler by using a predetermined arrival schedule. 
* This project also features an interactive console application(under [*CpuSchedulingAlgorithms.Console*](https://github.com/amal-stack/CpuSchedulingAlgorithms/tree/master/CpuSchedulingAlgorithms.Console)) to experiment with the implemented algorithms.

## Algorithms
This project currently implements the following scheduling algorithms:
### [First Come, First Serve](https://github.com/amal-stack/CpuSchedulingAlgorithms/blob/master/CpuSchedulingAlgorithms/FirstComeFirstServeScheduler.cs)
* A non-preemptive scheduling algorithm where the process with the earliest arrival time would be the first one to be granted the CPU. 
### [Shortest Job First](https://github.com/amal-stack/CpuSchedulingAlgorithms/blob/master/CpuSchedulingAlgorithms/ShortestJobFirstScheduler.cs)
* Another non-preemptive algorithm where the process requiring the shortest time to complete is granted the CPU first.
### [Shortest Remaining Time First](https://github.com/amal-stack/CpuSchedulingAlgorithms/blob/master/CpuSchedulingAlgorithms/ShortestRemainingTimeScheduler.cs)
* Preemptive version of shortest job first where the ready queue will be examined after each time quantum and if required suspending the current process if a process requiring a lesser amount of time arrives.
### [Round Robin](https://github.com/amal-stack/CpuSchedulingAlgorithms/blob/master/CpuSchedulingAlgorithms/RoundRobinScheduler.cs)
* A preemptive algorithm that allocates the CPU for a fixed *time quantum* for each process in the ready queue, a variation of first-come, first serve.

### [Priority (Non-preemptive)](https://github.com/amal-stack/CpuSchedulingAlgorithms/blob/master/CpuSchedulingAlgorithms/PriorityScheduler.cs)
* A scheduling algorithm where each process has an associated priority value. The process with the highest priority will be granted the CPU first. This is implemented as a non-preemptive algorithm where the priority is represented as an integer. The lower the value of the integer, the higher the process priority.
