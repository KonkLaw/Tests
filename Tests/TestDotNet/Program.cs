using BenchmarkDotNet.Running;
using TestDotNet.Utils;

RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();

BenchmarkRunner.Run<TestDotNet.Tests.GeneralCodeExecution.IndexerForStruct>();

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();