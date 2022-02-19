using System.Collections;

namespace CpuSchedulingAlgorithms;

public class ProcessQueueView : IEnumerable<ProcessControlBlock>
{
    private IEnumerable<ProcessControlBlock> _queue;

    public ProcessQueueView(IEnumerable<ProcessControlBlock> queue)
    {
        _queue = queue;
    }

    public IEnumerator<ProcessControlBlock> GetEnumerator()
    {
        return _queue.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}