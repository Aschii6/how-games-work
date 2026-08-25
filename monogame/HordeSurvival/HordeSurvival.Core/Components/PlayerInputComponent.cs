using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HordeSurvival.Core.Components;

public class PlayerInputComponent : ComponentBase
{
    private readonly MovementComponent _movement;

    public PlayerInputComponent(MovementComponent movement)
    {
        _movement = movement;
    }

    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var direction = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.W)) direction.Y -= 1;
        if (keyboardState.IsKeyDown(Keys.S)) direction.Y += 1;
        if (keyboardState.IsKeyDown(Keys.A)) direction.X -= 1;
        if (keyboardState.IsKeyDown(Keys.D)) direction.X += 1;

        if (direction != Vector2.Zero)
            direction.Normalize();

        _movement.TargetVelocity = direction * _movement.MaxSpeed;
    }
}
