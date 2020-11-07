using OpenGLGameCS.Game;
using OpenTK.Windowing.Desktop;
using System;

namespace OpenGLGameCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using(GameEngine game = new GameEngine(GameWindowSettings.Default, NativeWindowSettings.Default))
            {
                game.Initialise();
                game.RunGameLoop();
            }

            Console.WriteLine("Game has come to an end");
        }
    }
}
