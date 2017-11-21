using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

//BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
//Frequency = 2435957 Hz, Resolution = 410.5163 ns, Timer = TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//DefaultJob: Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//            Method | Mean | StdDev |
//  ---------------- | ----------- | ---------- |
//   NonReadonlyCall | 1.3527 ns | 0.0431 ns |
//      ReadonlyCall | 10.3586 ns | 0.1511 ns |

namespace Tests.Tests
{
    public class ReadonlyStructTest
    {
        public BigStruct nonReadonlyField = new BigStruct();
        public readonly BigStruct readonlyField = new BigStruct();

        [Benchmark]
        public float NonReadonlyCall()
        {
            return nonReadonlyField.GetAvg();
        }

        [Benchmark]
        public float ReadonlyCall()
        {
            return readonlyField.GetAvg();
        }
    }

    public struct BigStruct
    {
        // Just big amount of some data.
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

        // NoInlining - realy important.
        [MethodImpl(MethodImplOptions.NoInlining)]
        public float GetAvg()
        {
            return m11;   
        }
    }
}
