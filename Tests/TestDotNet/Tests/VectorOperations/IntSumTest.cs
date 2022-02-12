using BenchmarkDotNet.Attributes;
using System.Numerics;
using TestDotNet.Utils;

namespace TestDotNet.Tests.VectorOperations
{
    public class IntSumTest
    {
        int[] arrA;
        int[] arrB;
        int[] arrRes;

        Vector<int>[] arrSimdA;
        Vector<int>[] arrSimdB;
        Vector<int>[] arrSimdRes;

        public IntSumTest()
        {
            const int vectorsCount = 10000000;

            arrA = RandomHelper.GetIntNumbers(vectorsCount * 4);
            arrB = RandomHelper.GetIntNumbers(vectorsCount * 4);
            arrRes = new int[arrA.Length];

            arrSimdA = new Vector<int>[vectorsCount];
            arrSimdB = new Vector<int>[vectorsCount];
            arrSimdRes = new Vector<int>[vectorsCount];
            for (int i = 0; i < vectorsCount; i++)
            {
                arrSimdA[i] = new Vector<int>(arrA, i * 4);
                arrSimdB[i] = new Vector<int>(arrB, i * 4);
            }
        }

        [Benchmark]
        public int[] SumWithConvertionToSimdVectors()
        {
            int numsOnVectorCount = Vector<int>.Count;
            int[] _arrA = arrA;
            int[] _arrB = arrB;
            int[] _arrRes = arrRes;
            for (int i = 0; i <= _arrA.Length - numsOnVectorCount; i += numsOnVectorCount)
            {
                var va = new Vector<int>(_arrA, i);
                var vb = new Vector<int>(_arrB, i);
                Vector<int> vRes = va + vb;
                vRes.CopyTo(_arrRes, i);
            }
            return _arrRes;
        }

        [Benchmark]
        public int[] SumAsArrays()
        {
            int[] _arrA = arrA;
            int[] _arrB = arrB;
            int[] _arrRes = arrRes;
            for (int i = 0; i < _arrA.Length; i++)
            {
                _arrRes[i] = _arrA[i] + _arrB[i];
            }
            return _arrRes;
        }

        [Benchmark]
        public Vector<int>[] SumSimdVectros()
        {
            Vector<int>[] _arrA = arrSimdA;
            Vector<int>[] _arrB = arrSimdB;
            Vector<int>[] _arrRes = arrSimdRes;

            for (int i = 0; i < _arrA.Length; i++)
            {
                _arrRes[i] = _arrA[i] + _arrB[i];
            }

            return _arrRes;
        }
    }
}
