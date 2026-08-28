using System;
using System.Collections.Generic;
using HordeSurvival.Core.Animations;
using HordeSurvival.Core.Components;
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

    private readonly Player _player;

    private readonly TransformComponent _transform;
    private readonly MovementComponent _movement;
    private readonly AnimatedSpriteComponent _animatedSprite;

    public Goblin(ContentManager content, Player player)
    {
        _transform = AddComponent(new TransformComponent());
        _movement = AddComponent(new MovementComponent(_transform, 200f, 1f / 4f));
        _animatedSprite = AddComponent(new AnimatedSpriteComponent(_transform, LoadAnimations(content), "idle"));

        _animatedSprite.Finished += OnAnimationFinished;

        _player = player;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _attackCooldownRemaining = MathF.Max(0, _attackCooldownRemaining -= delta);
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
