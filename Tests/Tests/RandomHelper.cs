using System;

namespace Tests
{
    class RandomHelper
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public static float[] GetNumbers(int numbersCount)
        {
            float[] res = new float[numbersCount];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = random.Next() / (1.0f / int.MaxValue);
            }
            return res;
        }
    }
}
