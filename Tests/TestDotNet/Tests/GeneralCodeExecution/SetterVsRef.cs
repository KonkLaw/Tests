using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;

namespace TestDotNet.Tests.GeneralCodeExecution;

public class SetterVsRef
{
    public const int Count = 1000;
    private Vector3F_Ref[] arr_ref = new Vector3F_Ref[Count];
    private Vector3F_NonRef[] arr_nonref = new Vector3F_NonRef[Count];
    public readonly Random random = new Random();

    private int counter = 0;

    [Benchmark]
    public float TestRef_Get()
    {
        return new Vector3F_Ref()[random.Next(0, 3)];
    }

    [Benchmark]
    public float TestNonRef_Get()
    {
        return new Vector3F_Ref()[random.Next(0, 3)];
    }

    [Benchmark]
    public void TestRef_Set()
    {
        arr_ref[counter][random.Next(0, 3)] = 345f;
        counter++;
        counter %= Count;
    }

    [Benchmark]
    public void TestNonRef_Set()
    {
        arr_nonref[counter][random.Next(0, 3)] = 345345f;
        counter++;
        counter %= Count;
    }
}

[StructLayout(LayoutKind.Explicit, Size = 12)]
unsafe struct Vector3F_NonRef
{
    [FieldOffset(0)]
    private fixed float fields[3];
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
            if ((uint)index > 2)
                throw new InvalidOperationException();
            return fields[index];
        }
        set
        {
            if ((uint)index > 2)
                throw new InvalidOperationException();
            fields[index] = value;
        }
    }
}


[StructLayout(LayoutKind.Explicit, Size = 12)]
unsafe struct Vector3F_Ref
{
    [FieldOffset(0)]
    private fixed float fields[3];
    [FieldOffset(0)]
    public float X;
    [FieldOffset(4)]
    public float Y;
    [FieldOffset(8)]
    public float Z;

    public ref float this[int index]
    {
        get
        {
            if ((uint)index > 2)
                throw new InvalidOperationException();
            return ref fields[index];
        }
    }
}