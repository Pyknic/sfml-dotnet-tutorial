# Part 4: The Bat Class
In [the previous part](Part-03_Game-of-Pong.md) we made a simple game of Pong. As you might have noticed, we had a lot of duplicated code. We also specified the same numbers, like the size of the screen, multiple times. This way of programming makes the game error-prone, so we should try to avoid it. In this part we will refactor ("reorganize") the code in the previous part to adress those issues.

### Step 1: Create a Class
A class is a template for something that holds information and logic related to something special. In our case, we will start by taking all the data and logic that relates to the bats and put them in a class.

First, create an empty file called `Bat.cs` and enter the following:

```c#
namespace asteroids_csharp
{
    public class Bat
    {
        
    }
}
```

This is the basic frame for a new class. In our `Program.cs`-file, we could now write:

```c#
var bat = new Bat();
```

...and it would give us a new instance of this class. However, the class doesn't do much yet. Let's add some variables!

```c#
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
    }
}
```

Every bat consists of a shape, two bools and a speed. We also need to store what keyboard keys should make this specific bat move. The shape is already a class that consists of a position, size, color etc. so we don't need to repeat that.

### Step 2: Constructor
Next we need to set those values the first time. That is what a constructor is for.

```c#
public class Bat
{
    private RectangleShape shape;
    private bool moveUp, moveDown;
    private float speed;

    public Bat(Vector2f position)
    {
        shape = new RectangleShape(new Vector2f(20, 100));
        shape.Position = position;
        shape.FillColor = new Color(66, 121, 153);
        shape.Origin = shape.Size * 0.5f; // Middle of rectangle
        moveUp = false;
        moveDown = false;
        speed = 150.0f;
    }
}
```

Since the two bats will be controlled using different keyboard keys, we also need to store which keys control it.

```c#
public class Bat
{
    ...

    private Keyboard.Key upKey, downKey; // These are added

    public Bat(Vector2f position, 
               Keyboard.Key up, // Given as arguments
               Keyboard.Key down) 
    {
        ...

        upKey = up;     // Stored as members here
        downKey = down;
    }
}
```

### Step 3: Keyboard Events
The bat should be responsible for moving itself around when the user presses the right keys. To fix that, it needs to listen to the correct keyboard events. To get access to those, we can add a function that we call `OnEnter()` that takes the window and subscribes to any events it might need.

```c#
public class Bat
{
    ...

    public void OnEnter(RenderWindow window)
    {
        // here we have access to the window-instance.
    }
}
```

In that function, we can add the `KeyPressed`- and `KeyReleased`-listeners just like before.

```c#
public void OnEnter(RenderWindow window)
{
    window.KeyPressed += (s, e) =>
    {
        if (e.Code == upKey) // We use the key entered in the constructor here
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
```

### Step 4: Update
The update logic that is called every frame can also be put in its own function. This time, let's call it `OnUpdate()`.

```c#
public void OnUpdate(float deltaTime)
{
    if (moveUp) shape.Position -= new Vector2f(0, speed) * deltaTime;
    if (moveDown) shape.Position += new Vector2f(0, speed) * deltaTime;
}
```

Now that we have it as its own function, we could also constrain the bat so that it can't move outside.

```c#
public void OnUpdate(float deltaTime)
{
    ...

    if (shape.Position.Y < shape.Size.Y * 0.5f)
        shape.Position = new Vector2f(shape.Position.X, shape.Size.Y * 0.5f);
    if (shape.Position.Y > 600.0f - shape.Size.Y * 0.5f)
        shape.Position = new Vector2f(shape.Position.X, 600.0f - shape.Size.Y * 0.5f);
}
```

### Step 5: Collision
We will need some way to determine if two objects collide, so let's create an additional function that takes a circle and returns a `bool` that is `true` if the bat and the circle collides, else `false`.
```c#
public bool Intersects(CircleShape circle)
{
    return shape.GetGlobalBounds().Intersects(circle.GetGlobalBounds());
}
```

### Step 6: Render
Finally, we need to add a render function. It will be quite short.

```c#
public void OnRender(RenderWindow window)
{
    window.Draw(shape);
}
```

### Step 7: Instantiation
Currently, our new class is never used for anything, so we need to fix that. Open up `Program.cs` again and remove all code related to both the bats.

```c#
var batLeft = new Bat(new Vector2f(20, 300), Keyboard.Key.W, Keyboard.Key.S);
var batRight = new Bat(new Vector2f(800 - 20, 300), Keyboard.Key.Up, Keyboard.Key.Down);
batLeft.OnEnter(window);
batRight.OnEnter(window);
```

Note that we have to call `OnEnter` ourselves since this is a function we have created.

Next, let's update the loop.

```c#
batLeft.OnUpdate(deltaTime);
batRight.OnUpdate(deltaTime);
```

We also need to use the new `Intersect`-function we created instead of the old shapes.

```c#
batLeft.OnUpdate(deltaTime);
batRight.OnUpdate(deltaTime);
ball.Position += ballVelocity * deltaTime;

if (batLeft.Intersects(ball) || batRight.Intersects(ball))
{
    ballVelocity.X *= -1.0f;
}
```

And lastly, the render logic also need to be called.
```c#
batLeft.OnRender(window);
batRight.OnRender(window);
```

The game should functionally be pretty much the same, but now we have a cleaner separation.