//BenchmarkDotNet=v0.10.7, OS=Windows 10.0.18363
//Processor=Intel Core i7-8750H CPU 2.20GHz, ProcessorCount=12
//Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0


//BenchmarkDotNet=v0.10.7, OS=Windows 10.0.18363
//Processor=Intel Core i7-8750H CPU 2.20GHz, ProcessorCount=12
//Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0

//                         Method |      Mean |     Error |    StdDev |
//------------------------------- |----------:|----------:|----------:|
//                       Inlining | 0.0000 ns | 0.0000 ns | 0.0000 ns |
//                     NoInlining | 0.9535 ns | 0.0063 ns | 0.0056 ns |
//             AbstractMethodThis | 0.7346 ns | 0.0064 ns | 0.0059 ns |
//              InterfaceFromThis | 1.2313 ns | 0.0052 ns | 0.0049 ns |
//            AbstractMethodOther | 0.7371 ns | 0.0043 ns | 0.0036 ns |
//             InterfaceFromOther | 1.2081 ns | 0.0067 ns | 0.0063 ns |
//                 DelegateSimple | 0.9548 ns | 0.0018 ns | 0.0015 ns |
// DelegateFromCompiledExpression | 0.7334 ns | 0.0097 ns | 0.0081 ns |
//   DelegateFromManualExpression | 0.7292 ns | 0.0013 ns | 0.0011 ns |
	
//     REFLECTION IS APPROXIMATLY 200 times slowwer than delegate


//BenchmarkDotNet=v0.10.7, OS=Windows 10.0.18362
//Processor=Intel Core i7-8700K CPU 3.70GHz, ProcessorCount=12
//Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4121.0
//DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4121.0


//						  Method |      Mean |     Error |    StdDev |
//-------------------------------|----------:|----------:|----------:|
//						Inlining | 0.0000 ns | 0.0000 ns | 0.0000 ns |
//					  NoInlining | 0.8755 ns | 0.0203 ns | 0.0190 ns |
//			  AbstractMethodThis | 0.6544 ns | 0.0195 ns | 0.0152 ns |
//			   InterfaceFromThis | 1.1019 ns | 0.0101 ns | 0.0094 ns |
//			 AbstractMethodOther | 0.6661 ns | 0.0076 ns | 0.0055 ns |
//			  InterfaceFromOther | 1.1076 ns | 0.0256 ns | 0.0200 ns |
//				  DelegateSimple | 0.8755 ns | 0.0260 ns | 0.0217 ns |
//DelegateFromCompiledExpression | 0.6816 ns | 0.0108 ns | 0.0090 ns |
//  DelegateFromManualExpression | 0.6664 ns | 0.0115 ns | 0.0096 ns |

using BenchmarkDotNet.Attributes;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Tests.HelpersTypes;

namespace Tests.Tests
{
	public interface IFakeInterface
	{
		int InterfaceMethod(int input);
	}

	public abstract class BaseClass
	{
		public abstract int AbstractCall(int input);
	}

	public class FakeClass : BaseClass, IFakeInterface
	{
		public override int AbstractCall(int input) => input + 1;

		public int InterfaceMethod(int input) => input + 1;
	}

	public class OneCallTest : BaseClass, IFakeInterface
	{
		private readonly object expression = (Expression<Func<int, int>>)(i => i + 1);
#pragma warning disable IDE0044 // Add readonly modifier
		private IFakeInterface interfacePointerThis;
		private BaseClass abstractOther;
		private IFakeInterface interfacePointerOther;
		private Func<int, int> delegateSimple;
		private Func<int, int> delegateFromExpression;
		private Func<int, int> delegateFromManualExpression;
#pragma warning restore IDE0044 // Add readonly modifier

		public OneCallTest()
		{
			interfacePointerThis = this;
			abstractOther = new FakeClass();
			interfacePointerOther = new FakeClass();
			delegateSimple = MethodWithoutInlining;
			delegateFromExpression = ((Expression<Func<int, int>>)expression).Compile();

			ParameterExpression parameter1 = Expression.Parameter(typeof(int), "parameter1");
			Expression sumExpr = Expression.Add(parameter1, Expression.Constant(1));
			delegateFromManualExpression = Expression.Lambda<Func<int, int>>(sumExpr, parameter1).Compile();
		}

		[Benchmark]
		public int Inlining()
		{
			return MethodWithInlining(48);
		}

		[Benchmark]
		public int NoInlining()
		{
			return MethodWithoutInlining(48);
		}

		[Benchmark]
		public int AbstractMethodThis()
		{
			return AbstractCall(48);
		}

		[Benchmark]
		public int InterfaceFromThis()
		{
			return interfacePointerThis.InterfaceMethod(48);
		}

		[Benchmark]
		public int AbstractMethodOther()
		{
			return abstractOther.AbstractCall(48);
		}

		[Benchmark]
		public int InterfaceFromOther()
		{
			return interfacePointerOther.InterfaceMethod(48);
		}

		[Benchmark]
		public int DelegateSimple()
		{
			return delegateSimple(48);
		}

		[Benchmark]
		public int DelegateFromCompiledExpression()
		{
			return delegateFromExpression(48);
		}

