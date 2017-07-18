using BenchmarkDotNet.Attributes;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

//BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 2 (10.0.15063)
//Processor=Intel Core i5-3210M CPU 2.50GHz(Ivy Bridge), ProcessorCount=4
//Frequency=2435880 Hz, Resolution=410.5293 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2101.1
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2101.1
//
//
//               Method |       Mean |     Error |    StdDev |
//--------------------- |-----------:|----------:|----------:|
//           DirectCall |   1.306 ns | 0.0621 ns | 0.0519 ns |
//        InterfaceCall |   2.141 ns | 0.0062 ns | 0.0089 ns |
//    UsualDelegateCall |   1.316 ns | 0.0405 ns | 0.0379 ns |
// ReflectionCachedCall | 195.485 ns | 3.2107 ns | 2.6811 ns |

namespace Tests.Tests
{
	interface IFakeInterface
	{
		void MethodForCall();
	}

	public class CallTest : IFakeInterface
	{
		private readonly Action action;
		private readonly MethodInfo methodInfo;
		private readonly IFakeInterface interfaceRef;

		private int counter;

		public CallTest()
		{
			action = MethodForCall;
			methodInfo = GetType().GetMethod(nameof(MethodForCall));
			interfaceRef = this;
		}

		[Benchmark]
		public void DirectCall()
		{
			MethodForCall();
		}

		[Benchmark]
		public void InterfaceCall()
		{
			interfaceRef.MethodForCall();
		}

		[Benchmark]
		public void UsualDelegateCall()
		{
			action();
		}

		[Benchmark]
		public void ReflectionCachedCall()
		{
			methodInfo.Invoke(this, null);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void MethodForCall()
		{
			counter++;
		}
	}
}
