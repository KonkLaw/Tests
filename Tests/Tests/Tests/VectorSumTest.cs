﻿using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace Tests.Tests
{
	public class FloatSummTest
	{
		Vector4[] arrSimdA;
		Vector4[] arrSimdB;
		Vector4[] arrSimdRes;

		Vector4FVal[] arrValA;
		Vector4FVal[] arrValB;
		Vector4FVal[] arrValRes;

		float[] arrA;
		float[] arrB;
		float[] arrRes;

		public FloatSummTest()
		{
			const int vectorsCount = 10000000;

			arrSimdA = GetSimdVectros(vectorsCount);
			arrSimdB = GetSimdVectros(vectorsCount);
			arrSimdRes = new Vector4[vectorsCount];

			arrValA = GetValueTypeVectros(vectorsCount);
			arrValB = GetValueTypeVectros(vectorsCount);
			arrValRes = new Vector4FVal[vectorsCount];

			arrA = RandomHelper.GetFloatNumbers(vectorsCount * 4);
			arrB = RandomHelper.GetFloatNumbers(vectorsCount * 4);
			arrRes = new float [vectorsCount * 4];

			//Vector4FRef[] vecar = GetRefVectros(vectroCount);
			//Vector4FRef[] vecbr = GetRefVectros(vectroCount);
			//Vector4FRef[] vecresr = new Vector4FRef[vectroCount];
		}

		private static Vector4[] GetSimdVectros(int vectroCount)
		{
			float[] floats = RandomHelper.GetFloatNumbers(vectroCount * 4);
			var res = new Vector4[vectroCount];
			for (int i = 0; i < floats.Length; i += 4)
			{
				res[(i + 1) / 4] = new Vector4(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
			}
			return res;
		}

		private static Vector4FVal[] GetValueTypeVectros(int vectroCount)
		{
			float[] floats = RandomHelper.GetFloatNumbers(vectroCount * 4);
			var res = new Vector4FVal[vectroCount];
			for (int i = 0; i < floats.Length; i += 4)
			{
				res[(i + 1) / 4] = new Vector4FVal(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
			}
			return res;
		}

		private static Vector4FRef[] GetRefVectros(int vectroCount)
		{
			float[] floats = RandomHelper.GetFloatNumbers(vectroCount * 4);
			var res = new Vector4FRef[vectroCount];
			for (int i = 0; i < floats.Length; i += 4)
			{
				res[(i + 1) / 4] = new Vector4FRef(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
			}
			return res;
		}

		[Benchmark]
		public float[] ArraysSumWithSimd()
		{
			int numsOnVectorCount = Vector<float>.Count;
			float[] _arrA = arrA;
			float[] _arrB = arrB;
			float[] _arrRes = arrRes;

			for (int i = 0; i <= _arrRes.Length - numsOnVectorCount; i += numsOnVectorCount)
			{
				var va = new Vector<float>(_arrA, i);
				var vb = new Vector<float>(_arrB, i);
				Vector<float> vRes = va + vb;
				vRes.CopyTo(_arrRes, i);
			}
			return _arrRes;
		}

		[Benchmark]
		public Vector4[] VectorsSumWithSimd()
		{
			Vector4[] _arrSimdA = arrSimdA;
			Vector4[] _arrSimdB = arrSimdB;
			Vector4[] _arrSimdRes = arrSimdRes;
			for (int i = 0; i < _arrSimdRes.Length; i++)
			{
				_arrSimdRes[i] = _arrSimdA[i] + _arrSimdB[i];
			}
			return _arrSimdRes;
		}

		[Benchmark]
		public Vector4FVal[] VectorsValueTypeSum()
		{
			Vector4FVal[] _arrValA = arrValA;
			Vector4FVal[] _arrValB = arrValB;
			Vector4FVal[] _arrValRes = arrValRes;
			for (int i = 0; i < _arrValRes.Length; i++)
			{
				_arrValRes[i] = _arrValA[i] + _arrValB[i];
			}
			return _arrValRes;
		}
	}
}