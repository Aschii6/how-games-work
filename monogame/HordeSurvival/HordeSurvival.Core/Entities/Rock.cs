using HordeSurvival.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Entities;

public class Rock : Entity
{
    public Rock(ContentManager content, string spriteName, Vector2 position)
    {
        var texture = content.Load<Texture2D>($"Sprites/Decorations/{spriteName}");

        var transform = AddComponent(new TransformComponent());
        transform.Position = position;

        AddComponent(new SpriteComponent(transform, texture, new Rectangle(0, 0, texture.Width, texture.Height)));
    }
}
