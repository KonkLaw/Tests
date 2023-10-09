using System.Collections.Concurrent;
using TestDotNet.Tests.MultithreadingTest;
using TestDotNet.Utils;

class ConsurencyTest
{
	readonly struct RunInfo : IEquatable<RunInfo>
	{
		private readonly int threadId;
		private readonly int id;

		public RunInfo(int id)
		{
			threadId = Thread.CurrentThread.ManagedThreadId;
			this.id = id;
		}

		public bool Equals(RunInfo other)
			=> threadId == other.threadId && id == other.id;

		public override int GetHashCode() => threadId ^ id;

		public override string ToString()
			=> $"{threadId} ({id})";
	}

	private static readonly object SynchObject = new object();

	private readonly ConcurrentDictionary<RunInfo, byte>[] dictionaries;
	private readonly int vectorsCount = 100_000;
	private readonly int threadsCount;
	private readonly Vector3F[] array;
	private bool isFinished;

	private ConcurrentDictionary<RunInfo, byte> set;
	private ConcurrentDictionary<RunInfo, byte> Set
	{
		get
		{
			lock (SynchObject)
			{
				return set;
			}
		}
		set
		{
			lock (SynchObject)
			{
				set = value;
			}
		}
	}

	public ConsurencyTest()
	{
		int checkCount = 10000;
		dictionaries = new ConcurrentDictionary<RunInfo, byte>[checkCount];
		for (int i = 0; i < dictionaries.Length; i++)
		{
			dictionaries[i] = new ConcurrentDictionary<RunInfo, byte>();
		}

		threadsCount = Environment.ProcessorCount;
		array = RandomHelper.GetVectors(vectorsCount, -5, 5);
	}

	public void Get(int i, out int start, out int stop)
	{
		int chunkSize = vectorsCount / (threadsCount + 1);
		start = chunkSize * i;
		stop = chunkSize * (i + 1);
	}

	public void Run()
	{
		Task.Run(() =>
		{
			for (int i = 0; i < dictionaries.Length; i++)
			{
				Set = dictionaries[i];

				for (int j = 0; j < 2; j++)
				{
					RunWork(threadsCount);
                    AddInfo(threadsCount);
                }
			}
			isFinished = true;
		});
		Parallel.For(0, threadsCount, Process);

		foreach (ConcurrentDictionary<RunInfo, byte> dictionary in dictionaries)
		{
			foreach (KeyValuePair<RunInfo, byte> keyValuePair in dictionary)
			{
				Console.Write(keyValuePair.Key);
				Console.Write(" || ");
			}
			Console.WriteLine();
		}
		Console.WriteLine("Finish");
	}

	private void Process(int i)
	{
		while (!isFinished)
		{
			RunWork(i);
			AddInfo(i);
		}
	}

    private void AddInfo(int id) => Set.AddOrUpdate(new RunInfo(id), 0, (_, _) => 0);

    private void RunWork(int i)
	{
		Get(i, out int start, out int stop);
		Algorithms.Find3FBounds(array, start, stop);
	}
}