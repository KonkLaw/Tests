using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

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

class ForRunner : RunnerParallelFor<Matrix4F, Matrix4F[]>
{
    private readonly int threads;
    private int countInBatch;

    public ForRunner(int threads)
    {
        this.threads = threads;
    }

    protected override int GetThreadsCount() => threads;

    protected override void PrepareState(int batchSize)
    {
        countInBatch = batchSize;
    }

    protected override void Handle(int batchId)
    {
        int start = batchId * countInBatch;
        int stop = (batchId + 1) * countInBatch;
        TypeOfParallelizationTest.Process(Data, State, start, stop);
    }
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

class TaskRunner : RunnerTasksBase<Matrix4F, Matrix4F[]>
{
    private readonly int threadsCount;
    private int countInBatch;

    public TaskRunner(int threadsCount)
    {
        this.threadsCount = threadsCount;
    }

    protected override int GetThreadsCount() => threadsCount;

    protected override void PrepareState(int batchSize)
    {
        countInBatch = batchSize;
    }

    protected override void Handle(object? info)
    {
        int batchIdd = (int)info!;
        int start = batchIdd * countInBatch;
        int stop = (batchIdd + 1) * countInBatch;
        TypeOfParallelizationTest.Process(Data!, State, start, stop);
    }
}

public class TypeOfParallelizationTest
{
    public static void Process(Matrix4F[] input, Matrix4F[] output, int startIndex, int stopExcl)
    {
        for (int i = startIndex; i < stopExcl; i++)
        {
            Matrix4F.Invert(input[i], out output[i]);
        }
    }

    private readonly ForRunner for1 = new ForRunner(1);
    private readonly ForRunner for2 = new ForRunner(2);
    private readonly ForRunner for4 = new ForRunner(4);
    private readonly ForRunner for6 = new ForRunner(6);
    private readonly ForRunner for8 = new ForRunner(8);
    private readonly ForRunner forX = new ForRunner(Environment.ProcessorCount);
    private readonly TaskRunner tasks1 = new TaskRunner(1);
    private readonly TaskRunner tasks2 = new TaskRunner(2);
    private readonly TaskRunner tasks4 = new TaskRunner(4);
    private readonly TaskRunner tasks6 = new TaskRunner(6);
    private readonly TaskRunner tasks8 = new TaskRunner(8);
    private readonly TaskRunner tasksX = new TaskRunner(Environment.ProcessorCount);

    private Matrix4F[] matrices = null!;
    private Matrix4F[] result = null!;

    [Params(100, 1000, 5000, 10_000, 50_000, 100_000, 500_000, 5_000_000, 50_000_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        matrices = ParallelExecutionTest.GetRandomMatrices(Count);
        result = new Matrix4F[matrices.Length];
    }

    [Benchmark] public void CallOneThread() => Process(matrices, result, 0, matrices.Length);
    
    [Benchmark] public void ParallelFor1() => for1.Run(matrices, result);
    [Benchmark] public void TaskBased1() => tasks1.Run(matrices, result);

    [Benchmark] public void ParallelFor2() => for2.Run(matrices, result);
    [Benchmark] public void TaskBased2() => tasks2.Run(matrices, result);

    [Benchmark] public void ParallelFor4() => for4.Run(matrices, result);
    [Benchmark] public void TaskBased4() => tasks4.Run(matrices, result);

    [Benchmark] public void ParallelFor6() => for6.Run(matrices, result);
    [Benchmark] public void TaskBased6() => tasks6.Run(matrices, result);

    [Benchmark] public void ParallelFor8() => for8.Run(matrices, result);
    [Benchmark] public void TaskBased8() => tasks8.Run(matrices, result);

    [Benchmark] public void ParallelForCoreCount() => forX.Run(matrices, result);
    [Benchmark] public void TaskBasedCoreCount() => tasksX.Run(matrices, result);
}