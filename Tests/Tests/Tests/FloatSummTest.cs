using BenchmarkDotNet.Attributes;
using System.Numerics;
using Tests.Tests.BaseDataTypes;

//BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 2 (10.0.15063)
//Processor=Intel Core i5-2500 CPU 3.30GHz(Sandy Bridge), ProcessorCount=4
//Frequency=3233209 Hz, Resolution=309.2902 ns, Timer=TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2115.0
//DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2115.0
//
//             Method |     Mean |     Error |    StdDev |
//--------------------|---------:|----------:|----------:|
//  ArraysSumWithSimd | 33.46 ms | 0.2570 ms | 0.2404 ms |
// Vector4SumWithSimd | 32.62 ms | 0.1490 ms | 0.1394 ms |
//Vector4ValueTypeSum | 90.84 ms | 0.2621 ms | 0.2452 ms |




//	BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//Processor=Intel Core i5-3210M CPU 2.50GHz(Ivy Bridge), ProcessorCount=4
//Frequency=2435879 Hz, Resolution=410.5294 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0
//              Method |     Mean |     Error |    StdDev |
//-------------------- |---------:|----------:|----------:|
//   ArraysSumWithSimd | 39.53 ms | 0.1660 ms | 0.1472 ms |
//  Vector4SumWithSimd | 40.57 ms | 0.2608 ms | 0.2178 ms |
// Vector4ValueTypeSum | 93.77 ms | 0.1810 ms | 0.1605 ms |



namespace Tests.Tests
{
	public class FloatSummTest
	{
		Vector4[] simdVector4ArrayA;
		Vector4[] simdVector4ArrayB;
		Vector4[] simdVector4Result;

		Vector3[] simdVector3ArrayA;
		Vector3[] simdVector3ArrayB;
		Vector3[] simdVector3Result;

		Vector4FVal[] arr4A;
		Vector4FVal[] arr4B;
		Vector4FVal[] arr4Res;

		Vector3F[] arr3A;
		Vector3F[] arr3B;
		Vector3F[] arr3Res;

		float[] arrA;
		float[] arrB;
		float[] arrRes;

		public FloatSummTest()
		{
			const int vectorsCount = 800_000;

			simdVector4ArrayA = GetSimdVectro4Array(vectorsCount);
			simdVector4ArrayB = GetSimdVectro4Array(vectorsCount);
			simdVector4Result = new Vector4[vectorsCount];

			simdVector3ArrayA = GetSimdVectro3Array(vectorsCount);
			simdVector3ArrayB = GetSimdVectro3Array(vectorsCount);
			simdVector3Result = new Vector3[vectorsCount];

			arr4A = GetValueTypeVectro4Array(vectorsCount);
			arr4B = GetValueTypeVectro4Array(vectorsCount);
			arr4Res = new Vector4FVal[vectorsCount];

			arr3A = GetValueTypeVectro3Array(vectorsCount);
			arr3B = GetValueTypeVectro3Array(vectorsCount);
			arr3Res = new Vector3F[vectorsCount];

			arrA = RandomHelper.GetFloatNumbers(vectorsCount * 4);
			arrB = RandomHelper.GetFloatNumbers(vectorsCount * 4);
			arrRes = new float [vectorsCount * 4];
		}

