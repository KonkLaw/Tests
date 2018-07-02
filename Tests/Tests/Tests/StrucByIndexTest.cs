using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace Tests.Tests
{
	class StrucByIndexTest
	{
		private const int Count = 511;
		private readonly int[] indexes;
		private readonly IndexerStruct strusture;
		private int index;

		public StrucByIndexTest()
		{
			index = 0;
			indexes = RandomHelper.GetIntNumbers(Count + 1, 0, 2);
		}

		[Benchmark]
		public float GetByConversion()
		{
			int i = (index++) & Count;
			return strusture.GetByConversion(i);
		}

		[Benchmark]
		public float GetFromArrayByRef()
		{
			int i = (index++) & Count;
			return strusture.GetFromArrayByRef(i);
		}

		[Benchmark]
		public float GetFromArrayByThis()
		{
			int i = (index++) & Count;
			return strusture.GetFromArrayByThis(i);
		}

		[Benchmark]
		public float GetWithConditions()
		{
			int i = (index++) & Count;
			return strusture.GetWithConditions(i);
		}
	}

	unsafe struct IndexerStruct
	{
		private fixed float fields[3];
		public float X;
		public float Y;
		public float Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetFromArrayByThis(int i)
		{
			return this.fields[i];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetByConversion(int i)
		{
			fixed (void* ptr = (&this))
			{
				return *(((float*)ptr) + i);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetFromArrayByRef(int i)
		{
			return GetFromArrayByRef(ref this, i);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float GetFromArrayByRef(ref IndexerStruct str, int i)
		{
			return str.fields[i];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetWithConditions(int i)
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
