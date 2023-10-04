using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public class MinMaxComputationTest
{
	private readonly float[] values;
	private const int PointCount = 50_000;

	public MinMaxComputationTest()
	{
		values = RandomHelper.GetFloatNumbers(PointCount, -5, 5);
	}

	[Benchmark]
	public float SearchPositiveMin()
	{
		float[] data = values;
		float min = float.MaxValue;
		for (int i = 0; i < data.Length; i++)
		{
			float value = data[i];
			if (value >= 0 && value < min) // (1)
				min = value;
		}
		return min;
	}

	[Benchmark]
	public float SearchMinMax()
	{
		float[] data = values;

		float min;
		float max;
		min = max = data[0];
		for (int i = 1; i < data.Length; i++)
		{
			float value = data[i];
			if (value > max)
				max = value;
			else if (value < min)
				min = value;
		}
		return min + max;
	}

	[Benchmark]
	public float SearchPositiveMin_Fast()
	{
		float[] data = values;
		float min = float.MaxValue;
		for (int i = 0; i < data.Length; i++)
		{
			float value = data[i];
			if (value >= 0 & value < min)
				min = value;
		}
		return min;
	}
}