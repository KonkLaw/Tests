//  ----------------------------------------------------------------------------------------
//   Module name: StructInitTest.cs
//  
//   Author:      Artyom Polishchuk
//  
//   Created at:  20-04-2017
//  
//   Description: 
//  
//  ----------------------------------------------------------------------------------------

using System.CodeDom;
using BenchmarkDotNet.Attributes;
using Tests.HelpersTypes;

namespace Tests.Tests
{
	public class StructInitTest
	{
		public struct TestStruct
		{
			public float FloatVal;
			public int IntVal;
			public long LongVal;
			public double DoubleVal;

			public TestStruct(float floatVal, int intVal, long longVal, double doubleVal)
			{ 
				FloatVal = floatVal;		
				IntVal = intVal;
				LongVal = longVal;
				DoubleVal = doubleVal;
			}
		}

		private const int Count = 100000;
		//private readonly TestStruct[] array = new TestStruct[Count];

		private readonly int[] intArr = new int[Count];
		private readonly float[] floatArr = new float[Count];
		private readonly long[] longArr = new long[Count];
		private readonly double[] doubleArr = new double[Count];


		public StructInitTest()
		{
			floatArr = RandomHelper.GetFloatNumbers(Count);
			intArr = RandomHelper.GetIntNumbers(Count);
			longArr = RandomHelper.GetLongNumbers(Count);
			doubleArr = RandomHelper.GetDoubleNumbers(Count);
		}

		[Benchmark]
		public TestStruct GetWithConstr()
		{
			return new TestStruct(1f, 1, 1L, 1.0);
		}

		[Benchmark]
		public TestStruct GetWithInitializer()
		{
			return new TestStruct()
			{
				IntVal = 1,
				DoubleVal = 1.0,
				FloatVal = 1.0f,
				LongVal = 1L,
			};
		}
	}
}