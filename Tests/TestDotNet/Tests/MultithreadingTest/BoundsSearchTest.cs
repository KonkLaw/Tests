﻿using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public class BoundsSearchTest
{
    //[Params(false)]
    //public bool FindFullBounds { get; set; }

    //[Params(3)]
    //public int DimensionsCount { get; set; }
    //[Params(1_000, 3_000, 4_000, 5_000, 6_000, 7000, 7500, 10_000)]
    //public int Count { get; set; }

    //[Params(2)]
    //public int DimensionsCount { get; set; }
    //[Params(1_500, 6_000, 7_500, 9_000, 10_500, 11_250, 15_000)]
    //public int Count { get; set; }

    //[Params(1)]
    //public int DimensionsCount { get; set; }
    //[Params(3000, 9_000, 12_000, 15_000, 18_000, 21_000, 22500, 30_000)]
    //public int Count { get; set; }



    [Params(true)]
    public bool FindFullBounds { get; set; }

    [Params(3)]
    public int DimensionsCount { get; set; }
    [Params(1_000, 4_000, 5_000, 6_000, 7000, 7500, 10_000)]
    public int Count { get; set; }

    //[Params(2)]
    //public int DimensionsCount { get; set; }
    //[Params(1_500, 6_000, 7_500, 9_000, 10_500, 11_250, 15_000)]
    //public int Count { get; set; }

    //[Params(1)]
    //public int DimensionsCount { get; set; }
    //[Params(3_000, 12_000, 15_000, 18_000, 21_000, 22_500, 30_000)]
    //public int Count { get; set; }


    private readonly Collection<ComputeInfo> collection;

    public BoundsSearchTest()
    {
        List<int> threadsCounts = new List<int> { 2 };
        if (threadsCounts.Last() < Environment.ProcessorCount)
        {
            threadsCounts.Add(Environment.ProcessorCount);
        }
        //foreach (int multiplier in new[] { 2, 3, 5, 20 })
        //{
        //    threadsCounts.Add(multiplier * Environment.ProcessorCount);
        //}
        List<ComputeInfo> list = new List<ComputeInfo>
        {
            new ComputeInfo(null, true)
        };
        foreach (int count in threadsCounts)
        {
            if (count > Environment.ProcessorCount)
                break;
            list.Add(new ComputeInfo(count, UseTasks: false));
            //list.Add(new ComputeInfo(count, UseTasks: true));
        }
        collection = new Collection<ComputeInfo>(list);
    }

    public IEnumerable<ComputeInfo> ComputeInfos => collection;

    private Action action = null!;

    [GlobalSetup]
    public void Setup()
    {
        action = collection.GetCurrentEnumerated()!.GetAction(Count, DimensionsCount, FindFullBounds);
    }

    [Benchmark]
    [ArgumentsSource(nameof(ComputeInfos))]
    public void Run(ComputeInfo computeInfo) => action();
}

public record ComputeInfo(int? ThreadsCount, bool UseTasks)
{
    public override string ToString()
    {
        if (ThreadsCount.HasValue)
        {
            if (UseTasks)
                return $"{ThreadsCount.Value:D3} Tasks ";
            string xInfo = ThreadsCount.Value % Environment.ProcessorCount == 0
                ? string.Concat(ThreadsCount.Value / Environment.ProcessorCount, "x")
                : string.Empty;
            return $"{ThreadsCount.Value:D3} PaFor " + xInfo;
        }
        return "(--) SingleThread";
    }

    public Action GetAction(int pointCount, int dimensionsCount, bool findFullBounds)
    {
        Action<float[], int, int> GetAction1() => findFullBounds ? Algorithms.Find1FPosMin : Algorithms.Find1FBounds;
        Action<Vector2F[], int, int> GetAction2() => findFullBounds ? Algorithms.Find2FPosMin : Algorithms.Find2FBounds;
        Action<Vector3F[], int, int> GetAction3() => findFullBounds ? Algorithms.Find3FPosMin : Algorithms.Find3FBounds;

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
                throw new InvalidOperationException();
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

static class Algorithms
{
    public static void Find3FBounds(Vector3F[] data, int start, int stop)
    {
        FastBounds3 fastBounds3 = new FastBounds3(data[start]);
        for (int i = start + 1; i < stop; i++)
            fastBounds3.Merge(ref data[i]);
        Bounds bounds = fastBounds3.GetBounds();
    }

    public static void Find3FPosMin(Vector3F[] data, int start, int stop)
    {
        FastBounds3PosMin fastBounds = new FastBounds3PosMin();
        for (int i = start; i < stop; i++)
            fastBounds.Merge(ref data[i]);
        var bounds = fastBounds.GetPositiveMin();
    }

    public static void Find2FBounds(Vector2F[] data, int start, int stop)
    {
        FastBounds fastBounds3 = new FastBounds(data[start]);
        for (int i = start + 1; i < stop; i++)
            fastBounds3.Merge(ref data[i]);
        fastBounds3.GetBounds(out _, out _);
    }

    public static void Find2FPosMin(Vector2F[] data, int start, int stop)
    {
        FastBounds2PosMin fastBounds = new FastBounds2PosMin();
        for (int i = start; i < stop; i++)
            fastBounds.Merge(data[i]);
        var bounds = fastBounds.GetPositiveMin();
    }

    public static void Find1FBounds(float[] data, int start, int stop)
    {
        FastBounds1 fastBounds3 = new FastBounds1(data[start]);
        for (int i = start + 1; i < stop; i++)
            fastBounds3.Merge(ref data[i]);
        fastBounds3.GetBounds(out _, out _);
    }

    public static void Find1FPosMin(float[] data, int start, int stop)
    {
        FastBounds1PosMin fastBounds = new FastBounds1PosMin();
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

    struct FastBounds3PosMin
    {
        private Vector3F positiveMin;

        public FastBounds3PosMin()
        {
            positiveMin = new Vector3F(float.MaxValue);
        }

        public void Merge(ref Vector3F point)
        {
            if (point.X > 0 & point.X < positiveMin.X)
                positiveMin.X = point.X;

            if (point.Y > 0 & point.Y < positiveMin.Y)
                positiveMin.Y = point.Y;

            if (point.Z > 0 & point.Z < positiveMin.Z)
                positiveMin.Z = point.Z;
        }

        public Vector3F GetPositiveMin() => positiveMin;
    }

    struct FastBounds
    {
        private Vector2F min;
        private Vector2F max;

        public FastBounds(Vector2F firstPoint)
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

    struct FastBounds2PosMin
    {
        private Vector2F positiveMin;

        public FastBounds2PosMin()
        {
            positiveMin = new Vector2F(float.MaxValue);
        }

        public void Merge(Vector2F point)
        {
            if (point.X > 0 & point.X < positiveMin.X)
                positiveMin.X = point.X;

            if (point.Y > 0 & point.Y < positiveMin.Y)
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

    struct FastBounds1PosMin
    {
        private float positiveMin;

        public FastBounds1PosMin()
        {
            positiveMin = float.MaxValue;
        }

        public void Merge(float point)
        {
            if (point > 0 & point < positiveMin)
                positiveMin = point;
        }

        public float GetPositiveMin() => positiveMin;
    }
}
