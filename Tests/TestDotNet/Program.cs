using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using TestDotNet.Tests;
using TestDotNet.Tests.Memory;
using TestDotNet.Utils;

// ========================================

ManualConfig? config = null;

//bool fastRun = true;
bool fastRun = false;
if (fastRun)
{
    config = ManualConfig.Create(DefaultConfig.Instance)
        .AddJob(Job.Default
            .WithWarmupCount(3)
            .WithIterationTime(200 * TimeInterval.Millisecond) // ms per iteration (bunch of operation)
            .WithMaxIterationCount(16)
            //.WithIterationCount(1)
            );
    var color = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Fast run enabled");
    Console.ForegroundColor = color;
}


// output
config = (config ?? DefaultConfig.Instance)
    .WithSummaryStyle(
        DefaultConfig.Instance.SummaryStyle.WithMaxParameterColumnWidth(50));

// ========================================

RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();

BenchmarkRunner.Run<AllocationTest>(config);


// ========================================

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();