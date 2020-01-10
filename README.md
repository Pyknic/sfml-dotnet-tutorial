# Asteroids CSharp
Here are the instructions for the Asteroids C#-lab.

### Step X
```bash
mkdir Asteroids
cd Asteroids
dotnet new console
```

### Step X
```bash
dotnet add package CSFML --version 2.5.0
dotnet add package SFML.Net --version 2.5.0
```

### Step X
```bash
dotnet build
```

### Step X
```bash
dotnet run
```

It should print out `Hello World!` in the console.

### Step X
Open the file `Program.cs` in a text editor (Notepad++ or Brackets). Change it to the following:

```csharp
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
            using (RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Hello SFML")) {
                window.Closed += (s,e) => window.Close();
                RectangleShape shape = new RectangleShape(new Vector2f(100, 100));
            
                shape.Position = new Vector2f(400,300);
                shape.Origin = new Vector2f(50,50);
                shape.Rotation = 45.0f;
                shape.FillColor = Color.Red;

                while (window.IsOpen) {
                    window.DispatchEvents();
                    window.Clear();
                    window.Draw(shape);
                    window.Display();
                }
            } 
        }
    }
}
```

Now, to run your application:
```bash
dotnet build
dotnet run
```