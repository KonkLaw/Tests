using System;
using System.Diagnostics;

namespace MonoTests
{

//sr seq = 18888
//jr seq = 14709
//rr seq = 29483

//sr ran = 29417
//jr ran = 39592
//rr ran = 33346

    class Program
    {
        private const int N = 1000, M = 1000, IterationCount = 40000;
        private int[] single;
        private int[][] jagged;
        private int[,] rectangular;

        private static Random random = new Random();
        private static int[] randomIndeces = new int[N * M / 10];



        private static Stopwatch s = new Stopwatch();
        private const int warupCount = 100;
        private const int targetCount = 200;




        static void Main(string[] args)
        {
            for (int i = 0; i < randomIndeces.Length; i += 2)
            {
                randomIndeces[i + 0] = random.Next(0, N);
                randomIndeces[i + 1] = random.Next(0, M);
            }

            var singleArr = new int[N * M];
            var jaggedArr = new int[N][];
            for (int i = 0; i < jaggedArr.Length; i++)
            {
                jaggedArr[i] = new int[M];
            }
            var rectangularArr = new int[N, M];


            s.Reset();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            int res;
            s.Start();
            //for (int i = 0; i < warupCount; i++)
            {
                //res = SingleRun(singleArr, randomIndeces);
                res = JaggedRun(jaggedArr, randomIndeces);
                //res = RectangularRun(rectangularArr, randomIndeces);
                //res = SingleRun(singleArr);
                //res = JaggedRun(jaggedArr);
                //res = RectangularRun(rectangularArr);
            }
            s.Stop();

            Console.WriteLine(res);

            s.Start();
            //for (int i = 0; i < targetCount; i++)
            {
                //res = SingleRun(singleArr, randomIndeces);
                res = JaggedRun(jaggedArr, randomIndeces);
                //res = RectangularRun(rectangularArr, randomIndeces);
                //res = SingleRun(singleArr);
                //res = JaggedRun(jaggedArr);
                //res = RectangularRun(rectangularArr);
            }
            s.Stop();

            Console.WriteLine(res);

            Console.WriteLine(s.ElapsedMilliseconds);
            Console.ReadKey();
        }



        private static int SingleRun(int[] a)
        {
            int sum = 0;
            for (int iteration = 0; iteration < IterationCount; iteration++)
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < M; j++)
                        sum += a[i * M + j];
            return sum;
        }

        private static int SingleRun(int[] a, int[] randomIndeces)
        {
            int sum = 0;
            for (int iteration = 0; iteration < IterationCount; iteration++)
                for (int i = 0; i < randomIndeces.Length;)
                {
                    int i1 = randomIndeces[i++];
                    int i2 = randomIndeces[i++];
                    sum += a[i1 * M + i2];
                }                        
            return sum;
        }

        private static int JaggedRun(int[][] a)
        {
            int sum = 0;
            for (int iteration = 0; iteration < IterationCount; iteration++)
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < M; j++)
                        sum += a[i][j];
            return sum;
        }

        private static int JaggedRun(int[][] a, int[] randomIndeces)
        {
            int sum = 0;
            for (int iteration = 0; iteration < IterationCount; iteration++)
                for (int i = 0; i < randomIndeces.Length;)
                {
                    int i1 = randomIndeces[i++];
                    int i2 = randomIndeces[i++];
                    sum += a[i1][i2];
                }
            return sum;
        }

        private static int RectangularRun(int[,] a)
        {
            int sum = 0;
            for (int iteration = 0; iteration < IterationCount; iteration++)
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < M; j++)
                        sum += a[i, j];
            return sum;
        }

        private static int RectangularRun(int[,] a, int[] randomIndeces)
        {
            int sum = 0;
            for (int iteration = 0; iteration < IterationCount; iteration++)
                for (int i = 0; i < randomIndeces.Length;)
                {
                    int i1 = randomIndeces[i++];
                    int i2 = randomIndeces[i++];
                    sum += a[i1, i2];
                }
            return sum;
        }
    }
}
