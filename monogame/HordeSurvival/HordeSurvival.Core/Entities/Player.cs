using System.Collections.Generic;
using HordeSurvival.Core.Animations;
using HordeSurvival.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HordeSurvival.Core.Entities;

public class Player : Entity
{
    private readonly MovementComponent _movement;
    private readonly AnimatedSpriteComponent _animatedSprite;

    private string _state = "idle";

    public Player(ContentManager content)
    {
        var transform = AddComponent(new TransformComponent());
        _movement = AddComponent(new MovementComponent(transform, 300f, 1f / 3f));
        _animatedSprite = AddComponent(new AnimatedSpriteComponent(transform, LoadAnimations(content), "idle"));
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

        // Maybe check Velocity IsZeroApprox
        SetState(_movement.Velocity == Vector2.Zero ? "idle" : "run");

        base.Update(gameTime);
    }

    private void SetState(string newState)
    {
        if (_state == newState) return;

        _state = newState;

        switch (_state)
        {
            case "idle":
                _animatedSprite.Play("idle");
                break;
            case "run":
                _animatedSprite.Play("run");
                break;
        }
    }

    private static Dictionary<string, Animation> LoadAnimations(ContentManager content)
    {
        var idleTexture = content.Load<Texture2D>("Sprites/Warrior/Warrior_Idle");
        var idleAnimation = new Animation(idleTexture, 10, true);
        for (int i = 0; i < 8; i++)
        {
            idleAnimation.AddFrame(new Rectangle(192 * i, 0, 192, 192));
        }

        var runTexture = content.Load<Texture2D>("Sprites/Warrior/Warrior_Run");
        var runAnimation = new Animation(runTexture, 10, true);
        for (int i = 0; i < 6; i++)
        {
            runAnimation.AddFrame(new Rectangle(192 * i, 0, 192, 192));
        }

        return new Dictionary<string, Animation>
        {
            { "idle", idleAnimation },
            { "run", runAnimation }
        };
    }
}
