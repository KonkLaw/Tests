using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using TestDotNet.Tests.MultithreadingTest;
using TestDotNet.Utils;


// ========================================

ManualConfig? config = null;

bool fastRun = false;
if (fastRun)
    config = ManualConfig.Create(DefaultConfig.Instance)
        .AddJob(Job.Default
            .WithWarmupCount(3)
            .WithIterationTime(200 * TimeInterval.Millisecond) // ms per iteration (bunch of operation)
            .WithMaxIterationCount(16)
            //.WithIterationCount(1)
            );


// output
config = (config ?? DefaultConfig.Instance)
    .WithSummaryStyle(
        DefaultConfig.Instance.SummaryStyle.WithMaxParameterColumnWidth(50));

// ========================================


RunLoad();

RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();


BenchmarkRunner.Run<BoundsSearchTest>(config);


// ========================================

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();



void RunLoad()
{
	Task.Run(() =>
	{
		// See https://aka.ms/new-console-template for more information

		ConsoleColor oldColor = Console.BackgroundColor;
		Console.BackgroundColor = ConsoleColor.Red;
		Console.WriteLine("Run additional load");
		Console.BackgroundColor = oldColor;

		double res = 0;
		while (true)
		{
			double sin = Math.Sin(45);
			if (sin > 5)
				break;
			res += sin;
		}

		throw new InvalidOperationException();
	});
}