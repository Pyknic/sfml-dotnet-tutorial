using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace asteroids_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var window = new RenderWindow(new VideoMode(800, 600), "Hello SFML");
            window.Closed += (s,e) => window.Close();
                
            var shape = new RectangleShape(new Vector2f(100, 100));
            shape.Position = new Vector2f(400, 300);
            shape.FillColor = new Color(66, 121, 153);
            shape.Origin = shape.Size * 0.5f;

            bool moveUp = false, moveDown = false;
            window.KeyPressed += (s, e) =>
            {
                if (e.Code == Keyboard.Key.W)
                {
                    moveUp = true;
                }
                if (e.Code == Keyboard.Key.S)
                {
                    moveDown = true;
                }
            };

            window.KeyReleased += (s, e) =>
            {
                if (e.Code == Keyboard.Key.W)
                {
                    moveUp = false;
                }
                if (e.Code == Keyboard.Key.S)
                {
                    moveDown = false;
                }
            };

            var clock = new Clock();
            while (window.IsOpen)
            {
                var deltaTime = clock.Restart().AsSeconds();
                
                window.DispatchEvents();
                window.Clear(new Color(131, 197, 235));
                
                if (moveUp) shape.Position -= new Vector2f(0, 100.0f) * deltaTime;
                if (moveDown) shape.Position += new Vector2f(0, 100.0f) * deltaTime;
                window.Draw(shape);
                
                window.Display();
            }
        }
    }
}
