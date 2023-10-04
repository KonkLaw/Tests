using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using TestDotNet.Tests.MultithreadingTest;
using TestDotNet.Utils;


RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();

// ========================================

ManualConfig? config = null;

bool fastRun = false;
if (fastRun)
    config = ManualConfig.Create(DefaultConfig.Instance)
        .AddJob(Job.Default
            .WithWarmupCount(3)
            .WithIterationTime(200 * TimeInterval.Millisecond) // ms per iteration (bunch of operation)
            .WithMaxIterationCount(16)
            //.WithIterationCount(3))
            );


// output
config = (config ?? DefaultConfig.Instance)
    .WithSummaryStyle(
        DefaultConfig.Instance.SummaryStyle.WithMaxParameterColumnWidth(50));

// ========================================


BenchmarkRunner.Run<MinMaxComputationTest>(config);



// ========================================

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();