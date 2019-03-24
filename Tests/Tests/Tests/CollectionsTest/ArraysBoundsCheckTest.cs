using BenchmarkDotNet.Attributes;
using Tests.HelpersTypes;

//BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
//Frequency = 2435957 Hz, Resolution = 410.5163 ns, Timer = TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//DefaultJob: Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//    Method | Mean | StdDev |
// --------- | ------------ | ----------- |
//  Sum_slow | 202.3664 ms | 15.7631 ms |
//  Sum_fast | 151.1016 ms | 0.2391 ms |

namespace Tests.Tests
{
	public class ArraysBoundsCheckTest
	{
		private int count;
		float[] a;
		float[] b;
		float[] resultFixedCountRun;
		float[] resultOptimizedRun;

		public ArraysBoundsCheckTest()
		{
			count = 100000000;
			a = RandomHelper.GetFloatNumbers(count);
			b = RandomHelper.GetFloatNumbers(count);
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
			// In this  case caching is REALLY important.
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