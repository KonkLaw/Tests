using System;
using Tests.Tests;
using System.Numerics;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Tests
{
	partial class Program
	{
		static void Main(string[] args)
		{
			RunHelper.CheckEnviroment();
			RunHelper.CheckRunModeAndRequestEnter();


			//Matrix4x4 m = MatrixSimd;
			//Vector3 vector = new Vector3(1, 2, 3);
			//vector = Vector3.Transform(vector, m);

			// TODO: uncoment necessary test.
			//BoolToIntConvertion();
			//RunLazyTest();
			//new FloatSummTest().Vector3ValueTypeSum();


			//new CallTest();
			//Console.WriteLine("End of test.");
			//Console.WriteLine("Press any key for exit.");
			//Console.ReadLine();
			//
			//return;
			// NEW:
			BenchmarkRunner.Run<
				// NO results
				//StructInitTest
				//IntSumTest

				// MEASURED:
				//StructPassingTests
				//OperationTests
				//CallTest
				//ComparationTest
				//FloatSummTest
				//DictionaryVsArrayTest
				//MatrixTest
				//ParralelExcutionTest
				//ReadonlyStructTest
				//SimdTest
				//StrucByIndexTest
				//VectorOperationsSimdTest
				//FloatSummTest
				//FastSqr
				//IndexerStructTest
				//BoolToIntConversionTest
				CheckForNulBeforeCall
				>();

			Console.WriteLine("End of test.");
			Console.WriteLine("Press any key for exit.");
			Console.ReadLine();
		}


		private static void BoolToIntConvertion()
		{
			//new BoolToIntConversionTest().Test1();
			//new BoolToIntConversionTest().Test2();
			//Console.WriteLine(new BoolToIntConversionTest().Test3());
			//return;
			BenchmarkRunner.Run<BoolToIntConversionTest>();
		}

		private static void RunLazyTest()
		{
			BenchmarkRunner.Run<LazyTest>(
				ManualConfig.Create(DefaultConfig.Instance).With(new Job(new RunMode
					{ InvocationCount = 2 * 8096 })));
			//			BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
			//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
			//Frequency = 2435947 Hz, Resolution = 410.5180 ns, Timer = TSC
			//	  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
			//  Job - NDUWLQ : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
			//
			//InvocationCount = 16192
			//
			//
			//			Method | Mean | StdDev | Allocated |
			//------------------ | ----------- | ---------- | ---------- |
			// RunWithDirectLazy | 65.2032 ns | 1.3368 ns | 64 B |
			//RunWithCondLazy | 34.5843 ns | 0.1106 ns | 0 B |
		}
	}
}
