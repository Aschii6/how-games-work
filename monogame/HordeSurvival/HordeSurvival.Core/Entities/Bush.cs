using System.Collections.Generic;
using HordeSurvival.Core.Animations;
using HordeSurvival.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Entities;

public class Bush : Entity
{
    public Bush(ContentManager content, Vector2 position)
    {
        var transform = AddComponent(new TransformComponent());
        transform.Position = position;

        AddComponent(new AnimatedSpriteComponent(transform, LoadAnimations(content), "sway"));
    }

    private static Dictionary<string, Animation> LoadAnimations(ContentManager content)
    {
        var spriteSheet = content.Load<Texture2D>("Sprites/Decorations/Bush");

        var swayAnimation = new Animation(spriteSheet, 10, true);
        for (int i = 0; i < 8; i++)
        {
            swayAnimation.AddFrame(new Rectangle(128 * i, 0, 128, 128));
        }

        return new Dictionary<string, Animation> { { "sway", swayAnimation } };
    }
}
