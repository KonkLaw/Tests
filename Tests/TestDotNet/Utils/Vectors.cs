using System.Runtime.InteropServices;

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
}