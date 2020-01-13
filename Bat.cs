using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace asteroids_csharp
{
    public class Bat
    {
        private RectangleShape shape;
        private Keyboard.Key upKey, downKey;
        private bool moveUp, moveDown;
        private float speed;

        public Bat(Vector2f position, Keyboard.Key up, Keyboard.Key down)
        {
            shape = new RectangleShape(new Vector2f(20, 100));
            shape.Position = position;
            shape.FillColor = new Color(66, 121, 153);
            shape.Origin = shape.Size * 0.5f;
            moveUp = false;
            moveDown = false;
            speed = 150.0f;
            upKey = up;
            downKey = down;
        }

        public void OnEnter(RenderWindow window)
        {
            window.KeyPressed += (s, e) =>
            {
                if (e.Code == upKey)
                    moveUp = true;
                if (e.Code == downKey)
                    moveDown = true;
            };
            
            window.KeyReleased += (s, e) =>
            {
                if (e.Code == upKey)
                    moveUp = false;
                if (e.Code == downKey)
                    moveDown = false;
            };
        }

        public bool Intersects(CircleShape circle)
        {
            return shape.GetGlobalBounds().Intersects(circle.GetGlobalBounds());
        }

        public void OnUpdate(float deltaTime)
        {
            if (moveUp) shape.Position -= new Vector2f(0, speed) * deltaTime;
            if (moveDown) shape.Position += new Vector2f(0, speed) * deltaTime;
            if (shape.Position.Y < shape.Size.Y * 0.5f)
                shape.Position = new Vector2f(shape.Position.X, shape.Size.Y * 0.5f);
            if (shape.Position.Y > 600.0f - shape.Size.Y * 0.5f)
                shape.Position = new Vector2f(shape.Position.X, 600.0f - shape.Size.Y * 0.5f);
        }

        public void OnRender(RenderWindow window)
        {
            window.Draw(shape);
        }
    }
}