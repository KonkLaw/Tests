namespace Tests.Tests.BaseDataTypes
{
	public struct Matrix4F
	{
		public float M11;
		public float M12;
		public float M13;
		public float M14;
		public float M21;
		public float M22;
		public float M23;
		public float M24;
		public float M31;
		public float M32;
		public float M33;
		public float M34;
		public float M41;
		public float M42;
		public float M43;
		public float M44;

		public static Vector3F operator *(Matrix4F m, Vector3F v)
		{
			return new Vector3F
			(
				m.M11 * v.X + m.M21 * v.Y + m.M31 * v.Z + m.M41,
				m.M12 * v.X + m.M22 * v.Y + m.M32 * v.Z + m.M42,
				m.M13 * v.X + m.M23 * v.Y + m.M33 * v.Z + m.M43
			);
			float w = 1f / ((v.X * m.M14) + (v.Y * m.M24) + (v.Z * m.M34) + m.M44);
			return new Vector3F
			(
				((m.M11 * v.X + m.M21 * v.Y + m.M31 * v.Z + m.M41) * w),
				((m.M12 * v.X + m.M22 * v.Y + m.M32 * v.Z + m.M42) * w),
				((m.M13 * v.X + m.M23 * v.Y + m.M33 * v.Z + m.M43) * w)
			);
		}

		public static Matrix4FDuplicate ToMatrix4F(Matrix4F m)
		{
			return new Matrix4FDuplicate
			{
				M11 = m.M11,
				M12 = m.M12,
				M13 = m.M13,
				M14 = m.M14,
				M21 = m.M21,
				M22 = m.M22,
				M23 = m.M23,
				M24 = m.M24,
				M31 = m.M31,
				M32 = m.M32,
				M33 = m.M33,
				M34 = m.M34,
				M41 = m.M41,
				M42 = m.M42,
				M43 = m.M43,
				M44 = m.M44,
			};
		}

		public static Matrix4FDuplicate ToMatrix4FQuick(Matrix4F m)
		{
			unsafe
			{
				return *((Matrix4FDuplicate*)&m);
			}
		}
	}

	public struct Matrix4FDuplicate
	{
		public float M11;
		public float M12;
		public float M13;
		public float M14;
		public float M21;
		public float M22;
		public float M23;
		public float M24;
		public float M31;
		public float M32;
		public float M33;
		public float M34;
		public float M41;
		public float M42;
		public float M43;
		public float M44;
	}
}