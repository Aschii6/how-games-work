using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Animations;

public class Animation
{
    public Texture2D Texture { get; }

    public Rectangle CurrentFrame => _frames[_currentFrameIndex];

    // Possible to use another class instead of Rectangles, if I want to handle different textures per frame
    private readonly List<Rectangle> _frames = [];

    private readonly float _frameTime;
    private readonly bool _isLooping;
    private int _currentFrameIndex = 0;
    private float _timeSinceLastFrame = 0f;

    public Animation(Texture2D texture, int animationSpeed, bool isLooping = false)
    {
        if (animationSpeed < 1)
            throw new ArgumentException("Animation speed must be greater than or equal to 1.");

        Texture = texture;
        _isLooping = isLooping;
        _frameTime = 1 / (float)animationSpeed;
    }

    public void AddFrame(Rectangle frame)
    {
        _frames.Add(frame);
    }

    public void Reset()
    {
        _currentFrameIndex = 0;
        _timeSinceLastFrame = 0f;
    }

    public void Update(GameTime gameTime)
    {
        _timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

        _currentFrameIndex += (int)(_timeSinceLastFrame / _frameTime);
        _timeSinceLastFrame %= _frameTime;

        if (_isLooping)
            _currentFrameIndex %= _frames.Count;
        else
            _currentFrameIndex = Math.Min(_currentFrameIndex, _frames.Count - 1);
    }
}
