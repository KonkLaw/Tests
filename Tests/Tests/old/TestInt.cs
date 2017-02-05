using System;
using System.Diagnostics;
using System.Numerics;

namespace SimdTest
{
    static class TestInt
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        internal static void Run()
        {
            //int numbersOnVectorCount = Vector<int>.Count;
            //int[] a = GenerateIntData(numbersOnVectorCount);
            //int[] b = GenerateIntData(numbersOnVectorCount);
            //int[] resArr = new int[a.Length];
            //int[] resVect = new int[a.Length];
            //Stopwatch s = new Stopwatch();
            //
            //s.Restart();
            //SumAsVectors(a, b, resVect, numbersOnVectorCount);
            //s.Stop();
            //Console.WriteLine(s.ElapsedMilliseconds);
            //
            //s.Restart();
            //SumAsArrays(a, b, resArr);
            //s.Stop();
            //Console.WriteLine(s.ElapsedMilliseconds);

        }

        private static void SumAsVectors(int[] a, int[] b, int[] res, int numbersOnVectorCount)
        {
            //for (int i = 0; i < a.Length - numbersOnVectorCount; i += numbersOnVectorCount)
            //{
            //    var va = new Vector<int>(a, i);
            //    var vb = new Vector<int>(b, i);
            //    Vector<int> vRes = va + vb;
            //    vRes.CopyTo(res, i);
            //}
        }

        private static void SumAsArrays(int[] a, int[] b, int[] res)
        {
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] + b[i];
            }
        }

        private static int[] GenerateIntData(int numbersOnVectorCount)
        {
            const int vectorsCount = 10000000;
            int[] numbers = new int[numbersOnVectorCount * vectorsCount];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(int.MinValue, int.MaxValue);
            }
            return numbers;
        }
    }
}
