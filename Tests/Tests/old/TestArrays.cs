using BenchmarkDotNet.Attributes;

namespace SimdTest
{
	public class TestArrays
	{
        private int count;
        float[] a;
        float[] b; 
        float[] res1;
        float[] res2;

        public TestArrays()
        {
            count = 100000000;
            a = RandomHelper.GetNumbers(count);
            b = RandomHelper.GetNumbers(count);
            res1 = new float[count];
            res2 = new float[count];
        }

        [Benchmark]
        public float[] Sum_slow()
        {
            for (int i = 0; i < count; i++)
            {
                res1[i] = a[i] + b[i];
            }
            return res1;
        }

        [Benchmark]
        public float[] Sum_fast()
        {
            // This caching is REALLY important.
            float[] a_ = a;
            float[] b_ = b;
            float[] res2_ = res2;

            for (int i = 0; i < a_.Length; i++)
            {
                res2_[i] = a_[i] + b_[i];
            }
            return res2_;
        }
    }
}