# Asteroids CSharp
Here are the instructions for the Asteroids C#-lab.

### Step 1
```bash
mkdir Asteroids
cd Asteroids
dotnet new console
```

### Step 2
```bash
dotnet add package CSFML --version 2.5.0
dotnet add package SFML.Net --version 2.5.0
```

### Step 3
```bash
dotnet build
```

### Step 4
```bash
dotnet run
```

It should print out `Hello World!` in the console.

### Step 5
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
            using var window = new RenderWindow(new VideoMode(800, 600), "Hello SFML");
            window.Closed += (s,e) => window.Close();

            while (window.IsOpen) {
                window.DispatchEvents();
                window.Clear(new Color(131, 197, 235));
                window.Display();
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
