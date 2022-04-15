using BenchmarkDotNet.Attributes;
using System.Linq.Expressions;

namespace TestDotNet.Tests.GeneralCodeExecution;

//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1645 (21H1/May2021Update)
//Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//.NET SDK= 6.0.202
//|           Method |           Mean |       Error |      StdDev |
//|----------------- |---------------:|------------:|------------:|
//|  CreateFunction1 |      4.2774 ns |   0.1024 ns |   0.0958 ns |
//|   CreateFunctio2 |      0.7251 ns |   0.0290 ns |   0.0257 ns |
//| CreateExpression |    546.4990 ns |   2.9436 ns |   2.7535 ns |
//|    GetExpression |      1.5163 ns |   0.0065 ns |   0.0058 ns |
//|          GetName |      1.9016 ns |   0.0589 ns |   0.0492 ns |
//|    CompileCached | 41,277.5775 ns | 778.8589 ns | 728.5451 ns |
//|   CompileCreated | 43,656.7480 ns | 589.3296 ns | 551.2592 ns |
//|         CastTest |      0.4258 ns |   0.0509 ns |   0.0476 ns |

public class General
{
    private List<int> numbers = new List<int>();
    private Expression<Func<List<int>, int>> cachedExpression = list => list.Count;

    private object GetNumbers() => numbers;

    private int TestFunc(List<int> list) => list.Count;

    private Func<List<int>, int> CreateFunction_1() => TestFunc;

    private Func<List<int>, int> CreateFunction_2() => list => list.Count;

    [Benchmark]
    public Func<List<int>, int> CreateFunction1() => CreateFunction_1();

    [Benchmark]
    public Func<List<int>, int> CreateFunctio2() => CreateFunction_2();

    private Expression<Func<List<int>, int>> CraeteExpressionLambda_() => list => list.Count;

    [Benchmark]
    public Expression<Func<List<int>, int>> CreateExpression()
    {
        return CraeteExpressionLambda_();
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
        return CraeteExpressionLambda_().Compile();
    }

    [Benchmark]
    public List<int> CastTest()
    {
        return (List<int>)GetNumbers();
    }
}
