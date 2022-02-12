//BenchmarkDotNet = v0.12.1, OS = Windows 10.0.18363.1379(1909 / November2018Update / 19H2)
//Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//  [Host]     : .NET Framework 4.8 (4.8.4300.0), X64 RyuJIT
//  DefaultJob : .NET Framework 4.8 (4.8.4300.0), X64 RyuJIT

//|                              Method |      Mean |     Error |    StdDev |
//|------------------------------------ |----------:| ----------:| ----------:|
//| Increment_Inline                    | 0.0309 ns | 0.0014 ns | 0.0013 ns |
//| Increment_NoInline                  | 0.7355 ns | 0.0014 ns | 0.0012 ns |

//| Increment_Cast_Inline               | 0.4915 ns | 0.0018 ns | 0.0014 ns |
//| Increment_Cast_NoInline             | 1.2010 ns | 0.0021 ns | 0.0018 ns |

//| Increment_As_Inline                 | 0.9357 ns | 0.0020 ns | 0.0018 ns |
//| Increment_As_NoInline               | 1.9061 ns | 0.0229 ns | 0.0214 ns |

//| VirtCall                            | 0.5164 ns | 0.0042 ns | 0.0037 ns |
//| Virt_Increament                     | 0.5176 ns | 0.0022 ns | 0.0018 ns |

//| Increment_StructNonGeneric_Inline   | 0.1288 ns | 0.0063 ns | 0.0052 ns |
//| Increment_StructNonGeneric_NoInline | 1.1217 ns | 0.0107 ns | 0.0095 ns |


using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TestDotNet.Tests;

public class TypeCast
{
    private const int RunCount = 1_000_000;
    public BaseClass inst = new DerClass();

    [Benchmark]
    public void Increment_Inline()
    {
        inst.IncrimentInline();
    }

    [Benchmark]
    public void Increment_NoInline()
    {
        inst.IncrimentNoInline();
    }

    [Benchmark]
    public void Increment_Cast_Inline()
    {
        ((DerClass)inst).IncrimentInlineDer();
    }

    [Benchmark]
    public void Increment_Cast_NoInline()
    {
        ((DerClass)inst).IncrimentNoInlineDer();
    }

    [Benchmark]
    public void Increment_As_Inline()
    {
        (inst as DerClass).IncrimentInlineDer();
    }

    [Benchmark]
    public void Increment_As_NoInline()
    {
        (inst as DerClass).IncrimentNoInlineDer();
    }

    [Benchmark]
    public void VirtCall()
    {
        inst.DoNothing();
    }

    [Benchmark]
    public void Virt_Increament()
    {
        inst.IncrimentVirt();
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //private static T2 Help<T1, T2>(T1 inst)
    //    where T1 : BaseClass
    //    where T2 : BaseClass
    //{
    //    var m = new Mapper<T1, T2>();
    //    m.Base = inst;
    //    return m.Der;
    //}

    [Benchmark]
    public void Increment_StructNonGeneric_Inline()
    {
        Mapper2 m = new Mapper2
        {
            Base = inst
        };
        m.Der.IncrimentInlineDer();
    }

    [Benchmark]
    public void Increment_StructNonGeneric_NoInline()
    {
        Mapper2 m = new Mapper2();
        m.Base = inst;
        m.Der.IncrimentNoInlineDer();
    }

    //[Benchmark]
    //public void Increment_Struct_Generic_Inline()
    //{
    //    Help<BaseClass, DerClass>(inst).IncrimentInlineDer();
    //}
    //
    //[Benchmark]
    //public void Increment_Struct_Generic_NoInline()
    //{
    //    Help<BaseClass, DerClass>(inst).IncrimentNoInlineDer();
    //}




    public class BaseClass
    {
        protected int coutner;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void IncrimentNoInline() => coutner++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncrimentInline() => coutner++;

        public virtual void DoNothing() { }

        public virtual void IncrimentVirt() => coutner++;
    }

    public class DerClass : BaseClass
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void IncrimentNoInlineDer() => coutner++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncrimentInlineDer() => coutner++;

        public override void DoNothing() { }

        public override void IncrimentVirt() => coutner++;
    }

    public class Der2Class : BaseClass
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void IncrimentNoInlineDer2() => coutner++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncrimentInlineDer2() => coutner++;

        public override void DoNothing() { }

        public override void IncrimentVirt() => coutner++;
    }







    //[StructLayout(LayoutKind.Explicit)]
    //unsafe struct Mapper<T1, T2>
    //    where T1 : BaseClass
    //    where T2 : BaseClass
    //{
    //    [FieldOffset(0)]
    //    public T1 Base;
    //
    //    [FieldOffset(0)]
    //    public T2 Der;
    //}

    [StructLayout(LayoutKind.Explicit)]
    struct Mapper2
    {
        [FieldOffset(0)]
        public BaseClass Base;

        [FieldOffset(0)]
        public DerClass Der;
    }
}
