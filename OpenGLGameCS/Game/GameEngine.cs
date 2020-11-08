using OpenGLGameCS.Game.Rendering;
using OpenGLGameCS.Mathematics;
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

        //float[] vertices = new float[]
        //{
        //    -0.8f, -0.8f, 1.0f,
        //     0.0f,  0.8f, 1.0f,
        //     0.8f, -0.8f, 1.0f,
        //};

        float[] vertices =
        {
            // Front face - 2 triangles
            -1.0f,  1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,

            // Top Face
            -1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f, -1.0f,
            
            // Right Face
             1.0f,  1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            
            // Bottom Face
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
             1.0f, -1.0f,  1.0f,
             1.0f, -1.0f, -1.0f,
            
            // Back Face
             1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,
            
            // Left Face
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
        };

        public Camera cam;
        public Player player;
        public GameObject tri;

        // Called first after the Run() function is called, i think.
        // or after the constructor completes. basically, it only runs once.
        protected override void OnLoad()
        {
            base.OnLoad();
            string vertexShader =
                "#version 330\n" +
                "uniform mat4 mvp;\n" +
                "in vec3 in_pos;\n" +
                "void main() { gl_Position = mvp * vec4(in_pos, 1.0); }";

            string fragmentShader = 
                "#version 330\n" +
                "void main() { gl_FragColor = vec4(0.8, 0.2, 1.0, 1.0); }\n";

            tri = new GameObject(vertexShader, fragmentShader, vertices);
            tri.Position = new Vector3(0, 0, -5.0f);
            cam = new Camera();
            player = new Player();

            base.CursorGrabbed = true;

            Size = new OpenTK.Mathematics.Vector2i(1280, 720);
        }

        // Called after the engine stops, i think.
        protected override void OnUnload()
        {
            base.OnUnload();
        }

        float ticksElapsed;

        // Occours every time an update should be done.
        // This is called 2000~ times a second with my PC's speed
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            ticksElapsed++;

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Close();
            }

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

            cam.WorldView = player.WorldToCamera();
            cam.SetSize(Size.X, Size.Y, 0.01f, 100.0f, 65.0f);
            cam.UseViewport();


            // these objects could be placed in a list of GameObjects to
            // make it easier, ill do that later.
            player.Update(MouseState, KeyboardState);
            tri.Update(MouseState, KeyboardState);

            //player.Euler += new Vector3(0, 0.001f, 0);
            //tri.Euler.Z += ticksElapsed / 10000.0f;
            tri.Draw(cam);

            // OpenGL works with 2 buffers... i think
            // the screen buffer and back-end buffer. you draw to the
            // back-end buffer and then swap it to make it the screen buffer.
            // technically it isn't the system screen buffer, but the viewport
            // screen buffer... eh
            Context.SwapBuffers();
        }
    }
}
