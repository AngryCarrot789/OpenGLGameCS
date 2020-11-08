using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLGameCS.Mathematics
{
    public class Matrix4
    {
        // This will be a 4x4 matrix stuffed inside a 16 wide array, because
        // this is how opengl actually takes in the view matrixes and stuff
        public float[] M { get; set; }

        public Matrix4()
        {
            MakeZero();
        }

        public Matrix4(float[] m)
        {
            M = m;
        }

        public void Fill(float a)
        {
            M = new float[16];
            for(int i = 0; i < 16; i++)
            {
                M[i] = a;
            }
        }

        public void MakeZero()
        {
            Fill(0);
        }

        public void MakeIdentity()
        {
            // Sets the contents of the matrix to an identity matrix, where there's
            // essentially a diagonal line of 1s going from the X to W... sort of
            M[0]  = 1.0f; M[1]  = 0.0f; M[2]  = 0.0f; M[3]  = 0.0f;
            M[4]  = 0.0f; M[5]  = 1.0f; M[6]  = 0.0f; M[7]  = 0.0f;
            M[8]  = 0.0f; M[9]  = 0.0f; M[10] = 1.0f; M[11] = 0.0f;
            M[12] = 0.0f; M[13] = 0.0f; M[14] = 0.0f; M[15] = 1.0f;
        }

        public void MakeRotationX(float a)
        {
            // Sets the contents of the matrix such that it can
            // form a rotation in the X axis
            M[0]  = 1.0f; M[1]  = 0.0f;    M[2]  = 0.0f;     M[3]  = 0.0f;
            M[4]  = 0.0f; M[5]  = a.Cos(); M[6]  = -a.Sin(); M[7]  = 0.0f;
            M[8]  = 0.0f; M[9]  = a.Sin(); M[10] = a.Cos();  M[11] = 0.0f;
            M[12] = 0.0f; M[13] = 0.0f;    M[14] = 0.0f;     M[15] = 1.0f;
        }

        public void MakeRotationY(float a)
        {
            // same as above but in the Y axis
            M[0]  = a.Cos();  M[1]  = 0.0f; M[2]  = a.Sin(); M[3]  = 0.0f;
            M[4]  = 0.0f;     M[5]  = 1.0f; M[6]  = 0.0f;    M[7]  = 0.0f;
            M[8]  = -a.Sin(); M[9]  = 0.0f; M[10] = a.Cos(); M[11] = 0.0f;
            M[12] = 0.0f;     M[13] = 0.0f; M[14] = 0.0f;    M[15] = 1.0f;
        }

        public void MakeRotationZ(float a)
        {
            // same as above but in the Z axis
            M[0]  = a.Cos(); M[1]  = -a.Sin(); M[2]  = 0.0f; M[3]  = 0.0f;
            M[4]  = a.Sin(); M[5]  = a.Cos();  M[6]  = 0.0f; M[7]  = 0.0f;
            M[8]  = 0.0f;    M[9]  = 0.0f;     M[10] = 1.0f; M[11] = 0.0f;
            M[12] = 0.0f;    M[13] = 0.0f;     M[14] = 0.0f; M[15] = 1.0f;
        }

        public void MakeTranslation(Vector3 t)
        {
            // Moves the contens of the matrix such that a
            // translation "happens"... sort of.
            M[0]  = 1.0f; M[1]  = 0.0f; M[2]  = 0.0f; M[3]  = t.X;
            M[4]  = 0.0f; M[5]  = 1.0f; M[6]  = 0.0f; M[7]  = t.Y;
            M[8]  = 0.0f; M[9]  = 0.0f; M[10] = 1.0f; M[11] = t.Z;
            M[12] = 0.0f; M[13] = 0.0f; M[14] = 0.0f; M[15] = 1.0f;
        }

        public void MakeScale(Vector3 s)
        {
            // Sets the contents to achieve a scale.. .sort of again
            M[0]  = s.X;  M[1]  = 0.0f; M[2]  = 0.0f; M[3]  = 0.0f;
            M[4]  = 0.0f; M[5]  = s.Y;  M[6]  = 0.0f; M[7]  = 0.0f;
            M[8]  = 0.0f; M[9]  = 0.0f; M[10] = s.Z;  M[11] = 0.0f;
            M[12] = 0.0f; M[13] = 0.0f; M[14] = 0.0f; M[15] = 1.0f;
        }

        public Matrix4 Transposed()
        {
            // Returns a copy of the transposed matrix
            // Flips the matrix along a diagonal line. 
            //so the bottom left becomes the top right
            Matrix4 m = new Matrix4();
            m.M[0]  = M[0]; m.M[1]  = M[4]; m.M[2]  = M[8];  m.M[3]  = M[12];
            m.M[4]  = M[1]; m.M[5]  = M[5]; m.M[6]  = M[9];  m.M[7]  = M[13];
            m.M[8]  = M[2]; m.M[9]  = M[6]; m.M[10] = M[10]; m.M[11] = M[14];
            m.M[12] = M[3]; m.M[13] = M[7]; m.M[14] = M[11]; m.M[15] = M[15];
            return m;
        }

        public static Matrix4 Zero()
        {
            Matrix4 m = new Matrix4();
            m.MakeZero();
            return m;
        }

        public static Matrix4 Identity()
        {
            Matrix4 m = new Matrix4();
            m.MakeIdentity();
            return m;
        }

        public static Matrix4 RotateX(float a)
        {
            Matrix4 m = new Matrix4();
            m.MakeRotationX(a);
            return m;
        }

        public static Matrix4 RotateY(float a)
        {
            Matrix4 m = new Matrix4();
            m.MakeRotationY(a);
            return m;
        }

        public static Matrix4 RotateZ(float a)
        {
            Matrix4 m = new Matrix4();
            m.MakeRotationZ(a);
            return m;
        }

        public static Matrix4 Translate(Vector3 v)
        {
            Matrix4 m = new Matrix4();
            m.MakeTranslation(v);
            return m;
        }

        public static Matrix4 Scale(Vector3 v)
        {
            Matrix4 m = new Matrix4();
            m.MakeScale(v);
            return m;
        }

        public static Matrix4 Scale(float a)
        {
            Matrix4 m = new Matrix4();
            m.MakeScale(new Vector3(a));
            return m;
        }

        #region Operators

        // a big operation where the matrixes are multiplied by eachother.
        // where the rows and colums are all multiplied and added together
        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            Matrix4 m = new Matrix4();
            m.M[0] =  b.M[0] * a.M[0]  + b.M[4] * a.M[1]  + b.M[8]  * a.M[2] + b.M[12]  * a.M[3];
            m.M[1] =  b.M[1] * a.M[0]  + b.M[5] * a.M[1]  + b.M[9]  * a.M[2] + b.M[13]  * a.M[3];
            m.M[2] =  b.M[2] * a.M[0]  + b.M[6] * a.M[1]  + b.M[10] * a.M[2] + b.M[14]  * a.M[3];
            m.M[3] =  b.M[3] * a.M[0]  + b.M[7] * a.M[1]  + b.M[11] * a.M[2] + b.M[15]  * a.M[3];
                                                                                       
            m.M[4] =  b.M[0] * a.M[4]  + b.M[4] * a.M[5]  + b.M[8]  * a.M[6] + b.M[12]  * a.M[7];
            m.M[5] =  b.M[1] * a.M[4]  + b.M[5] * a.M[5]  + b.M[9]  * a.M[6] + b.M[13]  * a.M[7];
            m.M[6] =  b.M[2] * a.M[4]  + b.M[6] * a.M[5]  + b.M[10] * a.M[6] + b.M[14]  * a.M[7];
            m.M[7] =  b.M[3] * a.M[4]  + b.M[7] * a.M[5]  + b.M[11] * a.M[6] + b.M[15]  * a.M[7];
                                                          
            m.M[8] =  b.M[0] * a.M[8]  + b.M[4] * a.M[9]  + b.M[8]  * a.M[10] + b.M[12] * a.M[11];
            m.M[9] =  b.M[1] * a.M[8]  + b.M[5] * a.M[9]  + b.M[9]  * a.M[10] + b.M[13] * a.M[11];
            m.M[10] = b.M[2] * a.M[8]  + b.M[6] * a.M[9]  + b.M[10] * a.M[10] + b.M[14] * a.M[11];
            m.M[11] = b.M[3] * a.M[8]  + b.M[7] * a.M[9]  + b.M[11] * a.M[10] + b.M[15] * a.M[11];

            m.M[12] = b.M[0] * a.M[12] + b.M[4] * a.M[13] + b.M[8]  * a.M[14] + b.M[12] * a.M[15];
            m.M[13] = b.M[1] * a.M[12] + b.M[5] * a.M[13] + b.M[9]  * a.M[14] + b.M[13] * a.M[15];
            m.M[14] = b.M[2] * a.M[12] + b.M[6] * a.M[13] + b.M[10] * a.M[14] + b.M[14] * a.M[15];
            m.M[15] = b.M[3] * a.M[12] + b.M[7] * a.M[13] + b.M[11] * a.M[14] + b.M[15] * a.M[15];
            return m;
        }

        #endregion
    }
}
