using BenchmarkDotNet.Running;
using TestDotNet.Tests.GeneralCodeExecution;
using TestDotNet.Tests.VectorOperations;
using TestDotNet.Utils;


//new IntrinsicTest().Sse();

RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();

BenchmarkRunner.Run<General>();

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();