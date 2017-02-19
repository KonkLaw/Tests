using BenchmarkDotNet.Attributes;

namespace Tests.Tests
{
	public class TestArraysBoundsCheck
	{
		private int count;
		float[] a;
		float[] b;
		float[] resultFixedCountRun;
		float[] resultOptimizedRun;

		public TestArraysBoundsCheck()
		{
			count = 100000000;
			a = RandomHelper.GetNumbers(count);
			b = RandomHelper.GetNumbers(count);
			resultFixedCountRun = new float[count];
			resultOptimizedRun = new float[count];
		}

		[Benchmark]
		public float[] Sum_slow()
		{
			for (int i = 0; i < count; i++)
			{
				resultFixedCountRun[i] = a[i] + b[i];
			}
			return resultFixedCountRun;
		}

		[Benchmark]
		public float[] Sum_fast()
		{
			// This caching is REALLY important.
			float[] a_ = a;
			float[] b_ = b;
			float[] res2_ = resultOptimizedRun;

			for (int i = 0; i < a_.Length; i++)
			{
				res2_[i] = a_[i] + b_[i];
			}
			return res2_;
		}
	}
}