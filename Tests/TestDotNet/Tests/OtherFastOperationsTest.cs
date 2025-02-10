using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using static TestDotNet.Tests.TypeCast;

namespace TestDotNet.Tests;

#pragma warning disable CS8602
public class OtherFastOperationsTest
{
    private int i;
    public TypeCast.BaseClass inst = new TypeCast.DerClass();

    private object obj = new object();

    private static object? root;

    private readonly Dictionary<int, int> dict = new();

    public WeakReference<TypeCast.DerClass> der;

    public OtherFastOperationsTest()
    {
        var derClass = new TypeCast.DerClass();
        root = derClass;
        der = new WeakReference<TypeCast.DerClass>(derClass);
    }

    [Benchmark]
    public void Increment()
    {
        i++;
    }

    [Benchmark]
    public void Virt_Increament()
    {
        inst.IncrimentVirt();
    }

    [Benchmark]
    public void PinUnpin()
    {
        GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
        handle.Free();
    }

    [Benchmark]
    public void WeakReffCall_Inline()
    {
        if (der.TryGetTarget(out TypeCast.DerClass? target))
        {
            target.IncrimentInlineDer();
        }
        else
            throw new InvalidOperationException();
    }

    [Benchmark]
    public void WeakReffCall_NoInline()
    {
        if (der.TryGetTarget(out TypeCast.DerClass? target))
        {
            target.IncrimentNoInlineDer();
        }
        else
            throw new InvalidOperationException();
    }

    [Benchmark]
    public void TestDictionary()
    {
        const int key = 123;
        dict.TryGetValue(key, out int val);
        dict[key] = 234;

        val = dict[key];
        dict.Remove(key);
    }
}