using BenchmarkDotNet.Attributes;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using TestDotNet.Utils;

//BenchmarkDotNet = v0.13.1, OS = Windows 10.0.19043.1526(21H1 / May2021Update)
//Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//.NET SDK=6.0.101
//  [Host]     : .NET 6.0.1(6.0.121.56705), X64 RyuJIT
//  DefaultJob : .NET 6.0.1(6.0.121.56705), X64 RyuJIT


//|                  Method |      Mean |     Error |    StdDev |
//|------------------------ |----------:| ----------:| ----------:|
//| Indexer                 | 0.9562 ns | 0.0085 ns | 0.0079 ns |
//| NoSimd_Ref_Safe         | 1.7632 ns | 0.0165 ns | 0.0129 ns |
//| NoSimd_NoRef_Safe       | 1.9171 ns | 0.0112 ns | 0.0087 ns |
//| Simd_Safe_Ref_Fixed     | 2.1410 ns | 0.0119 ns | 0.0099 ns |
//| Simd_Safe_NoRef_NoFixed | 7.9380 ns | 0.0472 ns | 0.0442 ns |
//| NoSimd_Ref_UnSafe       | 5.8196 ns | 0.0341 ns | 0.0319 ns |
//| NoSimd_NoRef_UnSafe     | 6.7842 ns | 0.0301 ns | 0.0281 ns |
//| Simd_UnSafe             | 8.6305 ns | 0.0527 ns | 0.0493 ns |
//| Simd_NumericBased_Safe_ | 2.5487 ns | 0.0259 ns | 0.0243 ns |


namespace TestDotNet.Tests.VectorOperations;

[StructLayout(LayoutKind.Explicit)]
public struct Vector3Safe
{
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public static Vector3Safe Add_Ref(ref Vector3Safe left, ref Vector3Safe right)
    {
        return new Vector3Safe
        {
            X = left.X + right.X,
            Y = left.Y + right.Y,
            Z = left.Z + right.Z
        };
    }

    public static Vector3Safe Add_NoRef(Vector3Safe left, Vector3Safe right)
    {
        return new Vector3Safe
        {
            X = left.X + right.X,
            Y = left.Y + right.Y,
            Z = left.Z + right.Z
        };
    }

    public static Vector3Safe AddSimd_Ref_Fixed(ref Vector3Safe a, ref Vector3Safe b)
    {
        unsafe
        {
            fixed (void* aPtr = &a)
            {
                fixed (void* bPtr = &b)
                {
                    Vector128<float> vA = System.Runtime.Intrinsics.X86.Sse.LoadVector128((float*)aPtr);
                    Vector128<float> vB = System.Runtime.Intrinsics.X86.Sse.LoadVector128((float*)bPtr);
                    Vector128<float> resv = System.Runtime.Intrinsics.X86.Sse.Add(vA, vB);
                    Vector3Safe result;
                    void* resPtr = &result;
                    System.Runtime.Intrinsics.X86.Sse.Store((float*)resPtr, resv);
                    return result;
                }
            }
        }
    }

    public static Vector3Safe AddSimd_NoRef_NoFixed(Vector3Safe a, Vector3Safe b)
    {
        unsafe
        {
            void* aPtr = &a;
            void * bPtr = &b;
            Vector128<float> vA = System.Runtime.Intrinsics.X86.Sse.LoadVector128((float*)aPtr);
            Vector128<float> vB = System.Runtime.Intrinsics.X86.Sse.LoadVector128((float*)bPtr);
            Vector128<float> resv = System.Runtime.Intrinsics.X86.Sse.Add(vA, vB);
            Vector3Safe result;
            void* resPtr = &result;
            System.Runtime.Intrinsics.X86.Sse.Store((float*)resPtr, resv);
            return result;
        }
    }
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct Vector3Unsafe
{
    [FieldOffset(0)]
    public fixed float arr[3];

    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public static Vector3Unsafe Add_Ref(ref Vector3Unsafe left, ref Vector3Unsafe right)
    {
        return new Vector3Unsafe
        {
            X = left.X + right.X,
            Y = left.Y + right.Y,
            Z = left.Z + right.Z
        };
    }

    public static Vector3Unsafe Add_NoRef(Vector3Unsafe left, Vector3Unsafe right)
    {
        return new Vector3Unsafe
        {
            X = left.X + right.X,
            Y = left.Y + right.Y,
            Z = left.Z + right.Z
        };
    }

    public static Vector3Unsafe AddSimdNoRef(Vector3Unsafe a, Vector3Unsafe b)
    {
        Vector128<float> vA = System.Runtime.Intrinsics.X86.Sse.LoadVector128(a.arr);
        Vector128<float> vB = System.Runtime.Intrinsics.X86.Sse.LoadVector128(b.arr);
        Vector128<float> resv = System.Runtime.Intrinsics.X86.Sse.Add(vA, vB);
        Vector3Unsafe result = default;
        System.Runtime.Intrinsics.X86.Sse.Store(result.arr, resv);
        return result;
    }
}

public class IntrinsicTest
{
    private const int Count = 1024;
    private const int Mask = 1024 - 1;

