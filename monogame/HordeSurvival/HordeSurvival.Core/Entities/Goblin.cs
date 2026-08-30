using System;
using System.Collections.Generic;
using HordeSurvival.Core.Animations;
using HordeSurvival.Core.Components;
using HordeSurvival.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Entities;

public class Goblin : Entity
{
    private enum GoblinState
    {
        Idle,
        Run,
        Attack
    }

    private GoblinState _state = GoblinState.Idle;
    private float _attackCooldownRemaining;
    private float _flashTimeRemaining;

    private readonly Player _player;

    private readonly TransformComponent _transform;
    private readonly MovementComponent _movement;
    private readonly AnimatedSpriteComponent _animatedSprite;
    private readonly HitboxComponent _hitbox;
    private readonly HurtboxComponent _hurtbox;

    private static readonly Vector2 HitboxOffset = new(35, 0);

    public event Action<Goblin> Died;

    public Goblin(ContentManager content, Player player)
    {
        _transform = AddComponent(new TransformComponent());
        _movement = AddComponent(new MovementComponent(_transform, 200f, 1f / 4f));
        _animatedSprite = AddComponent(new AnimatedSpriteComponent(_transform, LoadAnimations(content), "idle"));
        _hurtbox = AddComponent(new HurtboxComponent(_transform, new CircleShape(30), Vector2.Zero, 45, 0.35f));
        _hitbox = AddComponent(new HitboxComponent(_transform, new CircleShape(55), HitboxOffset, 5));

        _animatedSprite.Finished += OnAnimationFinished;
        _hurtbox.Damaged += OnHurtboxDamaged;
        _hurtbox.Died += OnHurtboxDied;

        _player = player;
    }

    public HitboxComponent Hitbox => _hitbox;
    public HurtboxComponent Hurtbox => _hurtbox;

    public override void Update(GameTime gameTime)
    {
        var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        UpdateAttackCooldown(elapsedSeconds);
        HandleMovement();

        base.Update(gameTime);

        UpdateHitbox();
        UpdateFlash(elapsedSeconds);
    }

    private void UpdateAttackCooldown(float elapsedSeconds)
    {
        _attackCooldownRemaining = MathF.Max(0f, _attackCooldownRemaining - elapsedSeconds);
    }

    private void HandleMovement()
    {
        if (_state == GoblinState.Attack) return;

        var playerPos = _player.GetComponent<TransformComponent>().Position;
        var direction = Vector2.Normalize(playerPos - _transform.Position);
        var distance = (playerPos - _transform.Position).Length();

        if (distance < 64)
        {
            _movement.TargetVelocity = Vector2.Zero;
            SetState(_attackCooldownRemaining == 0 ? GoblinState.Attack : GoblinState.Idle);
        }
        else
        {
            _movement.TargetVelocity = direction * _movement.MaxSpeed;
            SetState(GoblinState.Run);
        }

        if (distance > 8)
            _animatedSprite.FlipH = direction.X < 0;
    }

    private void UpdateHitbox()
    {
        _hitbox.Enabled = _state == GoblinState.Attack && _animatedSprite.CurrentFrameIndex is 3 or 4 or 5;
        _hitbox.Offset = new Vector2(_animatedSprite.FlipH ? -HitboxOffset.X : HitboxOffset.X, HitboxOffset.Y);
    }

    private void OnHurtboxDamaged() => _flashTimeRemaining = 0.2f;

    private void UpdateFlash(float delta)
    {
        _flashTimeRemaining = MathF.Max(0, _flashTimeRemaining - delta);
        _animatedSprite.Tint = Color.Lerp(Color.White, Color.DeepPink, _flashTimeRemaining / 0.2f);
    }

    private void OnHurtboxDied() => Died?.Invoke(this);

    private void OnAnimationFinished()
    {
        if (_state == GoblinState.Attack)
        {
            SetState(GoblinState.Idle);
            _attackCooldownRemaining = 1;
        }
    }

    private void SetState(GoblinState newState)
    {
        if (_state == newState) return;

        _state = newState;

        _animatedSprite.Play(_state.ToString().ToLowerInvariant());
    }

    private static Dictionary<string, Animation> LoadAnimations(ContentManager content)
    {
        var spriteSheet = content.Load<Texture2D>("Sprites/Goblin/Torch_Red");

        var idleAnimation = new Animation(spriteSheet, 10, true);
        for (int i = 0; i < 7; i++)
        {
            idleAnimation.AddFrame(new Rectangle(192 * i, 0, 192, 192));
        }

        var runAnimation = new Animation(spriteSheet, 10, true);
        for (int i = 0; i < 6; i++)
        {
            runAnimation.AddFrame(new Rectangle(192 * i, 192 * 1, 192, 192));
        }

        var attackAnimation = new Animation(spriteSheet, 10, false);
        for (int i = 0; i < 6; i++)
        {
            attackAnimation.AddFrame(new Rectangle(192 * i, 192 * 2, 192, 192));
        }

        return new Dictionary<string, Animation>
        {
            { "idle", idleAnimation },
            { "run", runAnimation },
            { "attack", attackAnimation }
        };
    }
}
