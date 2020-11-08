using OpenGLGameCS.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLGameCS.Game.Rendering
{
    public class GameObject
    {
        public Mesh Mesh { get; private set; }
        public Shader Shader { get; private set; }

        // The position in world space
        public Vector3 Position { get; set; }


        // Rotation
        public Vector3 Euler { get; set; }

        // Scale
        public Vector3 Scale { get; set; }

        public GameObject()
        {
            Reset();
        }

        public GameObject(string vertexCode, string fragmentCode, float[] meshVertices)
        {
            LoadObject(vertexCode, fragmentCode, meshVertices);
            Reset();
        }

        public void LoadObject(string vertexCode, string fragmentCode, float[] meshVertices)
        {
            Mesh = new Mesh(meshVertices);
            Shader = new Shader(vertexCode, fragmentCode);
        }

        public virtual void Reset()
        {
            Position = Vector3.Zero;
            Euler = Vector3.Zero;
            Scale = Vector3.Ones;
        }

        public virtual void Update(MouseState mouse, KeyboardState keyboard)
        {

        }

        public virtual void Draw(Camera camera)
        {
            // need camera
            Matrix4 mvp = camera.Matrix() * LocalToWorld();

            Shader.Use();
            Shader.SetMatrix(mvp);
            Mesh.Draw();
        }

        // Scales down and moves the object awy from the center of the world to
        // create a sort of "world" view of the object, but as a matrix... sort of
        public Matrix4 LocalToWorld()
        {
            return
                Matrix4.Translate(Position) *
                Matrix4.RotateY(Euler.Y) *
                Matrix4.RotateX(Euler.X) *
                Matrix4.RotateZ(Euler.Z) *
                Matrix4.Scale(Scale);
        }

        // Scales up and moves the object towards the "center of the world" or center of
        // space as a matrix
        public Matrix4 WorldToLocal()
        {
            return
                Matrix4.Scale(1.0f / Scale) *
                Matrix4.RotateZ(-Euler.Z) *
                Matrix4.RotateX(-Euler.X) *
                Matrix4.RotateY(-Euler.Y) *
                Matrix4.Translate(-Position);
        }
    }
}
