using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

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

static class Algorithms
{
    public static void Find3F_2(Vector3F[] data, int start, int stop)
    {
        FastBounds3 fastBounds3 = new FastBounds3(data[start]);
        for (int i = start + 1; i < stop; i++)
            fastBounds3.Merge(ref data[i]);
        Bounds bounds = fastBounds3.GetBounds();
    }

    public static void Find3F_1(Vector3F[] data, int start, int stop)
    {
        FastBounds3_2 fastBounds = new FastBounds3_2();
        for (int i = start; i < stop; i++)
            fastBounds.Merge(ref data[i]);
        var bounds = fastBounds.GetPositiveMin();
    }

    public static void Find2F_2(Vector2F[] data, int start, int stop)
    {
        FastBounds2 fastBounds3 = new FastBounds2(data[start]);
        for (int i = start + 1; i < stop; i++)
            fastBounds3.Merge(ref data[i]);
        fastBounds3.GetBounds(out _, out _);
    }

    public static void Find2F_1(Vector2F[] data, int start, int stop)
    {
        FastBounds2_2 fastBounds = new FastBounds2_2();
        for (int i = start; i < stop; i++)
            fastBounds.Merge(data[i]);
        var bounds = fastBounds.GetPositiveMin();
    }

    public static void Find1F_2(float[] data, int start, int stop)
    {
        FastBounds1 fastBounds3 = new FastBounds1(data[start]);
        for (int i = start + 1; i < stop; i++)
            fastBounds3.Merge(ref data[i]);
        fastBounds3.GetBounds(out _, out _);
    }

    public static void Find1F_1(float[] data, int start, int stop)
    {
        FastBounds1_2 fastBounds = new FastBounds1_2();
        for (int i = start; i < stop; i++)
            fastBounds.Merge(data[i]);
        var bounds = fastBounds.GetPositiveMin();
    }

    public struct FastBounds3
    {
        private Vector3F min;
        private Vector3F max;

        public FastBounds3(Vector3F firstPoint)
        {
            min = max = firstPoint;
        }

        public void Merge(ref Vector3F point)
        {
            if (point.X > max.X)
                max.X = point.X;
            else if (point.X < min.X)
                min.X = point.X;

            if (point.Y > max.Y)
                max.Y = point.Y;
            else if (point.Y < min.Y)
                min.Y = point.Y;

            if (point.Z > max.Z)
                max.Z = point.Z;
            else if (point.Z < min.Z)
                min.Z = point.Z;
        }

        public Bounds GetBounds() => new Bounds { Max = max, Min = min };
    }

    struct FastBounds3_2
    {
        private Vector3F positiveMin;

        public FastBounds3_2()
        {
            positiveMin = new Vector3F(float.MaxValue);
        }

        public void Merge(ref Vector3F point)
        {
            if (point.X > 0 && point.X < positiveMin.X)
                positiveMin.X = point.X;

            if (point.Y > 0 && point.Y < positiveMin.Y)
                positiveMin.Y = point.Y;

            if (point.Z > 0 && point.Z < positiveMin.Z)
                positiveMin.Z = point.Z;
        }

        public Vector3F GetPositiveMin() => positiveMin;
    }

    struct FastBounds2
    {
        private Vector2F min;
        private Vector2F max;

        public FastBounds2(Vector2F firstPoint)
        {
            min = max = firstPoint;
        }

        public void Merge(ref Vector2F point)
        {
            if (point.X > max.X)
                max.X = point.X;
            else if (point.X < min.X)
                min.X = point.X;

            if (point.Y > max.Y)
                max.Y = point.Y;
            else if (point.Y < min.Y)
                min.Y = point.Y;
        }

        public void GetBounds(out Vector2F minOut, out Vector2F maxOut)
        {
            minOut = min;
            maxOut = max;
        }
    }

    struct FastBounds2_2
    {
        private Vector2F positiveMin;

        public FastBounds2_2()
        {
            positiveMin = new Vector2F(float.MaxValue);
        }

        public void Merge(Vector2F point)
        {
            if (point.X > 0 && point.X < positiveMin.X)
                positiveMin.X = point.X;

            if (point.Y > 0 && point.Y < positiveMin.Y)
                positiveMin.Y = point.Y;
        }

        public Vector2F GetPositiveMin() => positiveMin;
    }

