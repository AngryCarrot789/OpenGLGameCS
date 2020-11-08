using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace OpenGLGameCS.Mathematics
{
    public static class MathsExtensions
    {
        public static float Sqrt(this float a)
        {
            return MathF.Sqrt(a);
        }

        public static float Sin(this float a)
        {
            return MathF.Sin(a);
        }

        public static float Cos(this float a)
        {
            return MathF.Cos(a);
        }

        public static float Acos(this float a)
        {
            return MathF.Acos(a);
        }

        public static float Tan(this float a)
        {
            return MathF.Tan(a);
        }

        // Not exactly sure what this does tbh
        public static Vector3 MultiplyDirection(this Matrix4 a, Vector3 b)
        {
            return new Vector3(
                a.M[0] * b.X + a.M[1] * b.Y + a.M[2]  * b.Z,
                a.M[4] * b.X + a.M[5] * b.Y + a.M[6]  * b.Z,
                a.M[8] * b.X + a.M[9] * b.Y + a.M[10] * b.Z);
        }
    }
}
