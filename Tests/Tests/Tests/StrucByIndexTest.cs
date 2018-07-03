using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

// BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
// Processor=Intel Core i5-3210M CPU 2.50GHz(Ivy Bridge), ProcessorCount=4
// Frequency=2435879 Hz, Resolution=410.5294 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0


//             Method |     Mean |     Error |    StdDev |
//------------------- |---------:|----------:|----------:|
//    GetByConversion | 204.3 us | 1.1822 us | 1.0480 us |
//  GetFromArrayByRef | 153.2 us | 0.6373 us | 0.5961 us |
// GetFromArrayByThis | 153.1 us | 0.4251 us | 0.3550 us |
//  GetWithConditions | 619.1 us | 1.7965 us | 1.6804 us |


namespace Tests.Tests
{
	public class StrucByIndexTest
	{
		private const int Count = 100_000;
		private readonly int[] indexes;
		private readonly IndexerStruct strusture;
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
	}

	unsafe struct IndexerStruct
	{
		private fixed float fields[3];
		public float X;
		public float Y;
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
