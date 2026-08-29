using HordeSurvival.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Entities;

public class GrassTile : Entity
{
    public const int Size = 64;

    private static readonly Rectangle SourceRectangle = new(64, 64, Size, Size);

    public GrassTile(ContentManager content, Vector2 position)
    {
        var texture = content.Load<Texture2D>("Sprites/Decorations/Tilemap_color2");

        var transform = AddComponent(new TransformComponent());
        transform.Position = position;

        AddComponent(new SpriteComponent(transform, texture, SourceRectangle));
    }
}
