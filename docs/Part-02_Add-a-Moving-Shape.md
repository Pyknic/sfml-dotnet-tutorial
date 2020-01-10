# Add a Moving Shape

In the previous part we created a window with a calm blue background. In this part, we will add a moving rectangle to it.
```csharp
using var window = new RenderWindow(new VideoMode(800, 600), "Hello SFML");
window.Closed += (s,e) => window.Close();

// This is where we will create our shape

while (window.IsOpen) { 
    window.DispatchEvents();
    window.Clear(new Color(131, 197, 235));

    // This is where we render ("paint") it to the window

    window.Display();
}
```

### Step 1: Rectangle
To create a rectangular shape:
```csharp
var shape = new RectangleShape(new Vector2f(100, 100));
```

To render it:
```csharp
window.Draw(shape);
```

**Tip:** Try changing the values in `new Vector2f(100, 100)` to get a different size.

### Step 2: Position & Color
Directly after creating the shape, we should specify how it should appear.
```csharp
var shape = new RectangleShape(new Vector2f(100, 100));
shape.Position = new Vector2f(400, 300);
shape.FillColor = new Color(66, 121, 153);
```

**Tip:** Google `rgb(66, 121, 153)` and you will get a color picker where you can see what values to use to specify different colors.

### Step 3: Origin
Even though we specified the center of the screen as the position, it is only the top-left corner of the box that is located there. To Fix that, we should set the origin of the cube.
```csharp
shape.Origin = shape.Size * 0.5f;
```
Both `shape.Origin` and `shape.Size` are fields of type `Vector2f`. You can use `+`, `-`, `*` etc. with vectors just like with numbers. In this case we use multiplication to set the origin to halfway through the size of the shape.

### Step 4: Keyboard Input
To make the rectangle move, first we need to detect when the player press a key.
```csharp
window.KeyPressed += (s, e) =>
{
    // Code here will be run when the user press any key
};
```

We can test if it works by changing the color of the rect when a key is pressed.

```csharp
window.KeyPressed += (s, e) =>
{
    shape.FillColor = Color.Red;
};
```

To make it a bit more interesting, set the color depending on which key was pressed.

```csharp
window.KeyPressed += (s, e) =>
{
    if (e.Code == Keyboard.Key.W)
    {
        shape.FillColor = Color.Red;
    }
};
```

### Step 5: Moving the Rectangle
To move the rectangle around, we can modify the code so that it adds a value to the position when we press "S" and subtracts a value when we press "W".
```csharp
window.KeyPressed += (s, e) =>
{
    if (e.Code == Keyboard.Key.W)
    {
        shape.Position -= new Vector2f(0.0f, 10.0f);
    }
    if (e.Code == Keyboard.Key.S)
    {
        shape.Position += new Vector2f(0.0f, 10.0f);
    }
};
```

You might note some lag. The rectangle moves one step, then it stays there a short while before it starts moving smoothly. This is due to how keyboard events work. We can fix this by not changing the position in the event but inside the main Update-loop.

### Step 5: Smooth Movement
To fix the lag, we need to store the state of the input keys outside the event.
```csharp
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
```

We can then use those `bool`-values in the update loop.

```csharp
while (window.IsOpen) {
    window.DispatchEvents();
    window.Clear(new Color(131, 197, 235));
    
    if (moveUp) shape.Position -= new Vector2f(0, 10.0f);
    if (moveDown) shape.Position += new Vector2f(0, 10.0f);
    window.Draw(shape);
    
    window.Display();
}
```

You can try it yourself! 

### Step 6: Stop Moving
Currently, the rectangle moves indefinitely. We should end the movement as soon as the player release the key. We can do that by introducing a new keyboard event.

```csharp
bool moveUp = false, moveDown = false;
window.KeyPressed += (s, e) => { /* ... */ };
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
```

### Step 7: Control the Speed
Now the problem is that the rectangle moves extremely fast. That is because the update loop runs many times per second, much more often than the keyboard inputs are received. To compensate for that, we can keep track of how much time has passed since the previous frame and use that to reduce the speed.

```csharp
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
```

Notice how we multiply the vector with `deltaTime`. This is the number of seconds since the last frame, which is typically a value around `0.001`. Therefore we need to *increase* the speed to `100.0f` since this is the speed in pixels *per second*. 
