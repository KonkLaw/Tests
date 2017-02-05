using System;
using System.Diagnostics;

namespace Tests
{
    struct BigStruct
    {
        public float m11;
        public float m12;
        public float m13;
        public float m21;
        public float m22;
        public float m23;
        public float m31;
        public float m32;
        public float m33;

        public BigStruct(bool foo)
        {
            m11 = 2.32f;
            m12 = 2342.32f;
            m13 = 22.32f;
            m21 = 232f;
            m22 = 242.32f;
            m23 = 22.342f;
            m31 = 244.32f;
            m32 = 23242.32f;
            m33 = 232.32f;
        }

        public float GetAvg()
        {
            return (m11 + m12 + m13 + m21 + m22 + m23 + m31 + m32 + m33) / 9;   
        }
    }

    class ReadOnlyTest
    {
        private readonly BigStruct val;

        public ReadOnlyTest()
        {
            val = new BigStruct(true);
        }

        public double RunLoop()
        {
            double sum = 0;
            for (int i = 0; i < ReadonlyStructTest.TestCount; i++)
            {
                sum = val.GetAvg();
            }
            return sum;
        }
    }

    class NonReadOnlyTest
    {
        private BigStruct val;

        public NonReadOnlyTest()
        {
            val = new BigStruct(true);
        }

        public double RunLoop()
        {
            double sum = 0;
            for (int i = 0; i < ReadonlyStructTest.TestCount; i++)
            {
                sum = val.GetAvg();
            }
            return sum;
        }
    }

    class ReadonlyStructTest
    {
        public const int TestCount = 10000000;

        internal static void RunTests()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Restart();
            var res1 = new ReadOnlyTest().RunLoop();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var res2 = new NonReadOnlyTest().RunLoop();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}
