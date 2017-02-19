using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;
using System.Numerics;
using Tests.Tests;

namespace Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			CheckEnviroment();

			// TODO: uncoment necessary test.
			//RunArraysTest();
			//ReadonlyStructRun();
			RunSimdTest();

			Console.WriteLine("End of test.");
			Console.WriteLine("Press any key.");
			Console.ReadLine();
		}

		private static void ReadonlyStructRun()
		{
			BenchmarkRunner.Run<ReadonlyStructTest>();
			//BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
			//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
			//Frequency = 2435957 Hz, Resolution = 410.5163 ns, Timer = TSC
			//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
			//DefaultJob: Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
			//            Method | Mean | StdDev |
			//  ---------------- | ----------- | ---------- |
			//   NonReadonlyCall | 1.3527 ns | 0.0431 ns |
			//     ReadonlyCall | 10.3586 ns | 0.1511 ns |
		}

		private static void RunArraysTest()
		{
			BenchmarkRunner.Run<TestArraysBoundsCheck>();
			//BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
			//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
			//Frequency = 2435957 Hz, Resolution = 410.5163 ns, Timer = TSC
			//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
			//DefaultJob: Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
			//    Method | Mean | StdDev |
			// --------- | ------------ | ----------- |
			//  Sum_slow | 202.3664 ms | 15.7631 ms |
			//  Sum_fast | 151.1016 ms | 0.2391 ms |
		}

		private static void RunSimdTest()
		{
			BenchmarkRunner.Run<VectorSumTest>();
		}

		private static void CheckEnviroment()
		{
			bool is64 = Environment.Is64BitProcess;
			if (!is64)
			{
				EndWithMessage("Use 64 bit process.");
			}
			bool isDebug = false;
			#if (DEBUG)
			    isDebug = true;
			#endif
			if (isDebug)
			{
				EndWithMessage("Build in 'Release' mode.");
			}

			if (Debugger.IsAttached)
			{
				EndWithMessage("Debuger is attached.");
			}

			if (!Vector.IsHardwareAccelerated)
			{
				EndWithMessage("Hardware acceleration is dissabled or not supported.");
			}

			int sizeOfRegister = Vector<float>.Count;
			if (sizeOfRegister != 4)
			{
				EndWithMessage("Wrong register size. Test should be corrected.");
			}
			Console.WriteLine(string.Format("Register size = {0} int(float) numbers = {1} bytes = {2} bits",
					sizeOfRegister,
					sizeOfRegister * sizeof(int),
					sizeOfRegister * sizeof(int) * 8));
		}

		private static void EndWithMessage(string message)
		{
			throw new Exception(message);
		}
	}
}
