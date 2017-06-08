using System;
using BenchmarkDotNet.Attributes;

namespace Tests.Tests
{
	public class LazyDirect<T>
	{
		private Func<T> creator;
		private Func<T> getter;
		private T value;

		public T Value => getter();

		public LazyDirect(Func<T> creator)
		{
			this.creator = creator ?? throw new ArgumentNullException(nameof(creator)); ;
			getter = FirstCreator;
		}

		private T FirstCreator()
		{
			value = creator();
			creator = null;
			getter = DirectValueGetter;
			return value;
		}

		private T DirectValueGetter() => value;
	}

	public class LazyCond<T>
	{
		private bool inited;
		private Func<T> creator;
		private T value;

		public T Value
		{
			get
			{
				if (!inited)
				{
					value = creator();
					creator = null;
					inited = true;
				}
				return value;
			}
		}

		public bool HasValue => creator == null;

		public LazyCond(Func<T> creator)
		{
			inited = false;
			this.creator = creator ?? throw new ArgumentNullException(nameof(creator));
		}
	}

	public class LazyTest
	{
		private int directIndex = -1;
		private int condIndex = -1;

		private LazyDirect<int>[] lazy1;
		private LazyCond<int>[] lazy2;

		private const int CountTest = 2000000;

		[GlobalSetupAttribute]
		public void Setup()
		{
			lazy1 = new LazyDirect<int>[CountTest];
			lazy2 = new LazyCond<int>[CountTest];
			for (int i = 0; i < CountTest; i++)
			{
				lazy1[i] = new LazyDirect<int>(() => 3);
				lazy2[i] = new LazyCond<int>(() => 3);
			}

			for (int i = 0; i < CountTest; i++)
			{
				if (RandomHelper.GetRandomBool())
				{
					var t1 = lazy1[i].Value;
					var t2 = lazy2[i].Value;
				}
			}
			directIndex = 0;
			condIndex = 0;
		}

		[Benchmark]
		public unsafe int RunWithDirectLazy()
		{
			LazyDirect<int>[] _arr = lazy1;
			directIndex += 2;
			int index = directIndex;
			return _arr[index - 2].Value + _arr[index - 1].Value;
		}

		[Benchmark]
		public int RunWithCondLazy()
		{
			LazyCond<int>[] _arr = lazy2;
			condIndex += 2;
			int index = condIndex;
			return _arr[index - 2].Value + _arr[index - 1].Value;
		}

		public void Run()
		{
			var l = new LazyDirect<int>(() => 3);
			var tt = l.Value;
			tt = l.Value;
		}
	}
}