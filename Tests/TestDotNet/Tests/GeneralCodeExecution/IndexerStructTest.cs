using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;


//	BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//	Processor=Intel Core i7-8700K CPU 3.70GHz, ProcessorCount=12
//	Frequency=3609376 Hz, Resolution=277.0562 ns, Timer=TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3260.0
//DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3260.0
//
//
//				   Method |      Mean |     Error |    StdDev |
//----------------------- |----------:|----------:|----------:|
//			 Get_RawIndex | 0.2332 ns | 0.0057 ns | 0.0053 ns |
//			   GetByArray | 0.4547 ns | 0.0146 ns | 0.0136 ns |
//		   GetByBranching | 1.1424 ns | 0.0088 ns | 0.0068 ns |
//		GetBySafeAndArray | 0.6857 ns | 0.0096 ns | 0.0090 ns |
// GetBySafeAndConversion | 0.6885 ns | 0.0085 ns | 0.0080 ns |

//====================================================================
//	BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//	Processor=Intel Core i7-8700K CPU 3.70GHz, ProcessorCount=12
//	Frequency=3609376 Hz, Resolution=277.0562 ns, Timer=TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3260.0
//DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3260.0
//
//
//					 Method |      Mean |     Error |    StdDev |
//	----------------------- |----------:|----------:|----------:|
//			   Get_RawIndex | 0.2246 ns | 0.0055 ns | 0.0052 ns |
//				 GetByArray | 0.4594 ns | 0.0078 ns | 0.0073 ns |
//			 GetByBranching | 1.6518 ns | 0.0179 ns | 0.0159 ns |
//		  GetBySafeAndArray | 0.6799 ns | 0.0099 ns | 0.0093 ns |
//   GetBySafeAndConversion | 0.6840 ns | 0.0078 ns | 0.0069 ns |




//	BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//	Processor=Intel Core i7-8700K CPU 3.70GHz, ProcessorCount=12
//	Frequency=3609376 Hz, Resolution=277.0562 ns, Timer=TSC
//[Host]     : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.3260.0
//DefaultJob : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.3260.0
//
//
//					Method |     Mean |     Error |    StdDev |
// ----------------------- |---------:|----------:|----------:|
//			  Get_RawIndex | 1.234 ns | 0.0105 ns | 0.0093 ns |
//				GetByArray | 1.582 ns | 0.0130 ns | 0.0121 ns |
//			GetByBranching | 1.602 ns | 0.0079 ns | 0.0070 ns |
//		 GetBySafeAndArray | 1.724 ns | 0.0101 ns | 0.0094 ns |
//  GetBySafeAndConversion | 1.928 ns | 0.0155 ns | 0.0145 ns |

namespace TestDotNet.Tests.GeneralCodeExecution;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct TestIndexerStrutWithBuffer
{
    [FieldOffset(0)]
    public fixed float buffer[3];

    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public float this[int index]
    {
        get { return buffer[index]; }
    }
}

[StructLayout(LayoutKind.Explicit)]
public struct TestIndexerStrutWithBufferSafeByArray
{
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public float this[int index]
    {
        get
        {
            unsafe
            {
                fixed (TestIndexerStrutWithBufferSafeByArray* p = &this)
                {
                    var data = (float*)p;
                    return data[index];
                }
            }
        }
    }
}

[StructLayout(LayoutKind.Explicit)]
public struct TestIndexerStrutWithBufferSafeConversion
{
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public float this[int index]
    {
        get
        {
            unsafe
            {
                fixed (TestIndexerStrutWithBufferSafeConversion* p = &this)
                {
                    return (*(TestIndexerStrutWithBuffer*)p)[index];
                }
            }
        }
    }
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct TestIndexerStrutBranching
{
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public float this[int index]
    {
        get
        {
            if (index == 0)
                return X;
            else if (index == 1)
                return Y;
            else if (index == 2)
                return Z;
            else
            {
                throw new Exception();
            }
        }
    }
}

public class IndexerStructTest
{
    private int counter;
    private readonly int[] data;

    private TestIndexerStrutWithBuffer s1;
    private TestIndexerStrutBranching s2;
    private TestIndexerStrutWithBufferSafeByArray s3;
    private TestIndexerStrutWithBufferSafeConversion s4;

    private const int Size = 1024;

    public IndexerStructTest()
    {
        data = RandomHelper.GetIntNumbers(Size, 0, 3);
        counter = 0;
        s1 = new TestIndexerStrutWithBuffer();
        s2 = new TestIndexerStrutBranching();
        s3 = new TestIndexerStrutWithBufferSafeByArray();
        s4 = new TestIndexerStrutWithBufferSafeConversion();
    }

    [Benchmark]
    public int Get_RawIndex()
    {
        counter++;
        counter = counter & Size;
        return data[counter];
    }

    [Benchmark]
    public float GetByArray()
    {
        counter++;
        counter = counter & Size;
        return s1[data[counter]];
    }

    [Benchmark]
    public float GetByBranching()
    {
        counter++;
        counter = counter & Size;
        return s2[data[counter]];
    }

    [Benchmark]
    public float GetBySafeAndArray()
    {
        counter++;
        counter = counter & Size;
        return s3[data[counter]];
    }

    [Benchmark]
    public float GetBySafeAndConversion()
    {
        counter++;
        counter = counter & Size;
        return s4[data[counter]];
    }
}
