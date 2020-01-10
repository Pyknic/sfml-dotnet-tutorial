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
            using(RenderWindow window=new RenderWindow(new VideoMode(800,600),"Hello SFML")){
                window.Closed+=(s,e)=>window.Close();
                RectangleShape shape=new RectangleShape(new Vector2f(100,100));
            
                shape.Position=new Vector2f(400,300);
                shape.Origin=new Vector2f(50,50);
                shape.Rotation=45.0f;
                shape.FillColor=Color.Red;

                while(window.IsOpen){
                    window.DispatchEvents();
                    window.Clear();
                    window.Draw(shape);
                    window.Display();
                }
            } 
        }
    }
}
