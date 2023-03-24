using BenchmarkDotNet.Running;
using TestDotNet.Tests.GeneralCodeExecution;
using TestDotNet.Utils;


//new IntrinsicTest().Sse();

RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();

BenchmarkRunner.Run<SetterVsRef>();

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();