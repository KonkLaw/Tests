using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public class MatrixCalcTest
{
	[Params(100_000, 5_000_000)]
    public int MatrixCount { get; set; }

	private readonly Collection<MatrixComputeInfo> collection;
	public Collection<MatrixComputeInfo> Collection => collection;
	private Action action = null!;

	public MatrixCalcTest()
    {
        List<MatrixComputeInfo> list = new()
        {
	        new MatrixComputeInfo(null),
			new MatrixComputeInfo(2),
	        new MatrixComputeInfo(4)
        };
        collection = new Collection<MatrixComputeInfo>(list);
    }

    [GlobalSetup]
    public void Setup()
	    => action = collection.GetLastEnumerated()!.GetAction(MatrixCount);

    [Benchmark]
    [ArgumentsSource(nameof(Collection))]
	public void Run(MatrixComputeInfo info) => action();
}

class MatrixAlgorithms
{
	public static Matrix4F[] GetRandomMatrices(int count)
	{
		var matrices = new Matrix4F[count];
		for (int i = 0; i < count; i++)
		{
			matrices[i] = new Matrix4F
			{
				M11 = RandomHelper.GetFloat(),
				M12 = RandomHelper.GetFloat(),
				M13 = RandomHelper.GetFloat(),
				M14 = RandomHelper.GetFloat(),

				M21 = RandomHelper.GetFloat(),
				M22 = RandomHelper.GetFloat(),
				M23 = RandomHelper.GetFloat(),
				M24 = RandomHelper.GetFloat(),

				M31 = RandomHelper.GetFloat(),
				M32 = RandomHelper.GetFloat(),
				M33 = RandomHelper.GetFloat(),
				M34 = RandomHelper.GetFloat(),

				M41 = RandomHelper.GetFloat(),
				M42 = RandomHelper.GetFloat(),
				M43 = RandomHelper.GetFloat(),
				M44 = RandomHelper.GetFloat(),
			};
		}
		return matrices;
	}

	public static void ProcessRange(Matrix4F[] matrix, int start, int stop)
	{
		Matrix4F res = new();
		for (int i = start; i < stop; i++)
		{
			Matrix4F.Invert(matrix[i], out Matrix4F result);
			res += result;
		}
	}
}


public record MatrixComputeInfo(int? ThreadsCount)
{
	public override string ToString() => ThreadsCount.HasValue ? ThreadsCount.Value.ToString() : "(1) SingleThread";

	public Action GetAction(int matricesCount)
	{
		Matrix4F[] matrices = MatrixAlgorithms.GetRandomMatrices(matricesCount);

		Action action;
		if (ThreadsCount.HasValue)
			action = () => new ForBasedCompute<Matrix4F>(ThreadsCount.Value, MatrixAlgorithms.ProcessRange).Run(matrices, null);
		else
			action = () => MatrixAlgorithms.ProcessRange(matrices, 0, matrices.Length);
		return action;
	}
}