		[Benchmark]
		public int DelegateFromManualExpression()
		{
			return delegateFromManualExpression(48);
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int MethodWithInlining(int input)
		{
			return input + 1;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int MethodWithoutInlining(int input)
		{
			return input + 1;
		}

		public override int AbstractCall(int input)
		{
			return input + 1;
		}

		public int InterfaceMethod(int input)
		{
			return input + 1;
		}
	}


	public class Class1 : BaseClass, IFakeInterface
	{
		public override int AbstractCall(int input) => input + 1;
		public int InterfaceMethod(int input) => input + 1;
	}

	public class Class2 : BaseClass, IFakeInterface
	{
		public override int AbstractCall(int input) => input + 1;
		public int InterfaceMethod(int input) => input + 1;
	}

	public class Class3 : BaseClass, IFakeInterface
	{
		public override int AbstractCall(int input) => input + 1;
		public int InterfaceMethod(int input) => input + 1;
	}

	public class Class4 : BaseClass, IFakeInterface
	{
		public override int AbstractCall(int input) => input + 1;
		public int InterfaceMethod(int input) => input + 1;
	}

	public class Class5 : BaseClass, IFakeInterface
	{
		public override int AbstractCall(int input) => input + 1;
		public int InterfaceMethod(int input) => input + 1;
	}


//	BenchmarkDotNet=v0.10.7, OS=Windows 10.0.18363
//Processor=Intel Core i7-8750H CPU 2.20GHz, ProcessorCount=12
//Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0

//        Method |      Mean |     Error |    StdDev |
//-------------- |----------:|----------:|----------:|
//  AbstractCall |  9.737 ms | 0.0363 ms | 0.0340 ms |
// InterfaceCall | 11.278 ms | 0.0163 ms | 0.0144 ms |
//  DelegateCall |  9.973 ms | 0.0307 ms | 0.0287 ms |



//BenchmarkDotNet=v0.10.7, OS=Windows 10.0.18362
//Processor=Intel Core i7-8700K CPU 3.70GHz, ProcessorCount=12
//Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4121.0
//DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4121.0

//	      Method |     Mean |     Error |    StdDev |
//-------------- |---------:|----------:|----------:|
//	AbstractCall | 8.560 ms | 0.0665 ms | 0.0519 ms |
// InterfaceCall | 9.926 ms | 0.0420 ms | 0.0372 ms |
//  DelegateCall | 8.757 ms | 0.1688 ms | 0.2253 ms |

	public class MultimpleCall
	{
		private const int RunCount = 1_000_000;
		private const int SamplesCount = 5;

		private readonly BaseClass[] abstractArray;
		private readonly IFakeInterface[] interafceArray;
		private readonly Func<int, int>[] delegateArray;

		public MultimpleCall()
		{
			abstractArray = GetArrayFroAbstract();
			interafceArray = GetArrayFroInterface();
			delegateArray = GetArrayFroDelegates();
		}

		BaseClass[] GetArrayFroAbstract()
		{
			Func<BaseClass>[] factories = new Func<BaseClass>[SamplesCount]
			{
				() => new Class1(),
				() => new Class2(),
				() => new Class3(),
				() => new Class4(),
				() => new Class5(),
			};

			var result = new BaseClass[RunCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = RandomHelper.GetByRandom(factories);
			}
			return result;
		}

		IFakeInterface[] GetArrayFroInterface()
		{
			Func<IFakeInterface>[] factories = new Func<IFakeInterface>[SamplesCount]
			{
				() => new Class1(),
				() => new Class2(),
				() => new Class3(),
				() => new Class4(),
				() => new Class5(),
			};

			var result = new IFakeInterface[RunCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = RandomHelper.GetByRandom(factories);
			}
			return result;
		}

		Func<int, int>[] GetArrayFroDelegates()
		{
			Func<Func<int, int>>[] factories = new Func<Func<int, int>>[SamplesCount]
			{
				() => new Func<int, int>(MethodForDelegate1),
				() => new Func<int, int>(MethodForDelegate2),
				() => new Func<int, int>(MethodForDelegate3),
				() => new Func<int, int>(MethodForDelegate4),
				() => new Func<int, int>(MethodForDelegate5),
			};

			var result = new Func<int, int>[RunCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = RandomHelper.GetByRandom(factories);
			}
			return result;
		}

		[Benchmark]
		public void AbstractCall()
		{
			for (int i = 0; i < RunCount; i++)
			{
				abstractArray[i].AbstractCall(48);
			}
		}

		[Benchmark]
		public void InterfaceCall()
		{
			for (int i = 0; i < RunCount; i++)
			{
				interafceArray[i].InterfaceMethod(48);
			}
		}

		[Benchmark]
		public void DelegateCall()
		{
			for (int i = 0; i < RunCount; i++)
			{
				delegateArray[i](48);
			}
		}

		private int MethodForDelegate1(int input) => input + 1;
		private int MethodForDelegate2(int input) => input + 1;
		private int MethodForDelegate3(int input) => input + 1;
		private int MethodForDelegate4(int input) => input + 1;
		private int MethodForDelegate5(int input) => input + 1;
	}
}