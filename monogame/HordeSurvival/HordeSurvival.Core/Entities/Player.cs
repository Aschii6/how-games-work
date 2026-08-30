using System;
using System.Collections.Generic;
using HordeSurvival.Core.Animations;
using HordeSurvival.Core.Components;
using HordeSurvival.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HordeSurvival.Core.Entities;

public class Player : Entity
{
    private enum PlayerState
    {
        Idle,
        Run,
        Attack1,
        Attack2,
        Guard
    }

    private readonly MovementComponent _movement;
    private readonly AnimatedSpriteComponent _animatedSprite;
    private readonly HitboxComponent _hitbox;
    private readonly HurtboxComponent _hurtbox;

    private PlayerState _state = PlayerState.Idle;

    private bool _attack2Queued;
    private float _attack1ActiveTime;
    private float _attack1CooldownRemaining;
    private float _flashTimeRemaining;

    private static readonly Vector2 HitboxOffset = new(20, 10);

    public Player(ContentManager content)
    {
        var transform = AddComponent(new TransformComponent());
        _movement = AddComponent(new MovementComponent(transform, 300f, 1f / 3f));
        _animatedSprite = AddComponent(new AnimatedSpriteComponent(transform, LoadAnimations(content), "idle"));
        _hurtbox = AddComponent(new HurtboxComponent(transform, new CircleShape(25), Vector2.Zero, 100, 0.5f));
        _hitbox = AddComponent(new HitboxComponent(transform, new CircleShape(70), HitboxOffset, 20));

        _animatedSprite.Finished += OnAnimationFinished;
        _hurtbox.Damaged += OnHurtboxDamaged;
    }

    public HitboxComponent Hitbox => _hitbox;
    public HurtboxComponent Hurtbox => _hurtbox;

    public override void Update(GameTime gameTime)
    {
        var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        UpdateAttackTimers(elapsedSeconds);
        HandleAttackInput(Mouse.GetState());
        HandleMovement(Keyboard.GetState());

        base.Update(gameTime);

        UpdateHitbox();
        UpdateFlash(elapsedSeconds);
    }

    private void UpdateHitbox()
    {
        _hitbox.Enabled = _state is PlayerState.Attack1 or PlayerState.Attack2 &&
                          _animatedSprite.CurrentFrameIndex is 2 or 3;
        _hitbox.Offset = new Vector2(_animatedSprite.FlipH ? -HitboxOffset.X : HitboxOffset.X, HitboxOffset.Y);
    }

    private void OnHurtboxDamaged() => _flashTimeRemaining = 0.2f;

    private void UpdateFlash(float elapsedSeconds)
    {
        _flashTimeRemaining = MathF.Max(0, _flashTimeRemaining - elapsedSeconds);
        _animatedSprite.Tint = Color.Lerp(Color.White, Color.DeepPink, _flashTimeRemaining / 0.2f);
    }

    private void UpdateAttackTimers(float elapsedSeconds)
    {
        _attack1CooldownRemaining = Math.Max(0f, _attack1CooldownRemaining - elapsedSeconds);

        if (_state == PlayerState.Attack1)
            _attack1ActiveTime += elapsedSeconds;
    }

    private void HandleAttackInput(MouseState mouseState)
    {
        if (mouseState.LeftButton != ButtonState.Pressed) return;

        if (_state == PlayerState.Attack1)
        {
            if (_attack1ActiveTime >= 0.15f)
                _attack2Queued = true;
        }
        else if (_state is PlayerState.Idle or PlayerState.Run && _attack1CooldownRemaining <= 0f)
        {
            SetState(PlayerState.Attack1);
        }
    }

    private void HandleMovement(KeyboardState keyboardState)
    {
        var direction = Vector2.Zero;
        if (keyboardState.IsKeyDown(Keys.W)) direction.Y -= 1;
        if (keyboardState.IsKeyDown(Keys.S)) direction.Y += 1;
        if (keyboardState.IsKeyDown(Keys.A)) direction.X -= 1;
        if (keyboardState.IsKeyDown(Keys.D)) direction.X += 1;
        if (direction != Vector2.Zero)
            direction.Normalize();

        _movement.TargetVelocity = direction * _movement.MaxSpeed;

        if (_state is PlayerState.Idle or PlayerState.Run)
            SetState(_movement.Velocity == Vector2.Zero ? PlayerState.Idle : PlayerState.Run);

        if (_movement.Velocity.X < 0)
            _animatedSprite.FlipH = true;
        else if (_movement.Velocity.X > 0)
            _animatedSprite.FlipH = false;
    }

    private void OnAnimationFinished()
    {
        if (_state != PlayerState.Attack1)
        {
            SetState(PlayerState.Idle);
            return;
        }

        _attack1CooldownRemaining = 0.8f;

        if (_attack2Queued)
        {
            _attack2Queued = false;
            SetState(PlayerState.Attack2);
        }
        else
        {
            SetState(PlayerState.Idle);
        }
    }

    private void SetState(PlayerState newState)
    {
        if (_state == newState) return;

        _state = newState;

        if (_state == PlayerState.Attack1)
        {
            _attack1ActiveTime = 0f;
            _attack2Queued = false;
        }

        _animatedSprite.Play(_state.ToString().ToLowerInvariant());
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

        var attack1Texture = content.Load<Texture2D>("Sprites/Warrior/Warrior_Attack1");
        var attack1Animation = new Animation(attack1Texture, 10, false);
        for (int i = 0; i < 4; i++)
        {
            attack1Animation.AddFrame(new Rectangle(192 * i, 0, 192, 192));
        }

        var attack2Texture = content.Load<Texture2D>("Sprites/Warrior/Warrior_Attack2");
        var attack2Animation = new Animation(attack2Texture, 10, false);
        for (int i = 0; i < 4; i++)
        {
            attack2Animation.AddFrame(new Rectangle(192 * i, 0, 192, 192));
        }

        var guardTexture = content.Load<Texture2D>("Sprites/Warrior/Warrior_Guard");
        var guardAnimation = new Animation(guardTexture, 10, true);
        for (int i = 0; i < 6; i++)
        {
            guardAnimation.AddFrame(new Rectangle(192 * i, 0, 192, 192));
        }

        return new Dictionary<string, Animation>
        {
            { "idle", idleAnimation },
            { "run", runAnimation },
            { "attack1", attack1Animation },
            { "attack2", attack2Animation },
            { "guard", guardAnimation }
        };
    }
}
