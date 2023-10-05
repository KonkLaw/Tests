using System;
using System.Collections;
using BenchmarkDotNet.Attributes;
using TestDotNet.Tests.MultithreadingTest;

namespace TestDotNet.Tests;


public class Info
{
	private readonly long ticks;
	private readonly int id;

	public Info(long ticks, int id)
	{
		this.ticks = ticks;
		this.id = id;
		TestOfTest.Log("Info ctor. id" + id);
	}

	public override string ToString()
	{
		return $"tick={ticks} id={id}";
	}
}

public class MyEnumeratorCollection<T> : IEnumerable<T>
{
	private readonly List<T> source;
	private readonly long ticks;

	class MyEnumerator : IEnumerator<T>
	{
		private readonly List<T>.Enumerator enumerator;
		private readonly IEnumerator<T> enumerator2;

		public MyEnumerator(List<T> source)
		{
			TestOfTest.Log("Enumerator ctor.");
			enumerator = source.GetEnumerator();
			enumerator2 = enumerator;
		}

		object? IEnumerator.Current
		{
			get => Current;
		}

		public T Current
		{
			get
			{
				TestOfTest.Log("Enumerator current");
				return enumerator2.Current;
			}
		}

		public bool MoveNext()
		{
			TestOfTest.Log("Enumerator MoveNext");
			return enumerator2.MoveNext();
		}

		public void Reset()
		{
			TestOfTest.Log("Enumerator Reset");
			enumerator2.Reset();
		}

		public void Dispose()
		{
			TestOfTest.Log("Enumerator Dispose");
			enumerator2.Dispose();
		}
	}


	public MyEnumeratorCollection(long ticks, List<T> source)
	{
		this.ticks = ticks;
		this.source = source;
		TestOfTest.Log($"Collection Ctor. ticks={ticks}");
	}

	public IEnumerator<T> GetEnumerator()
	{
		TestOfTest.Log("GetEnumerator Typed.");
		return new MyEnumerator(source);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		TestOfTest.Log("GetEnumerator common.");
		return GetEnumerator();
	}
}

public class TestOfTest
{
	private int parameter;

	[Params(1, 2, 3)]
	public int Parameter
	{
		get
		{
			Log("Param getter");
			return parameter;
		}
		set
		{
			Log("Param setter");
			parameter = value;
		}
	}

	public MyEnumeratorCollection<Info> myCollection;

	public MyEnumeratorCollection<Info> MyCollection
	{
		get
		{
			Log("Collection getter");
			return myCollection;
		}
	}

	static TestOfTest()
	{
		Log("Static Ctor");
	}

	public TestOfTest()
	{
		long ticks = DateTime.Now.Ticks;
		myCollection = new MyEnumeratorCollection<Info>(ticks, new List<Info>
		{
			new Info(ticks, 8),
			new Info(ticks, 9),
		});
		Log("Ctor");
	}

	[GlobalSetup] public void Setup()
	{
		Log("GlobalSetup + Param=" + Parameter);
	}

	[GlobalCleanup] public void CleanUp() => Log("GlobalCleanup");

	[IterationSetup] public void IterationSetup() => Log("IterationSetup");

	[IterationCleanup] public void IterationCleanup() => Log("IterationCleanup");

	[Benchmark]
	[ArgumentsSource(nameof(MyCollection))]
	public void Run1(Info info)
	{
		Log("Run1 Param=" + Parameter + " info:" + info);
		Thread.Sleep(10);
	}

	[Benchmark]
	[ArgumentsSource(nameof(MyCollection))]
	public void Run2(Info info)
	{
		Log("Run2 Param=" + Parameter + " info:" + info);
		Thread.Sleep(10);
	}

	[Benchmark]
	[ArgumentsSource(nameof(MyCollection))]
	public void Run3(Info info)
	{
		Log("Run3 Param=" + Parameter + " info:" + info);
		Thread.Sleep(10);
	}

	public static void Log(string message)
	{
		Console.WriteLine(message);
	}
}