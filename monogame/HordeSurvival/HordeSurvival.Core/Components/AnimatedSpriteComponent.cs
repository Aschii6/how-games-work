using System;
using System.Collections.Generic;
using HordeSurvival.Core.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Components;

public class AnimatedSpriteComponent : ComponentBase
{
    public bool FlipH { get; set; } = false;
    public bool FlipV { get; set; } = false;

    private readonly TransformComponent _transform;

    private readonly Dictionary<string, Animation> _animations;

    private Animation _currentAnimation;

    public AnimatedSpriteComponent(TransformComponent transform, Dictionary<string, Animation> animations, string name)
    {
        if (!animations.ContainsKey(name))
            throw new ArgumentException($"Animation {name} does not exist");

        _transform = transform;
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
        var effects = SpriteEffects.None;
        if (FlipH) effects |= SpriteEffects.FlipHorizontally;
        if (FlipV) effects |= SpriteEffects.FlipVertically;

        var frame = _currentAnimation.CurrentFrame;
        var origin = new Vector2(frame.Width / 2f, frame.Height / 2f);

        spriteBatch.Draw(texture: _currentAnimation.Texture, position: _transform.Position,
            sourceRectangle: frame, color: Color.White, rotation: 0f, origin: origin,
            scale: Vector2.One, effects: effects, layerDepth: 0f);
    }
}
