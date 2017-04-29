using System;

namespace Tests
{
    class RandomHelper
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public static float[] GetFloatNumbers(int numbersCount)
        {
            float[] result = new float[numbersCount];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Random.Next() / (1.0f / int.MaxValue);
            }
            return result;
        }

		public static double[] GetDoubleNumbers(int numbersCount)
		{
			double[] result = new double[numbersCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Random.NextDouble();
			}
			return result;
		}

		public static long[] GetLongNumbers(int numbersCount)
		{
			long[] result = new long[numbersCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Random.Next() | (Random.Next() << 32);
			}
			return result;
		}

		public static int[] GetIntNumbers(int numbersCount)
		{
			int[] result = new int[numbersCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Random.Next();
			}
			return result;
		}

		public static int[] GetIntNumbers(int numbersCount, int minValue, int maxValue)
		{
			int[] result = new int[numbersCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Random.Next(minValue, maxValue);
			}
			return result;
		}

	    public static bool GetRandomBool() => Random.Next() % 2 == 0;
    }
}
