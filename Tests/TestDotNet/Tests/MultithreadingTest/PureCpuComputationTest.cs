using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public class PureCpuComputationTest
{
	[Params(5_000, 50_000)]
	public int MatrixCount { get; set; }

	private readonly Collection<MatrixLongComputeInfo> collection;
	public Collection<MatrixLongComputeInfo> Collection => collection;
	private Action action = null!;

	public PureCpuComputationTest()
	{
		List<MatrixLongComputeInfo> list = new()
		{
			new MatrixLongComputeInfo(null),
			new MatrixLongComputeInfo(2),
			new MatrixLongComputeInfo(4)
		};
		collection = new Collection<MatrixLongComputeInfo>(list);
	}

	[GlobalSetup]
	public void Setup()
		=> action = collection.GetLastEnumerated()!.GetAction(MatrixCount);

	[Benchmark]
	[ArgumentsSource(nameof(Collection))]
	public void Run(MatrixLongComputeInfo info) => action();
}

public record MatrixLongComputeInfo(int? ThreadsCount)
{
	public override string ToString() => ThreadsCount.HasValue ? ThreadsCount.Value.ToString() : "(1) SingleThread";

	public Action GetAction(int interactions)
	{
		Matrix4F matrix = MatrixAlgorithms.GetRandomMatrices(1)[0];

		Action action;
		if (ThreadsCount.HasValue)
			action = () =>
			{
				Parallel.For(0, ThreadsCount.Value, _ =>
				{
					MatrixComputationTest.Process(matrix, interactions);
				});
			};
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
				result -= new Matrix4F(RandomHelper.GetFloat(0.1f, 0.01f));
			else
				result += new Matrix4F(RandomHelper.GetFloat(0.1f, 0.01f));
			matrix = result;
		}
		return matrix;
	}
}