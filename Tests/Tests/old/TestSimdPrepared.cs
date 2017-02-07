using System;
using System.Diagnostics;
using System.Numerics;
using Tests;

namespace SimdTest
{
    static class TestSimdPrepared
    {
        internal static void Run()
        {
            const int vectroCount = 10000000;
            Vec4FVal[] vecav = GetVectrosVal(vectroCount);
            Vec4FVal[] vecbv = GetVectrosVal(vectroCount);
            Vec4FVal[] vecresv = new Vec4FVal[vectroCount];

            Vec4FRef[] vecar = GetVectrosRef(vectroCount);
            Vec4FRef[] vecbr = GetVectrosRef(vectroCount);
            Vec4FRef[] vecresr = new Vec4FRef[vectroCount];

            Vector4[] vecsimda = GetVectrosGet(vectroCount);
            Vector4[] vecsimdb = GetVectrosGet(vectroCount);
            Vector4[] vecsimdres = new Vector4[vectroCount];

            Stopwatch s = new Stopwatch();

            s.Restart();
            for (int i = 0; i < vecresr.Length; i++)
            {
                vecresr[i] = vecar[i] + vecbr[i];
            }
            s.Stop();
			GC.Collect();
			GC.WaitForPendingFinalizers();
            Console.WriteLine(s.ElapsedMilliseconds);


			s.Restart();
			for (int i = 0; i < vecresv.Length; i++)
			{
			    vecresv[i] = vecav[i] + vecbv[i];
			}
			s.Stop();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			Console.WriteLine(s.ElapsedMilliseconds);


			s.Restart();
            for (int i = 0; i < vecsimdres.Length; i++)
            {
                vecsimdres[i] = vecsimda[i] + vecsimdb[i];
            }
            s.Stop();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			Console.WriteLine(s.ElapsedMilliseconds);
        }

        private static Vector4[] GetVectrosGet(int vectroCount)
        {
            var floats = RandomHelper.GetNumbers(vectroCount * 4);
            var res = new Vector4[vectroCount];
            for (int i = 0; i < floats.Length; i += 4)
            {
                res[(i + 1) / 4] = new Vector4(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
            }
            return res;
        }

        private static Vec4FVal[] GetVectrosVal(int vectroCount)
        {
            var floats = RandomHelper.GetNumbers(vectroCount * 4);
            var res = new Vec4FVal[vectroCount];
            for (int i = 0; i < floats.Length; i += 4)
            {
                res[(i + 1) / 4] = new Vec4FVal(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
            }
            return res;
        }

        private static Vec4FRef[] GetVectrosRef(int vectroCount)
        {
            var floats = RandomHelper.GetNumbers(vectroCount * 4);
            var res = new Vec4FRef[vectroCount];
            for (int i = 0; i < floats.Length; i += 4)
            {
                res[(i + 1) / 4] = new Vec4FRef(floats[i], floats[i + 1], floats[i + 2], floats[i + 3]);
            }
            return res;
        }
    }
}
