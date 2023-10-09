namespace TestDotNet.Tests.MultithreadingTest;

abstract class RunnerParallelFor<TElement, TState>
{
    private readonly Action<int> body;
    protected TElement[]? Data;
    protected TState State;

    protected RunnerParallelFor()
    {
        body = Handle;
    }

    public void Run(TElement[] array, TState state)
    {
        Data = array;
        State = state;

        int batchCount = GetThreadsCount();
        PrepareState(array.Length / batchCount);
        Parallel.For(0, batchCount, body);

        Data = null;
    }

    protected abstract int GetThreadsCount();

    protected abstract void PrepareState(int batchSize);

    protected abstract void Handle(int batchId);
}

abstract class RunnerTasksBase<TElement, TState>
{
    private Task[]? tasks;
    protected TElement[]? Data;
    protected TState? State;

    public void Run(TElement[] array, TState? state)
    {
        Data = array;
        State = state;

        int batchCount = GetThreadsCount();
        PrepareState(array.Length / batchCount);
        if (tasks == null || tasks.Length != batchCount)
            tasks = new Task[batchCount];
        for (int i = 0; i < batchCount; i++)
        {
            tasks[i] = Task.Factory.StartNew(Handle, i);
        }
        Task.WaitAll(tasks);
    }

    protected abstract int GetThreadsCount();

    protected abstract void PrepareState(int batchSize);

    protected abstract void Handle(object? info);
}

class ForBasedCompute<TElem> : RunnerParallelFor<TElem, object?>
{
	private readonly Action<TElem[], int, int> action;
	private readonly int threadsCount;
	private int countInBatch;

	public ForBasedCompute(int threadsCount, Action<TElem[], int, int> action)
	{
		this.action = action;
		this.threadsCount = threadsCount;
	}

	protected override int GetThreadsCount() => threadsCount;

	protected override void PrepareState(int batchSize) => countInBatch = batchSize;

	protected override void Handle(int batchId)
	{
		int start = batchId * countInBatch;
		int stop = (batchId + 1) * countInBatch;
		action(Data!, start, stop);
	}
}

class TaskBasedCompute<TElem> : RunnerTasksBase<TElem, object?>
	where TElem : unmanaged
{
	private readonly Action<TElem[], int, int> action;
	private readonly int threadsCount;
	private int countInBatch;

	public TaskBasedCompute(int threadsCount, Action<TElem[], int, int> action)
	{
		this.action = action;
		this.threadsCount = threadsCount;
	}

	protected override int GetThreadsCount() => threadsCount;

	protected override void PrepareState(int batchSize) => countInBatch = batchSize;

	protected override void Handle(object? info)
	{
		int batchIdd = (int)info!;
		int start = batchIdd * countInBatch;
		int stop = (batchIdd + 1) * countInBatch;
		action(Data!, start, stop);
	}
}