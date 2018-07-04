namespace Tests.Tests.BaseDataTypes
{
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

		public static Vector4FRef operator + (Vector4FRef a, Vector4FRef b)
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

	public struct Vector3F
	{
		public const int ComponetsCount = 3;

		public float X;
		public float Y;
		public float Z;

		public Vector3F(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}