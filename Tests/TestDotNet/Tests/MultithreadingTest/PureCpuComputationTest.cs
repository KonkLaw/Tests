using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public class PureCpuComputationTest
{
	[Params(50_000_000)]
	public int MatrixCount { get; set; }

	private readonly Collection<MatrixLongComputeInfo> collection;
	public Collection<MatrixLongComputeInfo> Collection => collection;
	private Action action = null!;
    private CancellationTokenSource source;
    private Task task1;
    private Task task2;

    public PureCpuComputationTest()
	{
		List<MatrixLongComputeInfo> list = new()
		{
			new MatrixLongComputeInfo(null),
            new MatrixLongComputeInfo(Environment.ProcessorCount / 2),
            new MatrixLongComputeInfo(Environment.ProcessorCount / 2 + 1),
            new MatrixLongComputeInfo(Environment.ProcessorCount / 2 + 2),
            new MatrixLongComputeInfo(Environment.ProcessorCount / 2 + 3),
            new MatrixLongComputeInfo(Environment.ProcessorCount - 4),
            new MatrixLongComputeInfo(Environment.ProcessorCount - 3),
            new MatrixLongComputeInfo(Environment.ProcessorCount - 2),
            new MatrixLongComputeInfo(Environment.ProcessorCount - 1),
            new MatrixLongComputeInfo(Environment.ProcessorCount),
            new MatrixLongComputeInfo(Environment.ProcessorCount + 1)
        };

		collection = new Collection<MatrixLongComputeInfo>(list);
	}

    [GlobalSetup]
    public void Setup()
    {
        action = collection.GetCurrentEnumerated()!.GetAction(MatrixCount);

		source = new CancellationTokenSource();
		CancellationToken token = source.Token;

		task1 = Task.Run(() =>
		{
			Matrix4F matrix = MatrixAlgorithms.GetRandomMatrices(1)[0];
			while (!token.IsCancellationRequested)
			{
				MatrixComputationTest.Process(matrix, 1000);
			}
		});

        task2 = Task.Run(() =>
        {
            Matrix4F matrix = MatrixAlgorithms.GetRandomMatrices(1)[0];
            while (!token.IsCancellationRequested)
            {
                MatrixComputationTest.Process(matrix, 1000);
            }
        });
    }

	[GlobalCleanup]
	public void GlobalCleanUp()
	{
        source.Cancel();
        task1.Wait();
		task2.Wait();
    }

    [Benchmark]
	[ArgumentsSource(nameof(Collection))]
	public void Run(MatrixLongComputeInfo info) => action();
}

public record MatrixLongComputeInfo(int? ThreadsCount)
{
	public override string ToString() => ThreadsCount.HasValue ? ThreadsCount.Value.ToString("D2") : "(01)OneThread";

	public Action GetAction(int interactions)
	{
		Matrix4F matrix = MatrixAlgorithms.GetRandomMatrices(1)[0];

		Action action;
		if (ThreadsCount.HasValue)
		{
			int countPerThread = interactions / ThreadsCount.Value;
            Action<int> forRun = _ =>
            {
                MatrixComputationTest.Process(matrix, countPerThread);
            };

            //if (ThreadsCount.Value > Environment.ProcessorCount)
			//{
				action = () =>
					Parallel.For(0, ThreadsCount.Value, new ParallelOptions
					{
						MaxDegreeOfParallelism = ThreadsCount.Value,
					}, forRun);
            //}
			//else
			//{
            //    action = () => Parallel.For(0, ThreadsCount.Value, forRun);
            //}
		}
		else
			action = () => MatrixComputationTest.Process(matrix, interactions);
		return action;
	}
}

class MatrixComputationTest
{
	public static Matrix4F Process(Matrix4F matrix, int count)
	{
		for (int i = 0; i < count; i++)
		{
			Matrix4F.Invert(matrix, out Matrix4F result);
			if (i % 2 == 0)
				result -= new Matrix4F(RandomHelper.GetFloat(min: 0.01f, max: 0.1f));
			else
				result += new Matrix4F(RandomHelper.GetFloat(min: 0.01f, max: 0.1f));
			matrix = result;
		}
		return matrix;
	}
}