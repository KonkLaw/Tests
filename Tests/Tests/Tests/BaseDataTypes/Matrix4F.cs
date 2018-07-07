using System;

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

		public Vector3F MultiplyNoDivision(in Vector3F v)
		{
			return new Vector3F
			(
				M11 * v.X + M21 * v.Y + M31 * v.Z + M41,
				M12 * v.X + M22 * v.Y + M32 * v.Z + M42,
				M13 * v.X + M23 * v.Y + M33 * v.Z + M43
			);
		}

		public Vector3F MultiplyWithDivision(in Vector3F v)
		{
			float w = 1f / ((v.X * M14) + (v.Y * M24) + (v.Z * M34) + M44);
			return new Vector3F
			(
				((M11 * v.X + M21 * v.Y + M31 * v.Z + M41) * w),
				((M12 * v.X + M22 * v.Y + M32 * v.Z + M42) * w),
				((M13 * v.X + M23 * v.Y + M33 * v.Z + M43) * w)
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

		public static void Invert(Matrix4F value, out Matrix4F res)
		{
			float b0 = (value.M31 * value.M42) - (value.M32 * value.M41);
			float b1 = (value.M31 * value.M43) - (value.M33 * value.M41);
			float b2 = (value.M34 * value.M41) - (value.M31 * value.M44);
			float b3 = (value.M32 * value.M43) - (value.M33 * value.M42);
			float b4 = (value.M34 * value.M42) - (value.M32 * value.M44);
			float b5 = (value.M33 * value.M44) - (value.M34 * value.M43);

			float d11 = value.M22 * b5 + value.M23 * b4 + value.M24 * b3;
			float d12 = value.M21 * b5 + value.M23 * b2 + value.M24 * b1;
			float d13 = value.M21 * -b4 + value.M22 * b2 + value.M24 * b0;
			float d14 = value.M21 * b3 + value.M22 * -b1 + value.M23 * b0;

			float det = value.M11 * d11 - value.M12 * d12 + value.M13 * d13 - value.M14 * d14;
			if (Math.Abs(det) <= 1e-15)
			{
				res = new Matrix4F();
			}

			det = 1f / det;

			float a0 = (value.M11 * value.M22) - (value.M12 * value.M21);
			float a1 = (value.M11 * value.M23) - (value.M13 * value.M21);
			float a2 = (value.M14 * value.M21) - (value.M11 * value.M24);
			float a3 = (value.M12 * value.M23) - (value.M13 * value.M22);
			float a4 = (value.M14 * value.M22) - (value.M12 * value.M24);
			float a5 = (value.M13 * value.M24) - (value.M14 * value.M23);

			float d21 = value.M12 * b5 + value.M13 * b4 + value.M14 * b3;
			float d22 = value.M11 * b5 + value.M13 * b2 + value.M14 * b1;
			float d23 = value.M11 * -b4 + value.M12 * b2 + value.M14 * b0;
			float d24 = value.M11 * b3 + value.M12 * -b1 + value.M13 * b0;

			float d31 = value.M42 * a5 + value.M43 * a4 + value.M44 * a3;
			float d32 = value.M41 * a5 + value.M43 * a2 + value.M44 * a1;
			float d33 = value.M41 * -a4 + value.M42 * a2 + value.M44 * a0;
			float d34 = value.M41 * a3 + value.M42 * -a1 + value.M43 * a0;

			float d41 = value.M32 * a5 + value.M33 * a4 + value.M34 * a3;
			float d42 = value.M31 * a5 + value.M33 * a2 + value.M34 * a1;
			float d43 = value.M31 * -a4 + value.M32 * a2 + value.M34 * a0;
			float d44 = value.M31 * a3 + value.M32 * -a1 + value.M33 * a0;

			res = new Matrix4F
			{
				M11 = +d11 * det,
				M12 = -d21 * det,
				M13 = +d31 * det,
				M14 = -d41 * det,
				M21 = -d12 * det,
				M22 = +d22 * det,
				M23 = -d32 * det,
				M24 = +d42 * det,
				M31 = +d13 * det,
				M32 = -d23 * det,
				M33 = +d33 * det,
				M34 = -d43 * det,
				M41 = -d14 * det,
				M42 = +d24 * det,
				M43 = -d34 * det,
				M44 = +d44 * det
			};
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