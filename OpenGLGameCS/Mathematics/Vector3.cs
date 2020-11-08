using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLGameCS.Mathematics
{
    /// <summary>
    /// A Vector3 can be used to represent coordinates in 3D space
    /// </summary>
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static Vector3 Zero => new Vector3(0);
        public static Vector3 Ones => new Vector3(1);

        public Vector3(float x, float y, float z)
        {
            Set(x, y, z);
        }

        public Vector3(float a)
        {
            Set(a, a, a);
        }

        public Vector3(Vector3 prev)
        {
            Set(prev.X, prev.Y, prev.Z);
        }

        public void Set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void SetZero() => Set(0,0,0);
        public void SetOnes() => Set(1, 1, 1);

        public float MagnitudeSquared()
        {
            // this is how magnitude is calculated
            return X * X + Y * Y + Z * Z;
        }

        public float Magnitude()
        {
            // Magnitude is the square root of the squared magnitude...
            return MagnitudeSquared().Sqrt();
        }

        public void Normalise()
        {
            Vector3 v = new Vector3(this);
            v /= Magnitude();
            Set(v.X, v.Y, v.Z);
        }

        // Return a normalised copy
        public Vector3 Normalised()
        {
            return new Vector3(this) / Magnitude();
        }

        public void ClipMagnitude(float m)
        {
            if (m > 0)
            {
                float r = MagnitudeSquared() / (m * m);
                if (r > 1)
                {
                    Vector3 v = new Vector3(this);
                    v /= r.Sqrt();
                    Set(v.X, v.Y, v.Z);
                }
            }
        }

        public Vector3 Cross(Vector3 b)
        {
            return new Vector3(
                Y * b.Z - Z * b.Y,
                Z * b.X - X * b.Z,
                X * b.Y - Y * b.X);
        }

        #region Operators

        public static Vector3 operator +(Vector3 l, Vector3 r)
        {
            return new Vector3(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
        }

        public static Vector3 operator -(Vector3 l, Vector3 r)
        {
            return new Vector3(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
        }

        public static Vector3 operator *(Vector3 l, Vector3 r)
        {
            return new Vector3(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
        }

        public static Vector3 operator /(Vector3 l, Vector3 r)
        {
            return new Vector3(l.X / r.X, l.Y / r.Y, l.Z / r.Z);
        }

        public static Vector3 operator +(float a, Vector3 v)
        {
            return new Vector3(v.X + a, v.Y + a, v.Z + a);
        }

        public static Vector3 operator +(Vector3 v, float a)
        {
            return new Vector3(v.X + a, v.Y + a, v.Z + a);
        }

        public static Vector3 operator -(float a, Vector3 v)
        {
            return new Vector3(v.X - a, v.Y - a, v.Z - a);
        }

        public static Vector3 operator -(Vector3 v, float a)
        {
            return new Vector3(v.X - a, v.Y - a, v.Z - a);
        }

        public static Vector3 operator *(float a, Vector3 v)
        {
            return new Vector3(v.X * a, v.Y * a, v.Z * a);
        }

        public static Vector3 operator *(Vector3 v, float a)
        {
            return new Vector3(v.X * a, v.Y * a, v.Z * a);
        }

        public static Vector3 operator /(float a, Vector3 v)
        {
            return new Vector3(v.X / a, v.Y / a, v.Z / a);
        }

        public static Vector3 operator /(Vector3 v, float a)
        {
            return new Vector3(v.X / a, v.Y / a, v.Z / a);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        #endregion

        public override string ToString()
        {
            return $"{X}  {Y}  {Z}";
        }
    }
}
