using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace TestDotNet.Utils;

class ConfigHelper
{
    static IConfig GetConfig() => DefaultConfig.Instance
        .AddJob(Job.Default.WithRuntime(ClrRuntime.Net472))
        .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60));
}