using BenchmarkDotNet.Attributes;
using System;
using System.Linq;

//BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
//Frequency = 2435917 Hz, Resolution = 410.5230 ns, Timer = TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//DefaultJob: Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//
//	  Method | Mean | StdDev |
//------------ | ----------- | ---------- |
//   RunSigned | 27.6597 ms | 0.1177 ms |
// RunUnsigned | 12.2865 ms | 0.0896 ms |

namespace Tests.Tests
{
	public class ComparationTest
	{
		int[] signedValues;
		uint[] unsignedValues;

		const int minValueInArray = -500; // inclusive
		const int maxValueInArray = 2500; // exclusive

		const int minValueFilter = 5; // exclusive
		const int maxValueFilter = 100; // exclusive

		public ComparationTest()
		{
			if (minValueFilter >= maxValueFilter || minValueInArray >= maxValueInArray ||
				minValueFilter <= minValueInArray || maxValueFilter >= maxValueInArray)
				throw new Exception("Check bounds.");

			const int count = 10000000;
			signedValues = RandomHelper.GetIntNumbers(count, minValueInArray, maxValueInArray);
			unsignedValues = SickSh_tApi.Cast<int[], uint[]>(signedValues);
		}

		[Benchmark]
		public int RunSigned()
		{
			int counter = 0;
			int[] signedValues_ = signedValues;
			for (int i = 0; i < signedValues_.Length; i++)
			{
				int value = signedValues_[i];
				if (value > minValueFilter && value < maxValueFilter)
					counter++;
			}
			return counter;
		}

		[Benchmark]
		public int RunUnsigned()
		{
			int counter = 0;
			uint[] unsignedValues_ = unsignedValues;
			const uint delta = (minValueFilter + 1);
			const uint newUpperBound = maxValueFilter - delta;

			for (int i = 0; i < unsignedValues_.Length; i++)
			{
				uint value = unsignedValues_[i] - delta;
				if (value < newUpperBound)
					counter++;
			}
			return counter;
		}
	}
}
