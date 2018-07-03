using System.Numerics;
using BenchmarkDotNet.Attributes;
using Tests.Tests.BaseDataTypes;

//              Method |       Mean |     Error |    StdDev |
//---------------------|-----------:|----------:|----------:|
//          NonSimdRun | 1,055.2 us | 0.1108 us | 0.1037 us |
//            SimdRun3 |   747.5 us | 0.0977 us | 0.0763 us |
//            SimdRun4 | 1,010.0 us | 0.6459 us | 0.6042 us |
//SimdMatrOnVertorsDot |   593.3 us | 0.4458 us | 0.4170 us |
//SimdMatrOnVertorsSum |   251.0 us | 0.0415 us | 0.0388 us |

namespace Tests.Tests
{
    public class VectorOperationsSimdTest
    {
	    private const int VectorsCount = 100000;

	    private static readonly Matrix4F MatrixNonSimd = new Matrix4F
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

	    private static readonly Matrix4x4 MatrixSimd = new Matrix4x4
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

		private readonly Vector3F[] vectorsNonSimd;

	    private readonly Vector4[] vectorsSimd4;

	    private readonly Vector3[] vectorsSimd3;

		private readonly Vector3 v1 = new Vector3(
			RandomHelper.GetFloat(),
			RandomHelper.GetFloat(),
			RandomHelper.GetFloat());
	    private readonly Vector3 v2 = new Vector3(
		    RandomHelper.GetFloat(),
		    RandomHelper.GetFloat(),
		    RandomHelper.GetFloat());
	    private readonly Vector3 v3 = new Vector3(
		    RandomHelper.GetFloat(),
		    RandomHelper.GetFloat(),
		    RandomHelper.GetFloat());
	    private readonly Vector3 v4 = new Vector3(
		    RandomHelper.GetFloat(),
		    RandomHelper.GetFloat(),
		    RandomHelper.GetFloat());

		public VectorOperationsSimdTest()
	    {
		    vectorsNonSimd = RandomHelper.GetVectors(VectorsCount);

		    float[] values = RandomHelper.GetFloatNumbers(VectorsCount * 3);
		    vectorsSimd4 = new Vector4[VectorsCount];
		    for (int i = 0; i < vectorsNonSimd.Length; i++)
		    {
			    vectorsSimd4[i] = new Vector4(
					values[3 * i],
					values[3 * i + 1],
					values[3 * i + 2],
					0);
			}

		    vectorsSimd3 = new Vector3[VectorsCount];
		    for (int i = 0; i < vectorsNonSimd.Length; i++)
		    {
			    vectorsSimd3[i] = new Vector3(
				    values[3 * i],
				    values[3 * i + 1],
				    values[3 * i + 2]);
		    }



		}

		[Benchmark]
	    public Vector3F[] NonSimdRun()
		{
			Matrix4F m = MatrixNonSimd;
			Vector3F[] _vectors = vectorsNonSimd;
			for (int i = 0; i < _vectors.Length; i++)
			{
				_vectors[i] = m * _vectors[i];
			}
			return _vectors;
		}

	    [Benchmark]
		public Vector3[] SimdRun3()
	    {
		    Matrix4x4 m = MatrixSimd;
		    Vector3[] _vectors = vectorsSimd3;
		    for (int i = 0; i < _vectors.Length; i++)
		    {
			    _vectors[i] = Vector3.Transform(_vectors[i], m);
		    }
		    return _vectors;
	    }

	    [Benchmark]
	    public Vector4[] SimdRun4()
	    {
		    Matrix4x4 m = MatrixSimd;
		    Vector4[] _vectors = vectorsSimd4;
		    for (int i = 0; i < _vectors.Length; i++)
		    {
			    _vectors[i] = Vector4.Transform(_vectors[i], m);
		    }
		    return _vectors;
	    }

	    [Benchmark]
	    public Vector3[] SimdMatrOnVertorsDot()
	    {
		    Vector3 _v1 = v1;
		    Vector3 _v2 = v2;
		    Vector3 _v3 = v3;
		    Vector3 _v4 = v4;
		    Vector3[] _vectors = vectorsSimd3;
		    for (int i = 0; i < _vectors.Length; i++)
		    {
			    Vector3 v = _vectors[i];
			    _vectors[i] = new Vector3(
				    Vector3.Dot(v, _v1),
				    Vector3.Dot(v, _v2),
				    Vector3.Dot(v, _v3)) + _v4;
		    }
		    return _vectors;
	    }

	    [Benchmark]
		public Vector3[] SimdMatrOnVertorsSum()
	    {
			Vector3 _v1 = v1;
		    Vector3 _v2 = v2;
		    Vector3 _v3 = v3;
		    Vector3 _v4 = v4;
		    Vector3[] _vectors = vectorsSimd3;
		    for (int i = 0; i < _vectors.Length; i++)
		    {
			    Vector3 v = _vectors[i];
			    _vectors[i] = Vector3.Multiply(v, _v1) + Vector3.Multiply(v, _v2) + Vector3.Multiply(v, _v3) + _v4;
		    }
		    return _vectors;
		}
	}
}