		private static Vector4[] GetSimdVectro4Array(int vectroCount)
		{
			float[] floats = RandomHelper.GetFloatNumbers(vectroCount * 4);
			var res = new Vector4[vectroCount];
			for (int i = 0; i < floats.Length; i += 4)
			{
				res[(i + 1) / 4] = new Vector4(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
			}
			return res;
		}

		private static Vector3[] GetSimdVectro3Array(int vectroCount)
		{
			float[] floats = RandomHelper.GetFloatNumbers(vectroCount * 4);
			var res = new Vector3[vectroCount];
			for (int i = 0; i < floats.Length; i += 4)
			{
				res[(i + 1) / 4] = new Vector3(floats[i], floats[i + 1], floats[i + 2]);
			}
			return res;
		}

		private static Vector4FVal[] GetValueTypeVectro4Array(int vectroCount)
		{
			float[] floats = RandomHelper.GetFloatNumbers(vectroCount * 4);
			var res = new Vector4FVal[vectroCount];
			for (int i = 0; i < floats.Length; i += 4)
			{
				res[(i + 1) / 4] = new Vector4FVal(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
			}
			return res;
		}

		private static Vector3F[] GetValueTypeVectro3Array(int vectroCount)
		{
			float[] floats = RandomHelper.GetFloatNumbers(vectroCount * 4);
			var res = new Vector3F[vectroCount];
			for (int i = 0; i < floats.Length; i += 4)
			{
				res[(i + 1) / 4] = new Vector3F(floats[i], floats[i + 1], floats[i + 2]);
			}
			return res;
		}

		//[Benchmark]
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

		//[Benchmark]
		public float[] ArraysSumWithSimd_Unrolled_8()
		{
			int numsOnVectorCount = Vector<float>.Count;
			int numsOnVectorCountUntolled = 8 * Vector<float>.Count;
			float[] _arrA = arrA;
			float[] _arrB = arrB;
			float[] _arrRes = arrRes;

			for (int i = 0; i <= _arrRes.Length - numsOnVectorCount; i += numsOnVectorCountUntolled)
			{
				var va = new Vector<float>(_arrA, i);
				var vb = new Vector<float>(_arrB, i);
				Vector<float> vRes = va + vb;
				vRes.CopyTo(_arrRes, i);

				int indexWithUnroll1 = i + 1 * numsOnVectorCount;
				va = new Vector<float>(_arrA, indexWithUnroll1);
				vb = new Vector<float>(_arrB, indexWithUnroll1);
				vRes = va + vb;
				vRes.CopyTo(_arrRes, indexWithUnroll1);

				int indexWithUnrol2 = i + 2 * numsOnVectorCount;
				va = new Vector<float>(_arrA, indexWithUnrol2);
				vb = new Vector<float>(_arrB, indexWithUnrol2);
				vRes = va + vb;
				vRes.CopyTo(_arrRes, indexWithUnrol2);

				int indexWithUnroll3 = i + 3 * numsOnVectorCount;
				va = new Vector<float>(_arrA, indexWithUnroll3);
				vb = new Vector<float>(_arrB, indexWithUnroll3);
				vRes = va + vb;
				vRes.CopyTo(_arrRes, indexWithUnroll3);

				int indexWithUnroll4 = i + 4 * numsOnVectorCount;
				va = new Vector<float>(_arrA, indexWithUnroll4);
				vb = new Vector<float>(_arrB, indexWithUnroll4);
				vRes = va + vb;
				vRes.CopyTo(_arrRes, indexWithUnroll4);

				int indexWithUnroll5 = i + 5 * numsOnVectorCount;
				va = new Vector<float>(_arrA, indexWithUnroll5);
				vb = new Vector<float>(_arrB, indexWithUnroll5);
				vRes = va + vb;
				vRes.CopyTo(_arrRes, indexWithUnroll5);

				int indexWithUnroll6 = i + 6 * numsOnVectorCount;
				va = new Vector<float>(_arrA, indexWithUnroll6);
				vb = new Vector<float>(_arrB, indexWithUnroll6);
				vRes = va + vb;
				vRes.CopyTo(_arrRes, indexWithUnroll6);

				int indexWithUnroll7 = i + 7 * numsOnVectorCount;
				va = new Vector<float>(_arrA, indexWithUnroll7);
				vb = new Vector<float>(_arrB, indexWithUnroll7);
				vRes = va + vb;
				vRes.CopyTo(_arrRes, indexWithUnroll7);
			}
			return _arrRes;
		}

		//[Benchmark]
		public Vector4[] Vector4SumWithSimd()
		{
			Vector4[] _arrSimdA = simdVector4ArrayA;
			Vector4[] _arrSimdB = simdVector4ArrayB;
			Vector4[] _arrSimdRes = simdVector4Result;
			for (int i = 0; i < _arrSimdRes.Length; i++)
			{
				_arrSimdRes[i] = _arrSimdA[i] + _arrSimdB[i];
			}
			return _arrSimdRes;
		}

		//[Benchmark]
		public Vector4[] Vector4SumWithSimd_Unrolled_8()
		{
			Vector4[] _arrSimdA = simdVector4ArrayA;
			Vector4[] _arrSimdB = simdVector4ArrayB;
			Vector4[] _arrSimdRes = simdVector4Result;
			for (int i = 0; i < _arrSimdRes.Length; i += 8)
			{
				int indexUnrolled0 = i;
				_arrSimdRes[indexUnrolled0] = _arrSimdA[indexUnrolled0] + _arrSimdB[indexUnrolled0];

				int indexUnrolled1 = i + 1;
				_arrSimdRes[indexUnrolled1] = _arrSimdA[indexUnrolled1] + _arrSimdB[indexUnrolled1];

				int indexUnrolled2 = i + 2;
				_arrSimdRes[indexUnrolled2] = _arrSimdA[indexUnrolled2] + _arrSimdB[indexUnrolled2];

				int indexUnrolled3 = i + 3;
				_arrSimdRes[indexUnrolled3] = _arrSimdA[indexUnrolled3] + _arrSimdB[indexUnrolled3];

				int indexUnrolled4 = i + 4;
				_arrSimdRes[indexUnrolled4] = _arrSimdA[indexUnrolled4] + _arrSimdB[indexUnrolled4];

				int indexUnrolled5 = i + 5;
				_arrSimdRes[indexUnrolled5] = _arrSimdA[indexUnrolled5] + _arrSimdB[indexUnrolled5];

				int indexUnrolled6 = i + 6;
				_arrSimdRes[indexUnrolled6] = _arrSimdA[indexUnrolled6] + _arrSimdB[indexUnrolled6];

				int indexUnrolled7 = i + 7;
				_arrSimdRes[indexUnrolled7] = _arrSimdA[indexUnrolled7] + _arrSimdB[indexUnrolled7];
			}
			return _arrSimdRes;
		}

		[Benchmark]
		public Vector3[] Vector3SumWithSimd()
		{
			Vector3[] _arrSimdA = simdVector3ArrayA;
			Vector3[] _arrSimdB = simdVector3ArrayB;
			Vector3[] _arrSimdRes = simdVector3Result;
			for (int i = 0; i < _arrSimdRes.Length; i++)
			{
				_arrSimdRes[i] = _arrSimdA[i] + _arrSimdB[i];
			}
			return _arrSimdRes;
		}

		//[Benchmark]
		public Vector3[] Vector3SumWithSimd_Unrolled_8()
		{
			Vector3[] _arrSimdA = simdVector3ArrayA;
			Vector3[] _arrSimdB = simdVector3ArrayB;
			Vector3[] _arrSimdRes = simdVector3Result;
			for (int i = 0; i < _arrSimdRes.Length; i += 8)
			{
				int indexUnrolled0 = i;
				_arrSimdRes[indexUnrolled0] = _arrSimdA[indexUnrolled0] + _arrSimdB[indexUnrolled0];

				int indexUnrolled1 = i + 1;
				_arrSimdRes[indexUnrolled1] = _arrSimdA[indexUnrolled1] + _arrSimdB[indexUnrolled1];

				int indexUnrolled2 = i + 2;
				_arrSimdRes[indexUnrolled2] = _arrSimdA[indexUnrolled2] + _arrSimdB[indexUnrolled2];

				int indexUnrolled3 = i + 3;
				_arrSimdRes[indexUnrolled3] = _arrSimdA[indexUnrolled3] + _arrSimdB[indexUnrolled3];

				int indexUnrolled4 = i + 4;
				_arrSimdRes[indexUnrolled4] = _arrSimdA[indexUnrolled4] + _arrSimdB[indexUnrolled4];

				int indexUnrolled5 = i + 5;
				_arrSimdRes[indexUnrolled5] = _arrSimdA[indexUnrolled5] + _arrSimdB[indexUnrolled5];

				int indexUnrolled6 = i + 6;
				_arrSimdRes[indexUnrolled6] = _arrSimdA[indexUnrolled6] + _arrSimdB[indexUnrolled6];

				int indexUnrolled7 = i + 7;
				_arrSimdRes[indexUnrolled7] = _arrSimdA[indexUnrolled7] + _arrSimdB[indexUnrolled7];
			}
			return _arrSimdRes;
		}

		[Benchmark]
		public Vector4FVal[] Vector4ValueTypeSum()
		{
			Vector4FVal[] _arrValA = arr4A;
			Vector4FVal[] _arrValB = arr4B;
			Vector4FVal[] _arrValRes = arr4Res;
			for (int i = 0; i < _arrValRes.Length; i++)
			{
				_arrValRes[i] = _arrValA[i] + _arrValB[i];
			}
			return _arrValRes;
		}

		//Benchmark]
		public Vector4FVal[] Vector4ValueTypeSum_Unrolled_8()
		{
			Vector4FVal[] _arrSimdA = arr4A;
			Vector4FVal[] _arrSimdB = arr4B;
			Vector4FVal[] _arrSimdRes = arr4Res;
			for (int i = 0; i < _arrSimdRes.Length; i += 8)
			{
				int indexUnrolled0 = i;
				_arrSimdRes[indexUnrolled0] = _arrSimdA[indexUnrolled0] + _arrSimdB[indexUnrolled0];

				int indexUnrolled1 = i + 1;
				_arrSimdRes[indexUnrolled1] = _arrSimdA[indexUnrolled1] + _arrSimdB[indexUnrolled1];

				int indexUnrolled2 = i + 2;
				_arrSimdRes[indexUnrolled2] = _arrSimdA[indexUnrolled2] + _arrSimdB[indexUnrolled2];

				int indexUnrolled3 = i + 3;
				_arrSimdRes[indexUnrolled3] = _arrSimdA[indexUnrolled3] + _arrSimdB[indexUnrolled3];

				int indexUnrolled4 = i + 4;
				_arrSimdRes[indexUnrolled4] = _arrSimdA[indexUnrolled4] + _arrSimdB[indexUnrolled4];

				int indexUnrolled5 = i + 5;
				_arrSimdRes[indexUnrolled5] = _arrSimdA[indexUnrolled5] + _arrSimdB[indexUnrolled5];

				int indexUnrolled6 = i + 6;
				_arrSimdRes[indexUnrolled6] = _arrSimdA[indexUnrolled6] + _arrSimdB[indexUnrolled6];

				int indexUnrolled7 = i + 7;
				_arrSimdRes[indexUnrolled7] = _arrSimdA[indexUnrolled7] + _arrSimdB[indexUnrolled7];
			}
			return _arrSimdRes;
		}

		[Benchmark]
		public Vector3F[] Vector3ValueTypeSum()
		{
			Vector3F[] _arrValA = arr3A;
			Vector3F[] _arrValB = arr3B;
			Vector3F[] _arrValRes = arr3Res;
			for (int i = 0; i < _arrValRes.Length; i++)
			{
				_arrValRes[i] = _arrValA[i] + _arrValB[i];
			}
			return _arrValRes;
		}

		//[Benchmark]
		public Vector3F[] Vector3ValueTypeSum_Unrolled_8()
		{
			Vector3F[] _arrSimdA = arr3A;
			Vector3F[] _arrSimdB = arr3B;
			Vector3F[] _arrSimdRes = arr3Res;
			for (int i = 0; i < _arrSimdRes.Length; i += 8)
			{
				int indexUnrolled0 = i;
				_arrSimdRes[indexUnrolled0] = _arrSimdA[indexUnrolled0] + _arrSimdB[indexUnrolled0];

				int indexUnrolled1 = i + 1;
				_arrSimdRes[indexUnrolled1] = _arrSimdA[indexUnrolled1] + _arrSimdB[indexUnrolled1];

				int indexUnrolled2 = i + 2;
				_arrSimdRes[indexUnrolled2] = _arrSimdA[indexUnrolled2] + _arrSimdB[indexUnrolled2];

				int indexUnrolled3 = i + 3;
				_arrSimdRes[indexUnrolled3] = _arrSimdA[indexUnrolled3] + _arrSimdB[indexUnrolled3];

				int indexUnrolled4 = i + 4;
				_arrSimdRes[indexUnrolled4] = _arrSimdA[indexUnrolled4] + _arrSimdB[indexUnrolled4];

				int indexUnrolled5 = i + 5;
				_arrSimdRes[indexUnrolled5] = _arrSimdA[indexUnrolled5] + _arrSimdB[indexUnrolled5];

				int indexUnrolled6 = i + 6;
				_arrSimdRes[indexUnrolled6] = _arrSimdA[indexUnrolled6] + _arrSimdB[indexUnrolled6];

				int indexUnrolled7 = i + 7;
				_arrSimdRes[indexUnrolled7] = _arrSimdA[indexUnrolled7] + _arrSimdB[indexUnrolled7];
			}
			return _arrSimdRes;
		}

		[Benchmark]
		public Vector3F[] Vector3ValueTypeSum111()
		{
			unsafe
			{
				Vector3F[] _arrValA = arr3A;
				Vector3F[] _arrValB = arr3B;
				Vector3F[] _arrValRes = arr3Res;
				for (int i = 0; i < _arrValRes.Length; i++)
				{
					Vector3 va;
					Vector3 vb;
					Vector3 vr;
					fixed (void* a = &_arrValA[i])
					{
						va = *((Vector3*)a);
					}
					fixed (void* b = &_arrValB[i])
					{
						vb = *((Vector3*)b);
					}
					vr = va + vb;
					_arrValRes[i] = *((Vector3F*)(&vr));
				}
				return _arrValRes;
			}
		}
	}
}
