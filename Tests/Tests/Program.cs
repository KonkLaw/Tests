using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadonlyStructTest.RunTests();



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
