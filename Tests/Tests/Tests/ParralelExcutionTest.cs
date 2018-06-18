using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Tests.Tests.BaseDataTypes;

namespace Tests.Tests
{
	public class ParralelExcutionTest
	{
		private readonly Matrix4F[] inPrams;
		private readonly Matrix4F[] outPrams;
		private const int MatricesCount = 100_000;

		public ParralelExcutionTest()
		{
			inPrams = GetRandomMatrices(MatricesCount);
			outPrams = new Matrix4F[MatricesCount];
		}


		[Benchmark]
		public void SingleTread()
		{
			Matrix4F[] _in = inPrams;
			Matrix4F[] _out = outPrams;
			for (int i = 0; i < inPrams.Length; i++)
			{
				Matrix4F.Invert(_in[i], out _out[i]);
			}
		}

		[Benchmark]
		public void ParralelDefault()
		{
			Parallel.For(0, MatricesCount, InvertOne);
		}

		[Benchmark]
		public void Parralel12()
		{
			Parallel.For(0, MatricesCount, new ParallelOptions()
			{
				MaxDegreeOfParallelism = 12
			}, InvertOne);
		}

		[Benchmark]
		public void Parralel6()
		{
			Parallel.For(0, MatricesCount, new ParallelOptions
			{
				MaxDegreeOfParallelism = 6
			}, InvertOne);
		}

		[Benchmark]
		public void Parralel4()
		{
			Parallel.For(0, MatricesCount, new ParallelOptions
			{
				MaxDegreeOfParallelism = 6
			}, InvertOne);
		}

		[Benchmark]
		public void Parralel14()
		{
			Parallel.For(0, MatricesCount, new ParallelOptions
			{
				MaxDegreeOfParallelism = 14
			}, InvertOne);
		}

		[Benchmark]
		public void Parralel16()
		{
			Parallel.For(0, MatricesCount, new ParallelOptions
			{
				MaxDegreeOfParallelism = 14
			}, InvertOne);
		}

		private void InvertOne(int index)
		{
			Matrix4F.Invert(inPrams[index], out outPrams[index]);
		}


		private static Matrix4F[] GetRandomMatrices(int count)
		{
			var matrices = new Matrix4F[count];
			for (int i = 0; i < count; i++)
			{
				matrices[i] = new Matrix4F
				{
					M11 = RandomHelper.GetFloat(),
					M12 = RandomHelper.GetFloat(),
					M13 = RandomHelper.GetFloat(),
					M14 = RandomHelper.GetFloat(),

					M21 = RandomHelper.GetFloat(),
					M22 = RandomHelper.GetFloat(),
					M23 = RandomHelper.GetFloat(),
					M24 = RandomHelper.GetFloat(),

					M31 = RandomHelper.GetFloat(),
					M32 = RandomHelper.GetFloat(),
					M33 = RandomHelper.GetFloat(),
					M34 = RandomHelper.GetFloat(),

					M41 = RandomHelper.GetFloat(),
					M42 = RandomHelper.GetFloat(),
					M43 = RandomHelper.GetFloat(),
					M44 = RandomHelper.GetFloat(),
				};
			}
			return matrices;
		}
	}
}