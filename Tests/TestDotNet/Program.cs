using BenchmarkDotNet.Running;
using TestDotNet.Tests;
using TestDotNet.Utils;

RunHelper.CheckEnviroment();
RunHelper.CheckRunModeAndRequestEnter();

BenchmarkRunner.Run<TypeCast>();

Console.WriteLine("End of test.");
Console.WriteLine("Press any key for exit.");
Console.ReadLine();