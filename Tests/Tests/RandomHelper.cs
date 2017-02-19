using System;

namespace Tests
{
    class RandomHelper
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public static float[] GetFloatNumbers(int numbersCount)
        {
            float[] result = new float[numbersCount];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = random.Next() / (1.0f / int.MaxValue);
            }
            return result;
        }

		public static int[] GetIntNumbers(int numbersCount)
		{
			int[] result = new int[numbersCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = random.Next();
			}
			return result;
		}
    }
}
