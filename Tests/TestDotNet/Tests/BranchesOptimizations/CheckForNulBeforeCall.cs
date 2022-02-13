//Top of the script
#pragma warning disable 0649

using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;



//BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//Processor=Intel Core i7-8750H CPU 2.20GHz, ProcessorCount=12
//Frequency=2156249 Hz, Resolution=463.7683 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3362.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3362.0
//
//
//               Method |      Mean |     Error |    StdDev |
//--------------------- |----------:|----------:|----------:|
//    ReadonlyNotCalled | 0.2242 ns | 0.0021 ns | 0.0019 ns |
// NonReadonlyNotCalled | 0.2198 ns | 0.0018 ns | 0.0014 ns |
//             Readonly | 1.5002 ns | 0.0040 ns | 0.0033 ns |
//          NonReadonly | 1.5087 ns | 0.0201 ns | 0.0188 ns |
//                Empty | 0.0000 ns | 0.0000 ns | 0.0000 ns |


namespace TestDotNet.Tests.BranchesOptimizations;


#pragma warning disable IDE1005 // Delegate invocation can be simplified.

#pragma warning disable IDE0079

public class CheckForNulBeforeCall
{
    class SomeClass { }
#pragma warning disable CS0648 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private readonly Action<SomeClass>? readonlyActionNoAction;
#pragma warning restore CS0648

#pragma warning disable CS0648 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private readonly Action<SomeClass>? actionNoAction;
#pragma warning restore CS0648
    private readonly Action<SomeClass> readonlyAction;
    private readonly Action<SomeClass> action;
    private readonly SomeClass ptr;

    public CheckForNulBeforeCall()
    {
        ptr = new SomeClass();
        readonlyAction = EmptyAction;
        action = EmptyAction;
    }

    [Benchmark]
    public void ReadonlyNotCalled()
    {
        if (readonlyActionNoAction != null)
            readonlyActionNoAction(ptr);
    }

    [Benchmark]
    public void NonReadonlyNotCalled()
    {
        if (actionNoAction != null)
            action(ptr);
    }

    [Benchmark]
    public void Readonly()
    {
        if (readonlyAction != null)
            readonlyAction(ptr);
    }

    [Benchmark]
    public void NonReadonly()
    {
        if (action != null)
            action(ptr);
    }

    [Benchmark]
#pragma warning disable CA1822 // Mark members as static
    public void Empty()
#pragma warning restore CA1822 // Mark members as static
    {

    }

    private void EmptyAction(SomeClass h)
    {

    }
}


#pragma warning restore IDE1005 // Delegate invocation can be simplified.
#pragma warning restore IDE0079