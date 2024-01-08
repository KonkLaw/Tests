using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace TestDotNet.Utils;

public class Vector4FRef
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public Vector4FRef(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Vector4FRef operator +(Vector4FRef a, Vector4FRef b)
        => new Vector4FRef(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z,
            a.W + b.W);
}

public struct Vector4FVal
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public Vector4FVal(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Vector4FVal operator +(Vector4FVal a, Vector4FVal b)
        => new Vector4FVal(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z,
            a.W + b.W);
}

//[StructLayout(LayoutKind.Explicit)]
public struct Vector3F
{
    public const int ComponetsCount = 3;
    //[FieldOffset(0)]
    public float X;
    //[FieldOffset(sizeof(float))]
    public float Y;
    //[FieldOffset(2 * sizeof(float))]
    public float Z;

    //[FieldOffset(0)]
    //public fixed float fields[ComponetsCount];

    public Vector3F(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3F(float value)
    {
        X = Y = Z = value;
    }


    public static Vector3F operator +(Vector3F a, Vector3F b)
        => new Vector3F(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z);

    public static Vector3F operator -(Vector3F a, Vector3F b)
    => new Vector3F(
        a.X - b.X,
        a.Y - b.Y,
        a.Z - b.Z);

    public static Vector3F operator /(Vector3F a, float scalar)
        => new Vector3F(
            a.X / scalar,
            a.Y / scalar,
            a.Z / scalar);

    public Vector3F GetAbs() => new Vector3F(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

    public static Vector3F Cross(Vector3F v1, Vector3F v2)
        => new Vector3F(v1.Y * v2.Z - v1.Z * v2.Y, -(v1.X * v2.Z - v1.Z * v2.X), v1.X * v2.Y - v1.Y * v2.X);

    /// <summary>
    /// Gets result of the vector normalization.
    /// </summary>
    /// <returns>Normalized vector.</returns>
    public readonly Vector3F GetNormalized()
    {
        float inverseLength = GetInverseLength();
        return new Vector3F(X * inverseLength, Y * inverseLength, Z * inverseLength);
    }

    /// <summary>
    /// Calculates inverse vector length.
    /// </summary>
    /// <returns>Inverse vector length.</returns>
    public readonly float GetInverseLength() => (float)(1f / Math.Sqrt(X * X + Y * Y + Z * Z));
}

public struct Vector2F
{
    public float X;
    public float Y;

    public Vector2F(float value)
    {
        X = Y = value;
    }

    public Vector2F(float x, float y)
    {
        X = x;
        Y = y;
    }
}

public struct Bounds
{
    public Vector3F Min;
    public Vector3F Max;

    public Bounds(Vector3F position)
    {
        Min = position;
        Max = position;
    }

    public static Bounds GetInvalid()
    {
        return new Bounds
        {
            Max = new Vector3F(float.MinValue),
            Min = new Vector3F(float.MaxValue),
        };
    }

    private void Merge(ref Bounds bounds)
    {
        if (bounds.Min.X < Min.X)
            Min.X = bounds.Min.X;
        if (bounds.Min.Y < Min.Y)
            Min.Y = bounds.Min.Y;
        if (bounds.Min.Z < Min.Z)
            Min.Z = bounds.Min.Z;
        if (bounds.Max.X > Max.X)
            Max.X = bounds.Max.X;
        if (bounds.Max.Y > Max.Y)
            Max.Y = bounds.Max.Y;
        if (bounds.Max.Z > Max.Z)
            Max.Z = bounds.Max.Z;
    }

    public void MergeTransformed(ref Bounds bounds, ref Matrix4F transform)
    {
        Vector3F center = (bounds.Max + bounds.Min) / 2;
        Vector3F halfSize = bounds.Max - center;
        center = transform.MultiplyWithDivision(ref center);

        Vector3F vX = new Vector3F(halfSize.X, 0, 0);
        Vector3F vY = new Vector3F(0, halfSize.Y, 0);
        Vector3F vZ = new Vector3F(0, 0, halfSize.Z);

        vX = Matrix4F.TransformVector(ref vX, ref transform).GetAbs();
        vY = Matrix4F.TransformVector(ref vY, ref transform).GetAbs();
        vZ = Matrix4F.TransformVector(ref vZ, ref transform).GetAbs();

        Vector3F abs = vX + vY + vZ;

        Vector3F newMin = center - abs;
        Vector3F newMax = center + abs;


        if (newMin.X < Min.X)
            Min.X = newMin.X;
        if (newMin.Y < Min.Y)
            Min.Y = newMin.Y;
        if (newMin.Z < Min.Z)
            Min.Z = newMin.Z;
        if (newMax.X > Max.X)
            Max.X = newMax.X;
        if (newMax.Y > Max.Y)
            Max.Y = newMax.Y;
        if (newMax.Z > Max.Z)
            Max.Z = newMax.Z;
    }
}