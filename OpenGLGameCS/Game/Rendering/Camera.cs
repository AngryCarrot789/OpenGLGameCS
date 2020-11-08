using OpenGLGameCS.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLGameCS.Game.Rendering
{
    public class Camera
    {
        // Size of the viewport
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        // Near = the closest a vertex can be before not being rendered
        public float Near { get; set; }
        // Far = rendering distance basically. if vertex is too far away, dont render.
        public float Far { get; set; }
        // Fov is how much is seen on screen sort of.
        public float FOV { get; set; }

        // The matrix containing information for projecting the worldview matrix
        public Matrix4 Projection { get; set; }
        // A matrix containing info about how to move around the world around the camera... sort of
        // this is normally set as a player's worldview
        public Matrix4 WorldView { get; set; }

        public Camera()
        {
            ViewportHeight = 256;
            ViewportWidth = 512;
            Projection = Matrix4.Identity();
            WorldView = Matrix4.Identity();
        }

        public void SetSize(int w, int h, float n, float f, float fov)
        {
            ViewportWidth = w;
            ViewportHeight = h;
            Near = n;
            Far = f;
            FOV = fov;

            float fovRads = 1.0f / MathF.Tan(fov * MathF.PI / 360.0f);
            float aspect = ((float)h) / ((float)w);
            float distance = n - f;

            Projection.M[0]  = fovRads * aspect;
            Projection.M[1]  = 0.0f;
            Projection.M[2]  = 0.0f;
            Projection.M[3]  = 0.0f;

            Projection.M[4]  = 0.0f;
            Projection.M[5]  = fovRads;
            Projection.M[6]  = 0.0f;
            Projection.M[7]  = 0.0f;

            Projection.M[8]  = 0.0f;
            Projection.M[9]  = 0.0f;
            Projection.M[10] = (n + f) / distance;
            Projection.M[11] = (2 * n * f) / distance;

            Projection.M[12] = 0.0f;
            Projection.M[13] = 0.0f;
            Projection.M[14] = -1.0f;
            Projection.M[15] = 0.0f;
        }

        // Returns the camera's viewing matrix
        public Matrix4 Matrix()
        {
            return Projection * WorldView;
        }

        public void UseViewport()
        {
            GL.Viewport(0, 0, ViewportWidth, ViewportHeight);
        }
    }
}
