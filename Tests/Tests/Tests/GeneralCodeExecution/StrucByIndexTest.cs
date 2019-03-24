using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tests.HelpersTypes;

//BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//Processor=Intel Core i5-3210M CPU 2.50GHz(Ivy Bridge), ProcessorCount=4
//Frequency=2435879 Hz, Resolution=410.5294 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0


//                                    Method |     Mean |     Error |    StdDev |
//------------------------------------------ |---------:|----------:|----------:|
//                           GetByConversion | 203.4 us | 0.8550 us | 0.7998 us |
//                         GetFromArrayByRef | 148.0 us | 0.5221 us | 0.4360 us |
//                        GetFromArrayByThis | 147.4 us | 1.3208 us | 1.1708 us |
//                         GetWithConditions | 606.4 us | 1.7922 us | 1.6765 us |
// GetFromArrayByThis_FromSaveWithConversion | 166.7 us | 0.5212 us | 0.4875 us |




//	BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//	Processor=Intel Core i7-8700K CPU 3.70GHz, ProcessorCount=12
//	Frequency=3609384 Hz, Resolution=277.0556 ns, Timer=TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0
//DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0


//     Method |      Mean |     Error |    StdDev |
//------------------- |----------:|----------:|----------:|
//   GetByConversion | 133.05 us | 0.8309 us | 0.7772 us |
// GetFromArrayByRef |  96.35 us | 0.2012 us | 0.1680 us |
//GetFromArrayByThis |  97.59 us | 0.6532 us | 0.6110 us |
// GetWithConditions | 413.81 us | 1.5028 us | 1.2549 us |


namespace Tests.Tests
{
	public class StrucByIndexTest
	{
		private const int Count = 100_000;
		private readonly int[] indexes;
		private readonly IndexerStruct strusture;
		private readonly SafeStruct safeStruct;

		private int index;

		public StrucByIndexTest()
		{
			index = 0;
			indexes = RandomHelper.GetIntNumbers(Count + 1, 0, 2);
		}

		[Benchmark]
		public unsafe float GetByConversion()
		{
			float res = 0;
			for (int i = 0; i <= Count; i++)
			{
				res += strusture.GetByConversion(indexes[i]);
			}
			return res;
		}

		[Benchmark]
		public unsafe float GetFromArrayByRef()
		{
			float res = 0;
			for (int i = 0; i <= Count; i++)
			{
				res += strusture.GetFromArrayByRef(indexes[i]);
			}
			return res;
		}

		[Benchmark]
		public unsafe float GetFromArrayByThis()
		{
			float res = 0;
			for (int i = 0; i <= Count; i++)
			{
				res += strusture.GetFromArrayByThis(indexes[i]);
			}
			return res;
		}

		[Benchmark]
		public unsafe float GetWithConditions()
		{
			float res = 0;
			for (int i = 0; i <= Count; i++)
			{
				res += strusture.GetWithConditions(indexes[i]);
			}
			return res;
		}

		[Benchmark]
		public unsafe float GetFromArrayByThis_FromSaveWithConversion()
		{
			float res = 0;
			for (int i = 0; i <= Count; i++)
			{
				res += safeStruct.GetFromArrayByThis(indexes[i]);
			}
			return res;
		}
	}

	struct SafeStruct
	{
		public float X;
		public float Y;
		public float Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetFromArrayByThis(int i)
		{
			return GetFromArrayByRef(this, i);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static float GetFromArrayByRef(SafeStruct str, int i)
		{
			return (*((IndexerStruct*)(&str))).GetFromArrayByThis(i);
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	unsafe struct IndexerStruct
	{
		[FieldOffset(0)]
		private fixed float fields[3];
		[FieldOffset(0)]
		public float X;
		[FieldOffset(4)]
		public float Y;
		[FieldOffset(8)]
		public float Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe float GetFromArrayByThis(int i)
		{
			return this.fields[i];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe float GetByConversion(int i)
		{
			fixed (void* ptr = (&this))
			{
				return *(((float*)ptr) + i);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe float GetFromArrayByRef(int i)
		{
			return GetFromArrayByRef(ref this, i);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static float GetFromArrayByRef(ref IndexerStruct str, int i)
		{
			return str.fields[i];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe float GetWithConditions(int i)
		{
			if (i == 0)
			{
				return X;
			}
			else if (i == 1)
			{
				return Y;
			}
			else
				return Z;
		}
	}
}
