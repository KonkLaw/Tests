using BenchmarkDotNet.Attributes;

namespace TestDotNet.Utils;

public abstract class ComputeInfo<TParam>
{
    private readonly string name;

    private protected ComputeInfo(string name) => this.name = name;

    public abstract Action GetAction(TParam param);

    public override string ToString() => name;
}

public abstract class BaseTest<TParam>
{
    private readonly Collection<ComputeInfo<TParam>> collection;
    public IEnumerable<ComputeInfo<TParam>> ComputeInfos => collection;

    private Action action = null!;

    protected BaseTest()
    {
        collection = new Collection<ComputeInfo<TParam>>(GetComputeInfo());
    }

    protected static string GetName(Delegate del) => del.Method.Name;

    protected abstract IEnumerable<ComputeInfo<TParam>> GetComputeInfo();

    protected abstract TParam GetParam();

    [GlobalSetup]
    public void Setup()
    {
        action = collection.GetCurrentEnumerated().GetAction(GetParam());
    }

    [Benchmark]
    [ArgumentsSource(nameof(ComputeInfos))]
    public void Run(ComputeInfo<TParam> computeInfo) => action();
}