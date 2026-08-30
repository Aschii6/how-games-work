using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Animations;

public class Animation
{
    public Texture2D Texture { get; }

    public Rectangle CurrentFrame => _frames[_currentFrameIndex];
    public int CurrentFrameIndex => _currentFrameIndex;

    public event Action Finished;

    // Possible to use another class instead of Rectangles, if I want to handle different textures per frame
    private readonly List<Rectangle> _frames = [];

    private readonly float _secondsPerFrame;
    private readonly bool _isLooping;
    private int _currentFrameIndex = 0;
    private float _timeSinceLastFrame = 0f;
    private bool _hasFinished = false;

    public Animation(Texture2D texture, int framesPerSecond, bool isLooping = false)
    {
        if (framesPerSecond < 1)
            throw new ArgumentException("Animation speed must be greater than or equal to 1.");

        Texture = texture;
        _isLooping = isLooping;
        _secondsPerFrame = 1 / (float)framesPerSecond;
    }

    public void AddFrame(Rectangle frame)
    {
        _frames.Add(frame);
    }

    public void Reset()
    {
        _currentFrameIndex = 0;
        _timeSinceLastFrame = 0f;
        _hasFinished = false;
    }

    public void Update(GameTime gameTime)
    {
        if (_hasFinished) return;

        _timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

        _currentFrameIndex += (int)(_timeSinceLastFrame / _secondsPerFrame);
        _timeSinceLastFrame %= _secondsPerFrame;

        if (_isLooping)
        {
            _currentFrameIndex %= _frames.Count;
        }
        else if (_currentFrameIndex >= _frames.Count - 1)
        {
            _currentFrameIndex = _frames.Count - 1;
            _hasFinished = true;
            Finished?.Invoke();
        }
    }
}
