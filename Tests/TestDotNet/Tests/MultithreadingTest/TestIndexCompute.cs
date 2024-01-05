using BenchmarkDotNet.Attributes;

namespace TestDotNet.Tests.MultithreadingTest;


public class TestIndexCompute
{
    [Params(1_000_000, 4_000_000)]
    public int Bound { get; set; }

    [Params(false, true)]
    public bool UseMultiThread { get; set; }
    
    [Benchmark]
    public int[] GetIndeces()
    {
        int size = (int)Math.Sqrt(Bound);
        int bound = UseMultiThread ? 1 : int.MaxValue;
        return IndexGridComputer.GetIndeces(size, size, bound);
    }
}

public class TestWireFrameIndexCompute
{
    [Params(1_000_000, 4_000_000)]
    public int Bound { get; set; }

    [Params(false, true)]
    public bool UseMultiThread { get; set; }

    [Benchmark]
    public int[] GetIndeces()
    {
        int size = (int)Math.Sqrt(Bound);
        int bound = UseMultiThread ? 1 : int.MaxValue;
        return IndexGridComputer.GetWireframeIndeces(size, size, bound);
    }
}



class IndexGridComputer
{
    public static int[] GetIndeces(int width, int height, int bound)
    {
        if (width < 2 || height < 2)
            throw new ArgumentException("To small size for mesh.");
        var computation = new IndexGridComputeInfo(width, height, bound);
        ParallelComputation.Run(computation);
        return computation.Result;
    }

    class IndexGridComputeInfo : ParallelComputation.IComputationInfo
    {
        public readonly int[] Result;
        private readonly int cellsCount;
        private readonly int width;
        private readonly int bound;

        public IndexGridComputeInfo(int width, int height, int bound)
        {
            cellsCount = (width - 1) * (height - 1);
            Result = new int[6 * cellsCount];
            this.width = width;
            this.bound = bound;
        }

        public int GetSingleThreadItemsCount() => bound;

        public int GetItemsCount() => cellsCount;

        public void ProcessItems(int start, int stop)
        {
            int[] array = Result;
            int width_ = width;
            for (int i = start; i < stop; i++)
            {
                int cellJ = i / width_;
                int cellI = i % width_;
                WriteIndeces(cellI, cellJ, width_, array);
            }
        }
    }

    private static void WriteIndeces(int cellI, int cellJ, int widthPoints, int[] array)
    {
        int baseIndex = 6 * (cellJ * (widthPoints - 1) + cellI);
        int v0 = cellJ * widthPoints + cellI;
        array[baseIndex] = v0;
        array[++baseIndex] = v0 + widthPoints + 1;
        array[++baseIndex] = v0 + 1;
        array[++baseIndex] = v0;
        array[++baseIndex] = v0 + widthPoints;
        array[++baseIndex] = v0 + widthPoints + 1;
    }

    public static int[] GetWireframeIndeces(int width, int height, int bound)
    {
        if (width < 2 || height < 2)
            throw new ArgumentException("To small size for mesh.");
        var computation = new WireframeIndexGridComputeInfo(width, height, bound);
        ParallelComputation.Run(computation);
        return computation.Result;
    }

    class WireframeIndexGridComputeInfo : ParallelComputation.IComputationInfo
    {
        public readonly int[] Result;
        private readonly int cellsCount;
        private readonly int width;
        private readonly int bound;

        public WireframeIndexGridComputeInfo(int width, int height, int bound)
        {
            cellsCount = (width - 1) * (height - 1);
            Result = new int[8 * cellsCount];
            this.width = width;
            this.bound = bound;
        }

        public int GetSingleThreadItemsCount() => bound;

        public int GetItemsCount() => cellsCount;

        public void ProcessItems(int start, int stop)
        {
            int[] array = Result;
            int width_ = width;
            for (int i = start; i < stop; i++)
            {
                int cellJ = i / width_;
                int cellI = i % width_;
                WriteWireframeIndeces(cellI, cellJ, width_, array);
            }
        }
    }

    private static void WriteWireframeIndeces(int cellI, int cellJ, int widthPoints, int[] array)
    {
        int baseIndex = 8 * (cellJ * (widthPoints - 1) + cellI);
        int v0 = cellJ * widthPoints + cellI;
        int v1 = v0 + widthPoints;
        int v2 = v1 + 1;
        int v3 = v0 + 1;
        array[baseIndex] = v0;
        array[++baseIndex] = v1;
        array[++baseIndex] = v1;
        array[++baseIndex] = v2;
        array[++baseIndex] = v2;
        array[++baseIndex] = v3;
        array[++baseIndex] = v3;
        array[++baseIndex] = v0;
    }
}

class ParallelComputation
{
    private static readonly int DegreeOfParallelism;

    private readonly IComputationInfo computationInfo;
    private readonly int itemsCount;
    private readonly int maxBatchSize;

    static ParallelComputation()
    {
        DegreeOfParallelism = Environment.ProcessorCount;
    }

    private protected ParallelComputation(IComputationInfo computationInfo, int itemsCount, int maxBatchSize)
    {
        this.computationInfo = computationInfo;
        this.itemsCount = itemsCount;
        this.maxBatchSize = maxBatchSize;
    }

    internal static void Run(IComputationInfo computation)
    {
        int itemsCount = computation.GetItemsCount();
        if (itemsCount <= computation.GetSingleThreadItemsCount())
        {
            computation.ProcessItems(0, itemsCount);
        }
        else
        {
            int maxBatchSize = GetMaxSizeInBatch(itemsCount);
            Parallel.For(0, DegreeOfParallelism, new ParallelComputation(computation, itemsCount, maxBatchSize).Handle);
        }
    }

    private void Handle(int threadId)
    {
        int start = threadId * maxBatchSize;
        int stop = Math.Min((threadId + 1) * maxBatchSize, itemsCount);
        computationInfo.ProcessItems(start, stop);
    }

    private static int GetMaxSizeInBatch(int itemsCount)
        => itemsCount / DegreeOfParallelism + (itemsCount % DegreeOfParallelism == 0 ? 0 : 1);

    internal interface IComputationInfo
    {
        int GetSingleThreadItemsCount();
        int GetItemsCount();
        void ProcessItems(int start, int stop);
    }
}