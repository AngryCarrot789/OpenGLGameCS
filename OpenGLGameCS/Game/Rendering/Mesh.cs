using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLGameCS.Game.Rendering
{
    public class Mesh : IDisposable
    {
        // why not have this
        public string MeshName { get; set; }

        // Stores the ID to the Vertex Array Object
        public int VAO { get; private set; }
        // Stores the ID to the Vertex Buffer Object
        // containing the vertices and stuff
        public int VBO { get; private set; }

        // Holds a list of the object's vertices
        public List<float> Vertices { get; private set; }

        public Mesh()
        {
            Vertices = new List<float>();
        }

        public Mesh(float[] vertices)
        {
            LoadVertices(vertices);
        }

        public void LoadVertices(float[] vertices)
        {
            Vertices = new List<float>(vertices);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            {
                // Generates the Buffer at the specified ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                // this passes the vertices to the vertex buffer.
                // it requires that you specify the size of it, not by the size of the array,
                // but the size of the array in bytes, not capacity.
                // static draw......
                GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    Vertices.Count * sizeof(float),
                    Vertices.ToArray(),
                    BufferUsageHint.StaticDraw);
                // Enable editing of the first buffer object... i think
                GL.EnableVertexAttribArray(0);
                // Tells OpenGL some info about the buffer, like how many numbers per vertex
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);
            }
        }

        public void Draw()
        {
            GL.BindVertexArray(VAO);
            GL.DrawArrays(BeginMode.Triangles, 0, Vertices.Count);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
        }
    }
}
