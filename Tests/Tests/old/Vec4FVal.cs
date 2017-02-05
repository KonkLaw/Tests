namespace SimdTest
{
	struct Vec4FVal
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public Vec4FVal(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static Vec4FVal operator +(Vec4FVal a, Vec4FVal b)
		{
			return new Vec4FVal(
				a.X + b.X,
				a.Y + b.Y,
				a.Z + b.Z,
				a.W + b.W);
		}
	}
}