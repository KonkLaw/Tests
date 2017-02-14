using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using Tests.Tests;

namespace Tests
{
	public class TestWithIssue
	{
		private int count;
		public float[] a;
		public float[] b;
		public float[] res1;

		public TestWithIssue()
		{
			count = 100000000;
			a = RandomHelper.GetNumbers(count);
			b = RandomHelper.GetNumbers(count);
			res1 = new float[count];
		}

		[Benchmark]
		public float[] Sum_slow()
		{
			for (int i = 0; i < count; i++)
			{
				res1[i] = a[i] + b[i];
			}
			return res1;
		}

		[Benchmark]
		public void Sum_fast()
		{
			for (int i = 0; i < count; i++)
			{
				res1[i] = a[i] + b[i];
			}
		}
	}

	class Program
    {
		

		static void Main(string[] args)
        {
			BenchmarkRunner.Run<TestWithIssue>();

			// TODO: uncoment necessary test.
			// ReadonlyStructRun();
			// RunArrays();

			// TODO: add cast test
			if (CheckEnviroment2())
            {
                CheckEnviroment();
				//BenchmarkRunner.Run<TestArrays>();

				//Stopwatch s = new Stopwatch();

				//var t1 = new TestArrays().Sum_fast();
				//GC.Collect();
				//GC.WaitForPendingFinalizers();

				//s.Restart();

				//t1 = new TestArrays().Sum_fast();

				//s.Stop();
				//Console.WriteLine(s.ElapsedMilliseconds);

				// check wtf
				// IComparable.Equals(1, new object());

				//var t2 = new TestArrays().Sum_slow();
				//GC.Collect();
				//GC.WaitForPendingFinalizers();
				//s.Restart();

				//t2 = new TestArrays().Sum_slow();

				//s.Stop();
				//Console.WriteLine(s.ElapsedMilliseconds);

				//Console.WriteLine(t1.Length + t2.Length);


				//TestArrays.Run();
				//TestInt.Run();
				//TestFloat.Run();
				//TestSimdPrepared.Run();
			}
			Console.WriteLine("End of test.");
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

        private static void RunArrays()
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

        private static bool CheckEnviroment2()
        {
//            bool is64 = Environment.Is64BitProcess;
//            if (!is64)
//            {
//                Console.WriteLine("Use 64 bit process.");
//                return false;
//            }
//            bool isDebug = false;
//#if (DEBUG)
//            isDebug = true;
//#endif
            //if (isDebug)
            //{
            //    Console.WriteLine("Build in 'Release' mode.");
            //    return false;
            //}

            //if (Debugger.IsAttached)
            //{
            //    Console.WriteLine("Debuger is attached.");
            //    return false;
            //}

            //if (!Vector.IsHardwareAccelerated)
            //{
            //    Console.WriteLine("Hardware acceleration is dissabled or not supported.");
            //    return false;
            //}
            return true;
        }

        private static void CheckEnviroment()
        {
            //int sizeOfRegister = Vector<int>.Count;
            //Console.WriteLine(
            //    string.Format("Register size = {0} int(float) numbers = {1} bytes = {2} bits",
            //        sizeOfRegister,
            //        sizeOfRegister * sizeof(int),
            //        sizeOfRegister * sizeof(int) * 8));
        }
    }
}
