﻿namespace TestDotNet.Utils;

class RandomHelper
{
    private static readonly Random Random = new Random(DateTime.Now.Millisecond);

    public static float GetFloat() => Random.NextSingle();

    public static float GetFloat(float min, float max)
        => min + Random.NextSingle() * (max - min);

    public static float[] GetFloatNumbers(int numbersCount, float min = 0, float max = 1f)
    {
        var result = new float[numbersCount];
        for (int i = 0; i < result.Length; i++)
        {
	        result[i] = GetFloat(min: min, max: max);
        }
        return result;
    }

    public static Vector3F GetVector(float max, float min)
        => new Vector3F(GetFloat(min: min, max: max), GetFloat(min: min, max: max), GetFloat(min: min, max: max));

    public static Vector3F[] GetVectors(int count, float max, float min)
    {
        var vectors = new Vector3F[count];
        for (int i = 0; i < count; i++)
            vectors[i] = GetVector(max, min);
        return vectors;
    }

    public static Vector2F[] GetVectors2F(int count, float max, float min)
    {
        var vectors = new Vector2F[count];
        for (int i = 0; i < count; i++)
        {
            vectors[i] = new Vector2F(
                GetFloat(min: min, max: max),
                GetFloat(min: min, max: max));
        }
        return vectors;
    }

    public static double[] GetDoubleNumbers(int numbersCount)
    {
        var result = new double[numbersCount];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Random.NextDouble();
        }
        return result;
    }

    public static long[] GetLongNumbers(int numbersCount)
    {
        var result = new long[numbersCount];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Random.Next() | Random.Next() << 32;
        }
        return result;
    }

    public static int[] GetIntNumbers(int numbersCount)
    {
        var result = new int[numbersCount];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Random.Next();
        }
        return result;
    }

    public static int[] GetIntNumbers(int numbersCount, int minValueInclusive, int maxValueExclusive)
    {
        var result = new int[numbersCount];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Random.Next(minValueInclusive, maxValueExclusive);
        }
        return result;
    }

    public static int GetIntNumber(int minValueInclusive, int maxValueExclusive) => Random.Next(minValueInclusive, maxValueExclusive);

    public static bool GetRandomBool() => Random.Next() % 2 == 0;

    public static bool[] GetBooleans(int numbersCount)
    {
        var result = new bool[numbersCount];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Random.Next(2) == 0;
        }
        return result;
    }

    public static T GetByRandom<T>(Func<T>[] factories) => factories[Random.Next(0, factories.Length)]();

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
        while (indexForSwap > 0)
        {
            int randomIndex = Random.Next(0, indexForSwap + 1);

            int selectedNumber = input[randomIndex];
            input[randomIndex] = input[indexForSwap];
            input[indexForSwap] = selectedNumber;
            indexForSwap--;
        }
    }
}
