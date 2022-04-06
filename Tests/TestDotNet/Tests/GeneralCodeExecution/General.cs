using BenchmarkDotNet.Attributes;
using System.Linq.Expressions;

namespace TestDotNet.Tests.GeneralCodeExecution;


//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1586 (21H1/May2021Update)
//Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//.NET SDK= 6.0.201
// [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
//  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
//|           Method |           Mean |       Error |      StdDev |
//|----------------- |---------------:|------------:|------------:|
//| CreateExpression |    517.5797 ns |   1.5716 ns |   1.4700 ns |
//|    GetExpression |      1.5940 ns |   0.0103 ns |   0.0097 ns |
//|          GetName |      1.9134 ns |   0.0063 ns |   0.0059 ns |
//|    CompileCached | 39,562.3162 ns | 430.4176 ns | 359.4182 ns |
//|   CompileCreated | 42,035.0004 ns | 410.3830 ns | 383.8725 ns |
//|         CastTest |      0.1616 ns |   0.0037 ns |   0.0029 ns |

public class General
{
    private List<int> numbers = new List<int>();
    private Expression<Func<List<int>, int>> cachedExpression = list => list.Count;

    private object GetNumbers() => numbers;

    private Expression<Func<List<int>, int>> CraeteExpressionLambda()
    {
        return list => list.Count;
    }

    [Benchmark]
    public Expression<Func<List<int>, int>> CreateExpression()
    {
        return CraeteExpressionLambda();
    }

    [Benchmark]
    public MemberExpression GetExpression()
    {
        return (MemberExpression)cachedExpression.Body;
    }

    [Benchmark]
    public string GetName()
    {
        return GetExpression().Member.Name;
    }

    [Benchmark]
    public Func<List<int>, int> CompileCached()
    {
        return cachedExpression.Compile();
    }

    [Benchmark]
    public Func<List<int>, int> CompileCreated()
    {
        return CraeteExpressionLambda().Compile();
    }

    [Benchmark]
    public List<int> CastTest()
    {
        return (List<int>)GetNumbers();
    }
}
