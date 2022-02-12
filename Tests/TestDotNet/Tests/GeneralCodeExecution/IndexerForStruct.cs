using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TestDotNet.Utils;

namespace TestDotNet.Tests.GeneralCodeExecution;

//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
//Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//.NET SDK= 6.0.101
// [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
//  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
//|         Method |      Mean |     Error |    StdDev |
//|--------------- |----------:|----------:|----------:|
//|   Get_RawIndex | 0.2818 ns | 0.0102 ns | 0.0090 ns |    = + 0
//|     GetByArray | 0.7447 ns | 0.0099 ns | 0.0093 ns |    = + 0.4629 =  66%
//| GetByBranching | 0.9801 ns | 0.0247 ns | 0.0232 ns |    = + 0.6983 = 100%
//|   GetByPointer | 0.8749 ns | 0.0237 ns | 0.0210 ns |    = + 0.5931 =  85%



//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
//Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//.NET SDK= 6.0.101

// [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
//  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


//|         Method |      Mean |     Error |    StdDev |
//|--------------- |----------:|----------:|----------:|
//|   Get_RawIndex | 0.2887 ns | 0.0146 ns | 0.0137 ns |
//|     GetByArray | 0.7219 ns | 0.0145 ns | 0.0136 ns |
//| GetByBranching | 0.8096 ns | 0.0236 ns | 0.0197 ns |
//|   GetByPointer | 0.8190 ns | 0.0238 ns | 0.0223 ns |
//| GetByArraySafe | 0.8192 ns | 0.0158 ns | 0.0148 ns |


[StructLayout(LayoutKind.Explicit)]
public unsafe struct ArrayIndexer
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index > 2)
                throw new IndexOutOfRangeException();
            return buffer[index];
        }
    }
}

[StructLayout(LayoutKind.Explicit)]
public struct SafeArrayIndexer
{
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            unsafe
            {
                fixed(void* ptr = &this)
                { 
                    return (*(ArrayIndexer*)ptr)[index];
                }
            }
        }
    }
}

[StructLayout(LayoutKind.Explicit)]
public struct BranchingIndexer
{
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                throw new IndexOutOfRangeException();
            }
        }
    }
}

[StructLayout(LayoutKind.Explicit)]
public struct PointerIndexer
{
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public unsafe float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index > 2)
                throw new IndexOutOfRangeException();
            fixed (void* ptr = &this)
            {
                return *((float*)ptr + index);
            }
        }
    }
}



public class IndexerForStruct
{
    private const int Size = 1024 - 1;
    private readonly int[] data;
    private int counter;

    private ArrayIndexer s1;
    private BranchingIndexer s2;
    private PointerIndexer s3;
    private SafeArrayIndexer s4;

    public IndexerForStruct()
    {
        data = RandomHelper.GetIntNumbers(Size + 1, 0, 3);
        s1 = new ArrayIndexer { X = 1, Y = 2, Z = 3 };
        s2 = new BranchingIndexer { X = 1, Y = 2, Z = 3 };
        s3 = new PointerIndexer { X = 1, Y = 2, Z = 3 };
        s4 = new SafeArrayIndexer { X = 1, Y = 2, Z = 3 };
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
    public float GetByPointer()
    {
        counter++;
        counter = counter & Size;
        return s3[data[counter]];
    }
    
    [Benchmark]
    public float GetByArraySafe()
    {
        counter++;
        counter = counter & Size;
        return s4[data[counter]];
    }
}
