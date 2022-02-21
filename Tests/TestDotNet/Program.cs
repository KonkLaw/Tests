using BenchmarkDotNet.Running;
using TestDotNet.Tests.VectorOperations;
using TestDotNet.Utils;


//new IntrinsicTest().Sse();

RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();

BenchmarkRunner.Run<IntrinsicTest>();

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();