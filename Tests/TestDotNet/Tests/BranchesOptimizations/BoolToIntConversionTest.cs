using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TestDotNet.Utils;

namespace TestDotNet.Tests.BranchesOptimizations;

public class BoolToIntConversionTest
{
    private const int CountForTest = 8 * 100000;
    private readonly bool[] values;

    private int[] result;

    private readonly bool[] ba;
    private readonly bool[] ba1;
    private readonly bool[] ba2;
    private readonly bool[] ba3;
    private readonly bool[] ba4;
    private readonly bool[] ba5;
    private readonly bool[] ba6;
    private readonly bool[] ba7;
    private readonly int[] ia;
    private readonly int[] ia1;
    private readonly int[] ia2;
    private readonly int[] ia3;
    private readonly int[] ia4;
    private readonly int[] ia5;
    private readonly int[] ia6;
    private readonly int[] ia7;


    public BoolToIntConversionTest()
    {
        result = new int[CountForTest];
        values = RandomHelper.GetBooleans(CountForTest);

        ba = new bool[1];
        ba1 = new bool[1];
        ba2 = new bool[1];
        ba3 = new bool[1];
        ba4 = new bool[1];
        ba5 = new bool[1];
        ba6 = new bool[1];
        ba7 = new bool[1];
        ia = SickSh_tApi.Cast<bool[], int[]>(ba);
        ia1 = SickSh_tApi.Cast<bool[], int[]>(ba1);
        ia2 = SickSh_tApi.Cast<bool[], int[]>(ba2);
        ia3 = SickSh_tApi.Cast<bool[], int[]>(ba3);
        ia4 = SickSh_tApi.Cast<bool[], int[]>(ba4);
        ia5 = SickSh_tApi.Cast<bool[], int[]>(ba5);
        ia6 = SickSh_tApi.Cast<bool[], int[]>(ba6);
        ia7 = SickSh_tApi.Cast<bool[], int[]>(ba7);
    }

    private static Unioun u = new Unioun();
    private int index = 0;

    [Benchmark]
    public int M_Nobrunch()
    {
        index = (index + 1) % CountForTest;
        u.BoolVal = values[index];
        return u.IntVal;
    }

    [Benchmark]
    public int M_Branch()
    {
        index = (index + 1) % CountForTest;
        return Convert.ToInt32(values[index]);
    }


    //[Benchmark]
    public int[] ConvertInlinedWithCondition()
    {
        var result_ = result;
        var values_ = values;

        for (int i = 0; i < result_.Length; i++)
        {
            result_[i] = values_[i] ? 1 : 0;
        }
        return result_;
    }

    //![Benchmark]
    public int ConvertStandartWithCondition()
    {
        int count = 0;
        var values_ = values;

        for (int i = 0; i < values_.Length; i += 8)
        {
            count += Convert.ToInt32(values_[i]) +
                +Convert.ToInt32(values_[i + 1]) +
                +Convert.ToInt32(values_[i + 2]) +
                +Convert.ToInt32(values_[i + 3]) +
                                                  +
                +Convert.ToInt32(values_[i + 4]) +
                +Convert.ToInt32(values_[i + 5]) +
                +Convert.ToInt32(values_[i + 6]) +
                +Convert.ToInt32(values_[i + 7]);
        }
        return count;
    }

    //![Benchmark]
    public int ConvertNoCondition_Method()
    {
        int count = 0;
        var values_ = values;

        unsafe
        {
            for (int i = 0; i < values_.Length; i += 8)
            {
                count += ToInt(values_[i]) +
                +ToInt(values_[i + 1]) +
                +ToInt(values_[i + 2]) +
                +ToInt(values_[i + 3]) +
                +ToInt(values_[i + 4]) +
                +ToInt(values_[i + 5]) +
                +ToInt(values_[i + 6]) +
                +ToInt(values_[i + 7]);
            }
        }
        return count;
    }

    //![Benchmark]
    public int ConvertNoCondition_EnsureInline()
    {
        int count = 0;
        var values_ = values;

        unsafe
        {
            for (int i = 0; i < values_.Length; i += 8)
            {
                bool b = values_[i];
                bool b1 = values_[i + 1];
                bool b2 = values_[i + 2];
                bool b3 = values_[i + 3];
                bool b4 = values_[i + 4];
                bool b5 = values_[i + 5];
                bool b6 = values_[i + 6];
                bool b7 = values_[i + 7];
                count += *(int*)&b +
                        *(int*)&b1 +
                        *(int*)&b2 +
                        *(int*)&b3 +
                        *(int*)&b4 +
                        *(int*)&b5 +
                        *(int*)&b6 +
                        *(int*)&b7;
            }
        }
        return count;
    }

