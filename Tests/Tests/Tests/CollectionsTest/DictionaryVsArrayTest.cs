using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Tests.HelpersTypes;

// RESULTS:
//
//
// COUNT = 100
//BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 1 (10.0.14393)
//Processor=Intel Core i5-2500 CPU 3.30GHz(Sandy Bridge), ProcessorCount=4
//Frequency=3233206 Hz, Resolution=309.2905 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//
//
//            Method |     Mean |     Error |    StdDev |
//------------------ |---------:|----------:|----------:|
// RunWithDictionary | 1.684 us | 0.0019 us | 0.0018 us |
//      RunWithArray | 7.534 us | 0.0070 us | 0.0065 us |
//
//
//
// COUNT = 30
//BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 1 (10.0.14393)
//Processor=Intel Core i5-2500 CPU 3.30GHz(Sandy Bridge), ProcessorCount=4
//Frequency=3233206 Hz, Resolution=309.2905 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//
//
//            Method |     Mean |     Error |    StdDev |
//------------------ |---------:|----------:|----------:|
// RunWithDictionary | 480.5 ns | 0.5602 ns | 0.5240 ns |
//      RunWithArray | 882.6 ns | 0.8305 ns | 0.7769 ns |
//
//
//
// COUNT = 17
//BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 1 (10.0.14393)
//Processor=Intel Core i5-2500 CPU 3.30GHz(Sandy Bridge), ProcessorCount=4
//Frequency=3233206 Hz, Resolution=309.2905 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//
//
//            Method |     Mean |     Error |    StdDev |
//------------------ |---------:|----------:|----------:|
// RunWithDictionary | 270.8 ns | 0.2621 ns | 0.2452 ns |
//      RunWithArray | 275.8 ns | 5.5552 ns | 5.7048 ns |
//
//
// COUNT = 5
//BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 1 (10.0.14393)
//Processor=Intel Core i5-2500 CPU 3.30GHz(Sandy Bridge), ProcessorCount=4
//Frequency=3233206 Hz, Resolution=309.2905 ns, Timer=TSC
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2053.0
//
//
//            Method |     Mean |     Error |    StdDev |
//------------------ |---------:|----------:|----------:|
// RunWithDictionary | 78.00 ns | 0.1320 ns | 0.1235 ns |
//      RunWithArray | 29.02 ns | 0.0236 ns | 0.0221 ns |

namespace Tests.Tests
{
    public class DictionaryVsArrayTest
    {
        private readonly Dictionary<int, int> dictionary;
        private readonly int[] unsortedKeys;
        private readonly KeyValuePair[] pairsArray;

        public DictionaryVsArrayTest()
        {
            const int testCount = 17;
            int[] keys = RandomHelper.GetIntNumbers(testCount);
            int[] values = RandomHelper.GetIntNumbers(testCount);

            dictionary = new Dictionary<int,int>(testCount);
            pairsArray = new KeyValuePair[testCount];
            for (int i = 0; i < values.Length; i++)
            {
                var pair = new KeyValuePair
                {
                    Key = keys[i],
                    Value = values[i]
                };
                dictionary.Add(pair.Key, pair.Value);
                pairsArray[i] = pair;
            }
            RandomHelper.FisherYatesShuffle(keys);
            unsortedKeys = keys;
        }

        [Benchmark]
        public int RunWithDictionary()
        {
            int fakeSum = 0;
            for (int i = 0; i < unsortedKeys.Length; i++)
            {
                fakeSum += dictionary[unsortedKeys[i]];
            }
            return fakeSum;
        }

        [Benchmark]
        public int RunWithArray()
        {
            int fakeSum = 0;
            for (int sumIndex = 0; sumIndex < unsortedKeys.Length; sumIndex++)
            {
                for (int searchIndex = 0; searchIndex < unsortedKeys.Length; searchIndex++)
                {
                    KeyValuePair pair = pairsArray[searchIndex];
                    if (pair.Key == unsortedKeys[sumIndex])
                    {
                        fakeSum += pair.Value;
                        break;
                    }
                }
            }
            return fakeSum;
        }
    }

    struct KeyValuePair
    {
        public int Key;
        public int Value;


        public override bool Equals(object obj)
        {
            // shouldn't be called
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            // shouldn't be called
            throw new NotImplementedException();
        }
    }
}