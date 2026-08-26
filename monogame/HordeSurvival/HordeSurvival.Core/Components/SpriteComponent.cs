using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Components;

public class SpriteComponent : ComponentBase
{
    public bool FlipH { get; set; } = false;
    public bool FlipV { get; set; } = false;

    private readonly Texture2D _texture;
    private readonly Rectangle _sourceRectangle;

    private readonly TransformComponent _transform;

    public SpriteComponent(TransformComponent transform, Texture2D texture, Rectangle sourceRectangle)
    {
        _transform = transform;
        _texture = texture;
        _sourceRectangle = sourceRectangle;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var effects = SpriteEffects.None;
        if (FlipH) effects |= SpriteEffects.FlipHorizontally;
        if (FlipV) effects |= SpriteEffects.FlipVertically;

        var origin = new Vector2(_sourceRectangle.Width / 2f, _sourceRectangle.Height / 2f);

        spriteBatch.Draw(texture: _texture, position: _transform.Position,
            sourceRectangle: _sourceRectangle, color: Color.White, rotation: 0f, origin: origin,
            scale: Vector2.One, effects: effects, layerDepth: 0f);
    }
}
