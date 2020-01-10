# Game of Pong
In [the previous part](Part-03_Game-of-Pong.md) we made it possible for the player to move a rectangle up and down using keyboard inputs. In this part, we will turn that into a game of Pong by adding a second bat and a ball.

### Step 1: First Bat
To turn the rectangle we just created into a bat we simply need to change the size and position.

```csharp
var shape = new RectangleShape(new Vector2f(20, 100));
shape.Position = new Vector2f(20, 300);
```

### Step 2: Second Bat
To create a second bat, we simply need to duplicate the lines creating the bat and change the x-position.

```csharp
var shape = new RectangleShape(new Vector2f(20, 100));
shape.Position = new Vector2f(20, 300);
shape.FillColor = new Color(66, 121, 153);
shape.Origin = shape.Size * 0.5f;

var shape2 = new RectangleShape(new Vector2f(20, 100));
shape2.Position = new Vector2f(800 - 20, 300);
shape2.FillColor = new Color(66, 121, 153);
shape2.Origin = shape2.Size * 0.5f;
```

This doesn't look any different though since we also need to render it to the screen.

```csharp
window.Draw(shape);
window.Draw(shape2);
```

### Step 3: Multiplayer
We can make it into a multiplayer game by moving the second bat when the `up`-and `down`-keys are pressed.

```csharp
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

Note how all 4 if-statements check the same variable, `e.Code`. A different way to get the same result is to use a `switch`-statement instead:

```csharp
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
```

Remember to do the same for the `KeyReleased`-event. We also need to use those values in the update-loop.

```csharp
if (moveUp) shape.Position -= new Vector2f(0, 100.0f) * deltaTime;
if (moveDown) shape.Position += new Vector2f(0, 100.0f) * deltaTime;
if (move2Up) shape2.Position -= new Vector2f(0, 100.0f) * deltaTime;
if (move2Down) shape2.Position += new Vector2f(0, 100.0f) * deltaTime;
```

You should now be able to move the left bat using `W` and `S` and the second bat using the `up`- and `down`-keys.

### Step 4: Create a Ball
The game would be even more fun with something to hit. First, we create a ball-shape:

```csharp
var ball = new CircleShape(10.0f);
ball.Position = new Vector2f(400, 300);
ball.FillColor = new Color(189, 248, 255);
ball.Origin = new Vector2f(ball.Radius, ball.Radius);
var ballVelocity = new Vector2f(150.0f, 150.0f); // We will come back to this...
```

Note that the `ball.Origin` is slightly different since a circle doesn't have a `Size` property but a `Radius` instead.

We also need to render it.

```csharp
window.Draw(shape);
window.Draw(shape2);
window.Draw(ball);
```

### Step 5: Make it Move
To make the ball move, we need to update the position each frame.
```csharp
if (moveUp) shape.Position -= new Vector2f(0, 150.0f) * deltaTime;
if (moveDown) shape.Position += new Vector2f(0, 150.0f) * deltaTime;
if (move2Up) shape2.Position -= new Vector2f(0, 150.0f) * deltaTime;
if (move2Down) shape2.Position += new Vector2f(0, 150.0f) * deltaTime;

ball.Position += ballVelocity * deltaTime;

window.Draw(shape);
window.Draw(shape2);
window.Draw(ball);
```

This should make the ball move across the screen.

**Tip:** `ball.Position` has a dot between the words since `Position` is a public field inside the class `CircleShape` which `ball` is an instance of. `ballVelocity` does not have a dot since that is a single variable created by us in this file.

### Step 6: Collisions
It would be an even better game if the ball where to bounce if it hit one of the bats. First we need to detect that a collisions has happened. SFML has a neat function for that called `GetGlobalBounds().Intersects(...)`.

```csharp
ball.Position += ballVelocity * deltaTime;
if (ball.GetGlobalBounds().Intersects(shape.GetGlobalBounds())
||  ball.GetGlobalBounds().Intersects(shape2.GetGlobalBounds()))
{
    Console.WriteLine("Collision!");
}
```

**Tip:** The `||` symbols are read as `or`. If the ball collides with the left bat `or` it collides with the right bat, the program will enter the if-statement.

If you play the game and move the second bat down a little you can see the message being printed out in the terminal.

To make it bounce we simply need to flip the velocity in the x-direction when a collision happens.

```csharp
ball.Position += ballVelocity * deltaTime;
if (ball.GetGlobalBounds().Intersects(shape.GetGlobalBounds())
||  ball.GetGlobalBounds().Intersects(shape2.GetGlobalBounds()))
{
    Console.WriteLine("Collision!");
    ballVelocity.X *= -1.0f;
}
```

This bounce works, but the ball should also bounce against the top and bottom parts of the screen.

```csharp
if (ball.GetGlobalBounds().Top < 0.0f
||  ball.GetGlobalBounds().Top + ball.GetGlobalBounds().Height > 600.0f)
{
    ballVelocity.Y *= -1.0f;
}
```

We should also reset the game if the ball goes outside. This means movin the ball back to the center of the screen and resetting the velocity.

```csharp
if (ball.Position.X < -100.0f || ball.Position.X > 900.0f)
{
    ball.Position = new Vector2f(400.0f, 300.0f);
    ballVelocity = new Vector2f(150.0f, 150.0f);
}
```

### Step 7: Add Font
To make our game more competitive we should add a score board. To be able to render text onto the screen we first need to load a font-file. In this case we will use the [Roboto-Regular.ttf-font](https://fonts.google.com/specimen/Roboto) from Google Fonts.

Download the file mentioned and put it into the project folder. Next, open the file `asteroids-csharp.csproj` in a text editor and add the following just *before* the `</Project>` at the end:

```xml
<ItemGroup>
    <None Remove="Roboto-Regular.ttf" />
    <EmbeddedResource Include="Roboto-Regular.ttf">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </EmbeddedResource>
</ItemGroup>
```

This will copy the font file to the `bin/Debug/netcoreapp3.0/`-folder when we build the game.

Now we can load the font and use it to create a text. Add the following just after creating the two rectangles and the circle:

```csharp
var gui = new Text();
gui.Font = new Font("Roboto-Regular.ttf");
gui.CharacterSize = 30;
gui.DisplayedString = "Score: 0 / 0";
gui.Style = Text.Styles.Bold;
gui.Position = new Vector2f(400.0f, 30.0f);
gui.Origin = new Vector2f(gui.GetGlobalBounds().Width, gui.GetGlobalBounds().Height) * 0.5f;
```

To make the text appear we also need to render it, just like everything else.

```csharp
window.Draw(shape);
window.Draw(shape2);
window.Draw(ball);
window.Draw(gui);
```

Now the text should appear at the top of the screen.

**Tip:** If you get an error loading the font, it has probably been placed in the wrong folder. It should be beside the .csproj-file.

### Part 8: Keeping Score
To keep track of the score we need to store the number of victories for each player.

```csharp
var clock = new Clock();
var player1Score = 0;
var player2Score = 0;
while (window.IsOpen)
{
    ...
}
```

Next, we can increase the score and update the text of the gui whenever a ball goes outside. For this we should modify the reset-logic we created before.

```csharp
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
```

We now have a score board!