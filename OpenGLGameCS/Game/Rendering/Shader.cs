using OpenGLGameCS.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OpenGLGameCS.Game.Rendering
{
    // Shaders are made of 2 "sub shaders": a vertex shader
    // and a fragment shader.
    // vertex shaders are applied to every vertex every time you use the shader
    // (aka draw it... afaik).

    // the fragment shader is also applied to every "fragment" between the vertices.
    // Vertex shader defines the shape, and the fragment shader fills it in.

    // The vertex shader is what actually defines the real location of the vertices.
    // so essentially, the vertex shader can move around the vertices, which might sound
    // impossible. however, essentially, shaders are what move the objects around on screen.
    // more on that later tho.

    // vertex shader doesn't do anything in terms of colours.
    // gl_Position is a built in variable is the location of the
    // vertex the shader is being applied to. altering gl_Position moves that vertex around.
    // but atm, we dont need to do that so it will just be set as the input position which is what
    // we need to tell the shader. sort of
    public class Shader : IDisposable
    {
        public string ShaderName { get; set; }

        // Stores the ID of the main program
        public int ProgramID { get; private set; }
        // Stores the ID of the vertex shader
        public int VertexID { get; private set; }
        // Stores the ID of the fragment shader
        public int FragmentID { get; private set; }

        // Stores the ID of the matrix variable... sort of
        public int MVPID { get; private set; }

        public Shader(string vertexCode, string fragmentCode)
        {
            VertexID = LoadShader(vertexCode, ShaderType.VertexShader);
            FragmentID = LoadShader(fragmentCode, ShaderType.FragmentShader);

            ProgramID = GL.CreateProgram();
            GL.AttachShader(ProgramID, VertexID);
            GL.AttachShader(ProgramID, FragmentID);

            // Binds the matrix to the vertex shader sort of
            GL.BindAttribLocation(ProgramID, 0, "mvp");

            GL.LinkProgram(ProgramID);

            // Check if linked
            GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out int linked);
            if (linked < 1)
            {
                GL.GetProgramInfoLog(ProgramID, out string info);
                Console.WriteLine($"Failed to link shader program :\n{info}");
            }

            MVPID = GL.GetUniformLocation(ProgramID, "mvp");

            GL.DetachShader(ProgramID, VertexID);
            GL.DetachShader(ProgramID, FragmentID);
            GL.DeleteShader(VertexID);
            GL.DeleteShader(FragmentID);
        }

        // Loads the shaders into the main program
        public int LoadShader(string code, ShaderType type)
        {
            int shaderID = GL.CreateShader(type);
            GL.ShaderSource(shaderID, code);
            GL.CompileShader(shaderID);

            // Check if it compiled
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int isCompiled);
            if (isCompiled < 1)
            {
                string shaderType = type == ShaderType.VertexShader ? "Vertex" : (type == ShaderType.FragmentShader ? "Fragment" : "<Unknown>");
                GL.GetShaderInfoLog(shaderID, out string info);
                Console.WriteLine($"Failed to compile {shaderType} shader:\n{info}");
            }

            return shaderID;
        }

        public void Use()
        {
            GL.UseProgram(ProgramID);
        }

        public void SetMatrix(Matrix4 mvp)
        {
            // Checks if the MVPID is avaliable and 
            // checks if the matrix isn't null... duh lol
            if (MVPID != -1 && mvp != null)
            {
                // sets the matrix in the shader to the mvp.M matrix
                GL.UniformMatrix4(MVPID, 1, true, mvp.M);
            }
        }

        public void Dispose()
        {
            GL.DeleteProgram(ProgramID);
        }
    }
}
