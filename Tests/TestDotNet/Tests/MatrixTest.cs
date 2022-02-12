using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests;

public class MatrixTest
{
    private const int MatricesCount = 10000;

    private Matrix4F[] matricesSource;
    private Matrix4FDuplicate[] matricesDestination;

    private Matrix4F matr = new Matrix4F();

    public MatrixTest()
    {
        matricesSource = new Matrix4F[MatricesCount];
        matricesDestination = new Matrix4FDuplicate[MatricesCount];
    }

    //[Benchmark]
    public Matrix4FDuplicate[] TestSimpleCopy()
    {
        Matrix4F[] matricesSource__ = matricesSource;
        Matrix4FDuplicate[] matricesDestination__ = matricesDestination;

        for (int i = 0; i < matricesSource__.Length; i++)
        {
            matricesDestination__[i] = Matrix4F.ToMatrix4F(matricesSource__[i]);
        }
        return matricesDestination__;
    }

    //[Benchmark]
    public Matrix4FDuplicate[] TestTrickyCopy()
    {
        Matrix4F[] matricesSource__ = matricesSource;
        Matrix4FDuplicate[] matricesDestination__ = matricesDestination;

        for (int i = 0; i < matricesSource__.Length; i++)
        {
            matricesDestination__[i] = Matrix4F.ToMatrix4FQuick(matricesSource__[i]);
        }
        return matricesDestination__;
    }

    [Benchmark]
    public Matrix4FDuplicate TestSimpleCopy1()
    {
        return Matrix4F.ToMatrix4F(matr);
    }

    [Benchmark]
    public Matrix4FDuplicate TestTrickyCopy1()
    {
        return Matrix4F.ToMatrix4FQuick(matr);
    }
}
