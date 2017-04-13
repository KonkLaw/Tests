//  ----------------------------------------------------------------------------------------
//   Module name: LazyTest.cs
//  
//   Author:      Artyom Polishchuk
//  
//   Created at:  13-04-2017
//  
//   Description: 
//  
//  ----------------------------------------------------------------------------------------

using System;
using BenchmarkDotNet.Running;
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
			if (creator == null)
				throw new ArgumentNullException(nameof(creator));
			this.creator = creator;
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
		private Func<T> getter;
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
			if (creator == null)
				throw new ArgumentNullException(nameof(creator));
			inited = false;
			this.creator = creator;
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

	public class LazyTest
	{
		private LazyDirect<int>[] lazy1;
		private LazyCond<int>[] lazy2;

		private const int CountTest = 10000;

		public LazyTest()
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
				bool shouldPreinit = true;//i % 2 == 1;
				
				if (shouldPreinit)
				{
					var t1 = lazy1[i].Value;
					var t2 = lazy2[i].Value;
				}
			}
		}

		[Benchmark]
		public int RunWithDirectLazy()
		{
			int sum = 0;
			LazyDirect<int>[] _arr = lazy1;
			for (int i = 0; i < _arr.Length; i++)
			{
				sum += _arr[i].Value;
			}
			return sum;
		}

		[Benchmark]
		public int RunWithCondLazy()
		{
			int sum = 0;
			LazyCond<int>[] _arr = lazy2;
			for (int i = 0; i < _arr.Length; i++)
			{
				sum += _arr[i].Value;
			}
			return sum;
		}

		public void Run()
		{
			var l = new LazyDirect<int>(() => 3);
			var tt = l.Value;
			tt = l.Value;
		}
	}
}