    struct FastBounds1
    {
        private float min;
        private float max;

        public FastBounds1(float firstPoint)
        {
            min = max = firstPoint;
        }

        public void Merge(ref float point)
        {
            if (point > max)
                max = point;
            else if (point < min)
                min = point;
        }

        public void GetBounds(out float minOut, out float maxOut)
        {
            minOut = min;
            maxOut = max;
        }
    }

    struct FastBounds1_2
    {
        private float positiveMin;

        public FastBounds1_2()
        {
            positiveMin = float.MaxValue;
        }

        public void Merge(float point)
        {
            if (point > 0 && point < positiveMin)
                positiveMin = point;
        }

        public float GetPositiveMin() => positiveMin;
    }
}

public record ComputeInfo(int? ThreadsCount, bool UseTasks, string Id)
{
    public override string ToString() =>
        Id + (ThreadsCount.HasValue
            ? $" {(UseTasks ? "TaskBased" : "ParallelBased")} ({ThreadsCount.Value})"
            : " One thread");

    public Action GetAction(int pointCount, int dimensionsCount, bool findFullBounds)
    {
        Action<float[], int, int> GetAction1() => findFullBounds ? Algorithms.Find1F_1 : Algorithms.Find1F_2;
        Action<Vector2F[], int, int> GetAction2() => findFullBounds ? Algorithms.Find2F_1 : Algorithms.Find2F_2;
        Action<Vector3F[], int, int> GetAction3() => findFullBounds ? Algorithms.Find3F_1 : Algorithms.Find3F_2;

        switch (dimensionsCount)
        {
            case 1:
                float[] vectors1F = RandomHelper.GetFloatNumbers(pointCount);
                return GetAction(vectors1F, ThreadsCount, GetAction1());
            case 2:
                Vector2F[] vectors2F = RandomHelper.GetVectors2F(pointCount, -5, 5);
                return GetAction(vectors2F, ThreadsCount, GetAction2());
            case 3:
                Vector3F[] vectors3F = RandomHelper.GetVectors(pointCount, -5, 5);
                return GetAction(vectors3F, ThreadsCount, GetAction3());
            default:
                throw new NotImplementedException();
        }
    }

    private Action GetAction<TElement>(TElement[] array, int? threadsCount, Action<TElement[], int, int> action)
        where TElement : unmanaged
    {
        if (threadsCount.HasValue)
        {
            Action<TElement[], object?> computer = UseTasks
                ? new TaskBasedCompute<TElement>(threadsCount.Value, action).Run
                : new ForBasedCompute<TElement>(threadsCount.Value, action).Run;

            return () => computer(array, null);
        }
        else
        {
            return () => action(array, 0, array.Length);
        }
    }
}

public class BoundsSearchTest
{
    [Params(true, false)]
    public bool FindFullBounds { get; set; }

    [Params(1, 2, 3)]
    public int DimensionsCount { get; set; }

    [Params(100, 500, 1_000, 5_000, 7_000, 8_500, 10_000, 50_000, 100_000, 500_000, 5_000_000)]
    public int Count { get; set; }

    private readonly Collection<ComputeInfo> collection;

    public BoundsSearchTest()
    {
        List<int> threadsCounts = new List<int> { 1, 2, 4, 6, 8 };
        if (threadsCounts.Last() < Environment.ProcessorCount)
        {
            threadsCounts.Add(Environment.ProcessorCount);
        }

        int counter = 100;
        List<ComputeInfo> list = new List<ComputeInfo>
        {
            new ComputeInfo(null, true, "---")
        };
        foreach (int count in threadsCounts)
        {
            list.Add(new ComputeInfo(count, UseTasks: true, counter++.ToString()));
            list.Add(new ComputeInfo(count, UseTasks: false, counter++.ToString()));
        }
        collection = new Collection<ComputeInfo>(list);
    }

    public IEnumerable<ComputeInfo> ComputeInfos => collection;

    private Action action = null!;

    [GlobalSetup]
    public void Setup()
    {
        action = collection.GetLastEnumerated()!.GetAction(Count, DimensionsCount, FindFullBounds);
    }

    [Benchmark]
    [ArgumentsSource(nameof(ComputeInfos))]
    public void Run(ComputeInfo computeInfo) => action();
}