    //![Benchmark]
    public int ConvertWithUnioun()
    {
        //var ba_ = ba;
        //var ia_ = ia;
        var u = new Unioun();
        var u1 = new Unioun();
        var u2 = new Unioun();
        var u3 = new Unioun();
        var u4 = new Unioun();
        var u5 = new Unioun();
        var u6 = new Unioun();
        var u7 = new Unioun();

        int count = 0;
        var values_ = values;

        for (int i = 0; i < values_.Length; i += 8)
        {
            //ba_[0] = values_[i];
            //count += ia_[0];
            u.BoolVal = values_[i];
            u1.BoolVal = values_[i + 1];
            u2.BoolVal = values_[i + 2];
            u3.BoolVal = values_[i + 3];
            u4.BoolVal = values_[i + 4];
            u5.BoolVal = values_[i + 5];
            u6.BoolVal = values_[i + 6];
            u7.BoolVal = values_[i + 7];
            count += u.IntVal +
                    u1.IntVal +
                    u2.IntVal +
                    u3.IntVal +
                    u4.IntVal +
                    u5.IntVal +
                    u6.IntVal +
                    u7.IntVal;
        }
        return count;
    }

    //![Benchmark]
    public int ConvertWithUnioun2()
    {
        var ba_ = ba;
        var ba1_ = ba1;
        var ba2_ = ba2;
        var ba3_ = ba3;
        var ba4_ = ba4;
        var ba5_ = ba5;
        var ba6_ = ba6;
        var ba7_ = ba7;
        var ia_ = ia;
        var ia1_ = ia1;
        var ia2_ = ia2;
        var ia3_ = ia3;
        var ia4_ = ia4;
        var ia5_ = ia5;
        var ia6_ = ia6;
        var ia7_ = ia7;








        int count = 0;
        var values_ = values;

        for (int i = 0; i < values_.Length; i += 8)
        {
            //count += ia_[0];
            ba_[0] = values_[i];
            ba1_[0] = values_[i + 1];
            ba2_[0] = values_[i + 2];
            ba3_[0] = values_[i + 3];
            ba4_[0] = values_[i + 4];
            ba5_[0] = values_[i + 5];
            ba6_[0] = values_[i + 6];
            ba7_[0] = values_[i + 7];
            count += ia[0] +
                    ia1_[0] +
                    ia2_[0] +
                    ia3_[0] +
                    ia4_[0] +
                    ia5_[0] +
                    ia6_[0] +
                    ia7_[0];
        }
        return count;
    }


    internal void Test1()
    {
        int q1 = ConverterHelper.ToInt(true);
        int q2 = ConverterHelper.ToInt(false);

        float w1 = ConverterHelper.ToFloat(true) / float.Epsilon;
        float w2 = ConverterHelper.ToFloat(false);

        double e1 = ConverterHelper.ToDouble(true) / double.Epsilon;
        double e2 = ConverterHelper.ToDouble(false);
    }

    internal void Test2()
    {
        //int[] res1 = (int[])ConvertStandartWithCondition().Clone();
        //int[] res2 = (int[])ConvertInlinedWithCondition().Clone();
        //int[] res3 = (int[])ConvertNoCondition_Method().Clone();
        //int[] res4 = (int[])ConvertNoCondition_EnsureInline().Clone();

        //for (int i = 0; i < res1.Length; i++)
        //{
        //	int pattern = res1[i];
        //	if (pattern == res2[i] && pattern == res3[i] && pattern == res4[i])
        //		continue;
        //	else
        //		throw new NotImplementedException();
        //}
    }

    internal float Test3()
    {
        int trueCount = 0;
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i]) // (fun) !!! this is place for this optimization
                trueCount++;
        }

        return trueCount / (float)values.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe int ToInt(bool boolean)
    {
        //bool copy = boolean;
        return *(int*)&boolean;
    }
}


static class ConverterHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int ToInt(bool boolean)
    {
        bool copy = boolean;
        return *(int*)&copy;
    }

    public static unsafe float ToFloat(bool boolean) => *(float*)&boolean;

    public static unsafe double ToDouble(bool boolean) => *(double*)&boolean;
}

[StructLayout(LayoutKind.Explicit)]
public struct Unioun
{
    [FieldOffset(0)]
    public bool BoolVal;

    [FieldOffset(0)]
    public int IntVal;

    //public Unioun(bool v)
    //{
    //	IntVal = 0;
    //	BoolVal = v;
    //}
}

enum Qwe : byte
{
    Asd,
    Zxc
}
