using OpenGLGameCS.Mathematics;
using OpenTK.Graphics.ES20;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OpenGLGameCS.Game.Rendering
{
    public class PhysicalObject : GameObject
    {
        // The velocity of the object
        public Vector3 Velocity { get; set; }

        public PhysicalObject()
        {
            Reset();
        }

        public override void Reset()
        {
            Velocity = Vector3.Zero;
            base.Reset();
        }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            // slowly slow donw object
            Velocity *= (1.0f - 0.0009f);

            // this is where the delta time comes in. if the game is lagging,
            // delta time increases and we move faster
            Position += Velocity * GameTime.Delta;
        }
    }
}