    private int counter;

    private readonly Vector3Safe[] aS;
    private readonly Vector3Safe[] bS;

    private readonly Vector3Unsafe[] aU;
    private readonly Vector3Unsafe[] bU;

    public IntrinsicTest()
    {
        aS = new Vector3Safe[Count];
        bS = new Vector3Safe[Count];
        aU = new Vector3Unsafe[Count];
        bU = new Vector3Unsafe[Count];

        for (int i = 0; i < Count; i++)
        {
            aS[i] = new Vector3Safe
            {
                X = RandomHelper.GetFloat(),
                Y = RandomHelper.GetFloat(),
                Z = RandomHelper.GetFloat()
            };
            bS[i] = new Vector3Safe
            {
                X = RandomHelper.GetFloat(),
                Y = RandomHelper.GetFloat(),
                Z = RandomHelper.GetFloat()
            };

            aU[i] = new Vector3Unsafe
            {
                X = RandomHelper.GetFloat(),
                Y = RandomHelper.GetFloat(),
                Z = RandomHelper.GetFloat()
            };
            bU[i] = new Vector3Unsafe
            {
                X = RandomHelper.GetFloat(),
                Y = RandomHelper.GetFloat(),
                Z = RandomHelper.GetFloat()
            };
        }
    }

    [Benchmark]
    public Vector3Safe Indexer()
    {
        counter++;
        counter &= Mask;

        counter++;
        counter &= Mask;
        return aS[counter];
    }

    [Benchmark]
    public Vector3Safe NoSimd_Ref_Safe()
    {
        counter++;
        counter &= Mask;
        return Vector3Safe.Add_Ref(ref aS[counter], ref bS[counter]);
    }

    [Benchmark]
    public Vector3Safe NoSimd_NoRef_Safe()
    {
        counter++;
        counter &= Mask;
        return Vector3Safe.Add_NoRef(aS[counter], bS[counter]);
    }

    [Benchmark]
    public Vector3Safe Simd_Safe_Ref_Fixed()
    {
        counter++;
        counter &= Mask;
        return Vector3Safe.AddSimd_Ref_Fixed(ref aS[counter], ref bS[counter]);
    }

    [Benchmark]
    public Vector3Safe Simd_Safe_NoRef_NoFixed()
    {
        counter++;
        counter &= Mask;
        return Vector3Safe.AddSimd_NoRef_NoFixed(aS[counter], bS[counter]);
    }

    [Benchmark]
    public Vector3Unsafe NoSimd_Ref_UnSafe()
    {
        counter++;
        counter &= Mask;
        return Vector3Unsafe.Add_Ref(ref aU[counter], ref bU[counter]);
    }

    [Benchmark]
    public Vector3Unsafe NoSimd_NoRef_UnSafe()
    {
        counter++;
        counter &= Mask;
        return Vector3Unsafe.Add_NoRef(aU[counter], bU[counter]);
    }

    [Benchmark]
    public Vector3Unsafe Simd_UnSafe()
    {
        counter++;
        counter &= Mask;
        return Vector3Unsafe.AddSimdNoRef(aU[counter], bU[counter]);
    }

    [Benchmark]
    public unsafe Vector3Safe Simd_NumericBased_Safe_()
    {
        counter++;
        counter &= Mask;

        fixed (void* aPtr = &aS[counter])
        {
            fixed (void* bPtr = &bS[counter])
            {
                Vector3 av = *(Vector3*)aPtr;
                Vector3 bv = *(Vector3*)bPtr;
                Vector3 aRes = av + bv;
                return *(Vector3Safe*)&aRes;
            }
        }
    }
}