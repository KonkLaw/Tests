using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public class BoundsTransformMergeTest
{
    [Params(50, 100, 200, 300, 400, 500, 1000, 2000)]
    public int MatrixCount { get; set; }

    private readonly Collection<ComputeInfo> collection;
    public Collection<ComputeInfo> Collection => collection;

    private Action action = null!;

    public BoundsTransformMergeTest()
    {
        var list = new List<ComputeInfo>
        {
            new ComputeInfo(null),
            new ComputeInfo(2),
            new ComputeInfo(4),
        };
        if (Environment.ProcessorCount > 4)
            list.Add(new ComputeInfo(Environment.ProcessorCount));

        collection = new Collection<ComputeInfo>(list);
    }

    [GlobalSetup]
	public void Setup()
	{
        Matrix4F[] matrices = MatrixAlgorithms.GetRandomMatrices(MatrixCount);
        Bounds bounds = new Bounds
        {
            Min = RandomHelper.GetVector(-1, -5),
            Max = RandomHelper.GetVector(+5, +1),
        };
        action = collection.GetCurrentEnumerated()!.GetAction(matrices, bounds);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Collection))]
    public void Run(ComputeInfo info)
	{
        action();
	}

	public record ComputeInfo(int? threadsCount)
	{
        public override string ToString()
        {
            return threadsCount.HasValue ? threadsCount.Value.ToString("D2") : "--";
        }

        public Action GetAction(Matrix4F[] matrices, Bounds bounds)
        {
            if (threadsCount.HasValue)
            {
                int batchSize = matrices.Length / threadsCount.Value;

                return () =>
                {
                    Parallel.For(0, threadsCount.Value, (i) =>
                    {
                        int start = i * batchSize;
                        int stop = (i + 1) * batchSize;
                        Calculate(ref bounds, matrices, start, stop);
                    });
                };
            }
            else
            {
                return () =>
                {
                    Calculate(ref bounds, matrices, 0, matrices.Length);
                };
            }
        }
    }

    private static void Calculate(ref Bounds source, Matrix4F[] matrices, int start, int stop)
    {
        Bounds result = Bounds.GetInvalid();
        for (int i = start; i < stop; i++)
        {
            result.MergeTransformed(ref source, ref matrices[i]);
        }
    }
}
