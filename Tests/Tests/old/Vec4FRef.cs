namespace SimdTest
{
	struct Vec4FRef
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public Vec4FRef(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static Vec4FRef operator + (Vec4FRef a, Vec4FRef b)
		{
			return new Vec4FRef(
				a.X + b.X,
				a.Y + b.Y,
				a.Z + b.Z,
				a.W + b.W);
		}
	}
}