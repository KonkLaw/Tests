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
	    	
		public static bool[] GetBooleans(int numbersCount)
		{
			bool[] result = new bool[numbersCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Random.Next(2) == 0;
			}
			return result;
		}


        public static int[] GenerateRandomSequence_FisherYatesShuffle(int maxExclusive)
        {
            var numbers = new int[maxExclusive - 1];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i;
            }
            FisherYatesShuffle(numbers);
            return numbers;
        }

        public static void FisherYatesShuffle(int[] input)
        {
            int indexForSwap = input.Length - 1; // Last number;
            while(indexForSwap > 0)
            {
                int randomIndex = Random.Next(0, indexForSwap + 1);

                int selectedNumber = input[randomIndex];
                input[randomIndex] = input[indexForSwap];
                input[indexForSwap] = selectedNumber;
                indexForSwap--;
            }
        }
    }
}
