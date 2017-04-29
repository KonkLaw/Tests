using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Tests.Tests;

namespace Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			// TODO: buggy test.
			//BenchmarkRunner.Run<BuggyTest>();
			//return;

			ApproveReleaseWithoutDebugerStart();

			//CheckEnviroment();
			// TODO: uncoment necessary test.
			//RunStructInitTest();
			//RunArraysTest();
			//RunComparationTest();
			RunLazyTest();
			//ReadonlyStructRun();
			//RunSimdTest();
			//BenchmarkRunner.Run<IntSumTest>();

			Console.WriteLine("End of test.");
			Console.WriteLine("Press any key for exit.");
			Console.ReadLine();
		}

		private static void RunStructInitTest()
		{
			BenchmarkRunner.Run<StructInitTest>();
		}

		private static void RunLazyTest()
		{
			BenchmarkRunner.Run<LazyTest>(
				ManualConfig.Create(DefaultConfig.Instance).With(new Job(new RunMode
					{ InvocationCount = 2 * 8096 })));
		}

		private static void RunComparationTest()
		{
			//var test = new ComparationTest(); if (test.RunSigned() != test.RunUnsigned()) throw new Exception("ERROR.");
			BenchmarkRunner.Run<ComparationTest>();

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
			BenchmarkRunner.Run<ArraysBoundsCheckTest>();
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
			BenchmarkRunner.Run<FloatSummTest>();
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

		private static void ApproveReleaseWithoutDebugerStart()
		{
			bool isDebug = false;
#if (DEBUG)
			isDebug = true;
#endif
			if (isDebug || Debugger.IsAttached)
				return;

			Console.WriteLine("Press any key to start.");
			Console.ReadLine();
		}

		private static void EndWithMessage(string message)
		{
			throw new Exception(message);
		}
	}

	public class BuggyTest
	{
		private const int N = 10000;
		private readonly byte[] data;
		private readonly byte[] data2;

		private readonly SHA256 sha256 = SHA256.Create();
		private readonly MD5 md5 = MD5.Create();

		public BuggyTest()
		{
			data = new byte[N];
			new Random(42).NextBytes(data);

			data2 = new byte[N];
			new Random(42).NextBytes(data2);
		}

		[Benchmark]
		public byte[] Sha256()
		{
			return sha256.ComputeHash(data);
		}

		[Benchmark]
		public byte[] Md5()
		{
			return md5.ComputeHash(data);
		}

		//[Benchmark]
		[Benchmark]
		public byte[] Sha256__()
		{
			return sha256.ComputeHash(data2);
		}

		//[Benchmark]
		[Benchmark]
		public byte[] Md5__()
		{
			return md5.ComputeHash(data2);
		}
	}
}
