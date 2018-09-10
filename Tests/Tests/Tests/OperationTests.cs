using System;
using BenchmarkDotNet.Attributes;

namespace Tests.Tests
{
	public class OperationTests
	{
		private float a;
		private float b;

		public OperationTests()
		{
			a = RandomHelper.GetFloat();
			b = RandomHelper.GetFloat();
		}

		[Benchmark]
		public double LogTest()
		{
			return Math.Log10(a);
		}

		[Benchmark]
		public double SqrtTest()
		{
			return Math.Sqrt(a);
		}

		[Benchmark]
		public float SumTest()
		{
			return a + b;
		}

	}
}