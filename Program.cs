using System;
using System.IO;
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
            
            var shape = new RectangleShape(new Vector2f(20, 100));
            shape.Position = new Vector2f(20, 300);
            shape.FillColor = new Color(66, 121, 153);
            shape.Origin = shape.Size * 0.5f;
            
            var shape2 = new RectangleShape(new Vector2f(20, 100));
            shape2.Position = new Vector2f(800 - 20, 300);
            shape2.FillColor = new Color(66, 121, 153);
            shape2.Origin = shape2.Size * 0.5f;
            
            var ball = new CircleShape(10.0f);
            ball.Position = new Vector2f(400, 300);
            ball.FillColor = new Color(189, 248, 255);
            ball.Origin = new Vector2f(ball.Radius, ball.Radius);
            var ballVelocity = new Vector2f(200.0f, 150.0f);

            var gui = new Text();
            gui.Font = new Font("Roboto-Regular.ttf");
            gui.CharacterSize = 30;
            gui.DisplayedString = "Score: 0 / 0";
            //gui.FillColor = Color.White;
            gui.Style = Text.Styles.Bold;
            gui.Position = new Vector2f(400.0f, 30.0f);
            gui.Origin = new Vector2f(gui.GetGlobalBounds().Width, gui.GetGlobalBounds().Height) * 0.5f;

            bool moveUp = false, moveDown = false, move2Up = false, move2Down = false;
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
                if (e.Code == Keyboard.Key.Up)
                {
                    move2Up = true;
                }
                if (e.Code == Keyboard.Key.Down)
                {
                    move2Down = true;
                }
            };
            
            window.KeyReleased += (s, e) =>
            {
                switch (e.Code)
                {
                    case Keyboard.Key.W:
                        moveUp = false;
                        break;
                    case Keyboard.Key.S:
                        moveDown = false;
                        break;
                    case Keyboard.Key.Up:
                        move2Up = false;
                        break;
                    case Keyboard.Key.Down:
                        move2Down = false;
                        break;
                }
            };
            
            var clock = new Clock();
            var player1Score = 0;
            var player2Score = 0;
            while (window.IsOpen)
            {
                var deltaTime = clock.Restart().AsSeconds();
                
                window.DispatchEvents();
                window.Clear(new Color(131, 197, 235));
                
                if (moveUp) shape.Position -= new Vector2f(0, 150.0f) * deltaTime;
                if (moveDown) shape.Position += new Vector2f(0, 150.0f) * deltaTime;
                if (move2Up) shape2.Position -= new Vector2f(0, 150.0f) * deltaTime;
                if (move2Down) shape2.Position += new Vector2f(0, 150.0f) * deltaTime;

                ball.Position += ballVelocity * deltaTime;
                if (ball.GetGlobalBounds().Intersects(shape.GetGlobalBounds()) 
                ||  ball.GetGlobalBounds().Intersects(shape2.GetGlobalBounds()))
                {
                    ballVelocity.X *= -1.0f;
                }

                if (ball.GetGlobalBounds().Top < 0.0f
                ||  ball.GetGlobalBounds().Top + ball.GetGlobalBounds().Height > 600.0f)
                {
                    ballVelocity.Y *= -1.0f;
                }

                if (ball.Position.X < -100.0f || ball.Position.X > 900.0f)
                {
                    if (ball.Position.X > 400.0f)
                        player1Score++;
                    else
                        player2Score++;
                    gui.DisplayedString = $"Score: {player1Score} / {player2Score}";
                    
                    ball.Position = new Vector2f(400.0f, 300.0f);
                    ballVelocity = new Vector2f(150.0f, 150.0f);
                }

                window.Draw(shape);
                window.Draw(shape2);
                window.Draw(ball);
                window.Draw(gui);
                
                window.Display();
            }
        }
    }
}
