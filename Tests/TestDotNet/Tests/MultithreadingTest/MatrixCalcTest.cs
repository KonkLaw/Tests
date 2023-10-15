using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public class MatrixInvCalcTest
{
	[Params(500, 600, 7000, 800, 1000, 1500, 2000)]
    public int MatrixCount { get; set; }

	private readonly Collection<MatrixComputeInfo> collection;
	public Collection<MatrixComputeInfo> Collection => collection;
	private Action action = null!;

	public MatrixInvCalcTest()
    {
        List<MatrixComputeInfo> list = new()
        {
	        new MatrixComputeInfo(null),
			new MatrixComputeInfo(2),
	        new MatrixComputeInfo(4)
        };
		if (list.Last().ThreadsCount != Environment.ProcessorCount)
		{
            list.Add(new MatrixComputeInfo(Environment.ProcessorCount));
        }
        collection = new Collection<MatrixComputeInfo>(list);
    }

    [GlobalSetup]
    public void Setup()
	    => action = collection.GetCurrentEnumerated()!.GetAction(MatrixCount);

    [Benchmark]
    [ArgumentsSource(nameof(Collection))]
	public void Run(MatrixComputeInfo info) => action();
}

class MatrixAlgorithms
{
	public static Matrix4F GetRandomMatrix()
	{
		return new Matrix4F
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

	public static Matrix4F[] GetRandomMatrices(int count)
	{
		var matrices = new Matrix4F[count];
		for (int i = 0; i < count; i++)
		{
			matrices[i] = GetRandomMatrix();
		}
		return matrices;
	}

	public static void ProcessRange(Matrix4F[] sourceMatrix, Matrix4F[] destMatrix, int start, int stop)
	{
		for (int i = start; i < stop; i++)
		{
			Matrix4F.Invert(sourceMatrix[i], out destMatrix[i]);
		}
	}
}


public record MatrixComputeInfo(int? ThreadsCount)
{
	public override string ToString() => ThreadsCount.HasValue ? ThreadsCount.Value.ToString("D2") : "(1) SingleThread";

	public Action GetAction(int matricesCount)
	{
		Matrix4F[] matrices = MatrixAlgorithms.GetRandomMatrices(matricesCount);
		Matrix4F[] destMatrices = new Matrix4F[matricesCount];

        Action action;
		if (ThreadsCount.HasValue)
			action = () => new ForBasedCompute<Matrix4F>(ThreadsCount.Value, (array, start, stop) => MatrixAlgorithms.ProcessRange(matrices, destMatrices, start, stop)).Run(matrices, null);
		else
			action = () => MatrixAlgorithms.ProcessRange(matrices, destMatrices, 0, matrices.Length);
		return action;
	}
}