using System;

namespace Tests
{
    class RandomHelper
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public static float[] GetNumbers(int numbetCount)
        {
            //if (Vector<float>.Count != 4)
            //    throw new Exception("Current hardware suppor simd with count of float != 4. Reconsider test.");
            float[] res = new float[numbetCount];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = random.Next() / (1.0f / int.MaxValue);
            }
            return res;
        }
    }
}
