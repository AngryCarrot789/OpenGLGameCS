using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace OpenGLGameCS.Game
{
    public class GameEngine : GameWindow
    {
        public bool IsRunning { get; private set; }
        public GameEngine(
            GameWindowSettings gameWindowSettings, 
            NativeWindowSettings nativeWindowSettings) : 
            base(gameWindowSettings, nativeWindowSettings)
        {

        }

        public bool Initialise()
        {
            IsRunning = false;
            if (InitialiseOpenGL())
            {
                Console.WriteLine("Successfully initialised Game Engine");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to initialise Game Engine");
                return false;
            }
        }

        public bool InitialiseOpenGL()
        {
            GLFWBindingsContext binding = new GLFWBindingsContext();
            GL.LoadBindings(binding);

            if (GLFW.Init())
            {
                Console.WriteLine("Successfully initialised GLFW and OpenGL");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to initialise GLFW and OpenGL");
                return false;
            }
        }

        public void RunGameLoop()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Console.WriteLine("Starting Game Loop");
                base.Run();
            }
        }

        float[] vertices = new float[]
        {
            -0.8f, -0.8f, 1.0f,
             0.0f,  0.8f, 1.0f,
             0.8f, -0.8f, 1.0f,
        };

        int vao;
        int vbo;
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
        int vertexID;
        int fragmentID;
        int progID;

        // Called first after the Run() function is called, i think.
        // or after the constructor completes. basically, it only runs once.
        protected override void OnLoad()
        {
            base.OnLoad();

            // This generates a vertex array "location" in video memory and returns an ID that can
            // be used for getting the location of the array again.
            vao = GL.GenVertexArray();
            // this binds that array to the "active thing" in the GPU 
            GL.BindVertexArray(vao);

            // this generates an actual vertex array buffer to store vertices in it.
            vbo = GL.GenBuffer();

            // this binds the buffer to the "active thing" as well
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            // this passes the vertices to the vertex buffer.
            // it requires that you specify the size of it, not by the size of the array,
            // but the size of the array in bytes, not capacity.
            // static draw......
            GL.BufferData(
                BufferTarget.ArrayBuffer, 
                vertices.Length * sizeof(float), 
                vertices.ToArray(), 
                BufferUsageHint.StaticDraw);
            // this i think specifies that there's 3 vertices per "point"
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);

            // in OpenGL 3.3 which is what im using for OpenTK, you cannot use
            // GL.Begin() anymore because it was removed. I think the same for
            // GL.Vertex3, 2, etc.
            // So now you need to generate the arrays, buffers, etc. it's harder, but who would evem
            // use GL.Vertex2 anyway lol



            // Create the main shader program.
            progID = GL.CreateProgram();

            // ill just put the shader code in here for now. next video i'll load from a file.
            // this will be a simple pink shader
            // vertex shader doesn't do anything in terms of colours.
            // gl_Position is a built in variable is the location of the
            // vertex the shader is being applied to. altering gl_Position moves that vertex around.
            // but atm, we dont need to do that so it will just be set as the input position which is what
            // we need to tell the shader. sort of
            string vertexShader =
                "#version 330\n" +
                "in vec3 in_pos;\n" +
                "void main() { gl_Position = vec4(in_pos, 1.0); }";
            // compile and link the shader and it's stuff
            vertexID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexID, vertexShader);
            GL.CompileShader(vertexID);
            GL.GetShader(vertexID, ShaderParameter.CompileStatus, out int isCOmpiled);
            if (isCOmpiled < 1)
            {
                GL.GetShaderInfoLog(vertexID, out string info);
                Console.WriteLine($"Failed to compile vertex shader: {info}");
            }

            // now to make the fragment shader
            // gl_FragColour doesnt need to be linked... idk why atm
            // but the RGB value 0.8, 0.2, 1.0 is pink
            string fragmentShader = 
                "#version 330\n" +
                "void main() { gl_FragColor = vec4(0.8, 0.2, 1.0, 1.0); }\n";
            fragmentID = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentID, fragmentShader);
            GL.CompileShader(fragmentID);
            GL.GetShader(fragmentID, ShaderParameter.CompileStatus, out int isCOmpiled2);
            if (isCOmpiled2 < 1)
            {
                GL.GetShaderInfoLog(fragmentID, out string info);
                Console.WriteLine($"Failed to compile fragment shader: {info}");
            }

            // now attach them to the main program.

            GL.AttachShader(progID, vertexID);
            GL.AttachShader(progID, fragmentID);

            // now link the in_pos to the main program.
            // think this has to be below after linking
            GL.BindAttribLocation(progID, 0, "in_pos");

            GL.LinkProgram(progID);

            // check if it linked

            GL.GetProgram(progID, GetProgramParameterName.LinkStatus, out int linked);

            if (linked < 1)
            {
                GL.GetProgramInfoLog(progID, out string info);
                Console.WriteLine($"Not linked: {info}");
            }

            GL.DetachShader(progID, vertexID);
            GL.DetachShader(progID, fragmentID);

            GL.DeleteShader(vertexID);
            GL.DeleteShader(fragmentID);
        }

        // Called after the engine stops, i think.
        protected override void OnUnload()
        {
            base.OnUnload();

            // delete shaders after closing

            GL.DeleteProgram(progID);
        }

        // Occours every time an update should be done.
        // This is called 2000~ times a second with my PC's speed
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // This might be quite slow... but oh well
            GameTime.Delta = float.Parse(args.Time.ToString());
        }

        // Occours every time a frame should be rendered. this also runs as fast
        // as my PC can handle it, so 1000s of times a second.
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Clear the background and give it a colour
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.2f, 0.2f, 0.8f, 1.0f);

            // Setup the viewport so it stays the right size when resizing the window

            GL.Viewport(0, 0, Size.X, Size.Y);

            // Then give the triangle some colour. YOu also need to use shaders for this,
            // because i think they removed it in 3.2.

            // Making shaders is a bit iffy, but C# makes it extremely easy (imo lol)

            // then use the shader

            GL.UseProgram(progID);

            // Draw the array

            GL.BindVertexArray(vao);
            // this time you only need the capacity.
            GL.DrawArrays(BeginMode.Triangles, 0, vertices.Length);

            // OpenGL works with 2 buffers... i think
            // the screen buffer and back-end buffer. you draw to the
            // back-end buffer and then swap it to make it the screen buffer.
            // technically it isn't the system screen buffer, but the viewport
            // screen buffer... eh
            Context.SwapBuffers();
        }
    }
}
