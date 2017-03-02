using BenchmarkDotNet.Attributes;
using System;
using System.Linq;

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
