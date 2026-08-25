using System;
using System.Collections.Generic;
using HordeSurvival.Core.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Components;

public class AnimatedSpriteComponent : ComponentBase
{
    private TransformComponent _transform;
    private TransformComponent Transform => _transform ??= Owner.GetComponent<TransformComponent>();

    private readonly Dictionary<string, Animation> _animations;

    private Animation _currentAnimation;

    public AnimatedSpriteComponent(Dictionary<string, Animation> animations, string name)
    {
        if (!animations.ContainsKey(name))
            throw new ArgumentException($"Animation {name} does not exist");

        _animations = animations;
        _currentAnimation = animations[name];
    }

    public void Play(string name)
    {
        if (!_animations.TryGetValue(name, out var animation))
            throw new ArgumentException($"Animation '{name}' does not exist");
        if (ReferenceEquals(animation, _currentAnimation)) return;

        _currentAnimation = animation;
        _currentAnimation.Reset();
    }

    public override void Update(GameTime gameTime)
    {
        _currentAnimation.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_currentAnimation.Texture, Transform.Position, _currentAnimation.CurrentFrame, Color.White);
    }
}
