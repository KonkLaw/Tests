using System;
using System.Diagnostics;
using System.Numerics;

namespace SimdTest
{
    static class TestFloat
    { 
        internal static void Run()
        {
            //const int vectorsCount = 10000000;
            //int numbersOnVectorCount = Vector<float>.Count;
            //float[] a = RandomHelper.GetNumbers(numbersOnVectorCount * vectorsCount);
            //float[] b = RandomHelper.GetNumbers(numbersOnVectorCount * vectorsCount);
            //float[] resVect = new float[a.Length];
            //float[] resArr = new float[a.Length];
            //
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

        private static void SumAsVectors(float[] a, float[] b, float[] resVect, int numbersOnVectorCount)
        {
            //for (int i = 0; i < a.Length; i += numbersOnVectorCount)
            //{
            //    Vector<float> av = new Vector<float>(a, i);
            //    Vector<float> bv = new Vector<float>(b, i);
            //    Vector<float> rv = av + bv;
            //    rv.CopyTo(resVect, i);
            //    //(av + bv).CopyTo(resVect, i);
            //    //Vector4 av = new Vector4(a[i], a[i+1], a[i+2], a[i+3]);
            //    //Vector4 bv = new Vector4(b[i], b[i+1], b[i+2], b[i+3]);
            //    //(av + bv).CopyTo(resVect, i);
            //}
        }

        private static void SumAsArrays(float[] a, float[] b, float[] resArr)
        {
            for (int i = 0; i < a.Length; i++)
            {
                resArr[i] = a[i] + b[i];
                //resArr[i + 1] = a[i + 1] + b[i + 1];
                //resArr[i + 2] = a[i + 2] + b[i + 2];
                //resArr[i + 3] = a[i + 3] + b[i + 3];
            }
        }
    }
}