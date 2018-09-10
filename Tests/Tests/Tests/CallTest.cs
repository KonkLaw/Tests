using BenchmarkDotNet.Attributes;
using System;
using System.Linq.Expressions;
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
		int MethodForNoninlinedCall(int input);
	}

	public class CallTest : IFakeInterface
	{
		private readonly IFakeInterface interfaceRef;
		private readonly Func<CallTest, int> function;

		private readonly MethodInfo methodInfo;
		private readonly object[] arrayCache = new object[1];

		private readonly Func<CallTest, int> functionFromExpression;


		public int Asd = 123;

		object expression = (Expression<Func<CallTest, int>>)(i => i.Asd);
		private Func<CallTest, int> expr11;

		//private int counter;

		public CallTest()
		{
			interfaceRef = this;
			function = i => i.Asd;
			methodInfo = GetType().GetMethod(nameof(MethodForCall));
			functionFromExpression = ((Expression<Func<CallTest, int>>)expression).Compile();



			var methodInfo1 = typeof(MyClass).GetMethod(nameof(MyClass.MethodForCall11));
			ParameterExpression instance = Expression.Parameter(typeof(CallTest), "instance");
			MethodCallExpression value = Expression.Call(methodInfo1, instance);
			expr11 = Expression.Lambda<Func<CallTest, int>>(value, instance).Compile();

			//var input = Expression.Parameter(typeof(object), "input");
			//var method = o.GetType().GetMethod("GetName",
			//	System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			////you should check for null *and* make sure the return type is string here.
			//Assert.IsFalse(method == null && !method.ReturnType.Equals(typeof(string)));


			//now build a dynamic bit of code that does this:
			//(object o) => ((TestType)o).GetName();
		
		}

		//[Benchmark]
		public int DirectCallInlined()
		{
			return MethodForInlinedCall(48);
		}

		//[Benchmark]
		public int InterfaceCall()
		{
			return interfaceRef.MethodForNoninlinedCall(48);
		}

		[Benchmark]
		public int UsualDelegateCall()
		{
			return function(this);
		}

		[Benchmark]
		public int ReflectionCachedCall()
		{
			arrayCache[0] = this;
			return (int)methodInfo.Invoke(this, arrayCache);
		}

		[Benchmark]
		public int FunctionFromExpression()
		{
			return functionFromExpression(this);
		}

		[Benchmark]
		public int ReflectionAndCompiledCachedCall()
		{
			return expr11(this);
		}






		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int MethodForInlinedCall(int input)
		{
			return input + 1;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		int IFakeInterface.MethodForNoninlinedCall(int input)
		{
			return input + 1;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int MethodForCall(CallTest input)
		{
			return input.Asd;
		}

		//[MethodImpl(MethodImplOptions.NoInlining)]
		//public static int MethodForCall11(CallTest input)
		//{
		//	return input.Asd;
		//}


		public class MyClass
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			public static int MethodForCall11(CallTest input)
			{
				return input.Asd;
			}
		}
	}
}
