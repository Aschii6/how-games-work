using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Components;

public class SpriteComponent : ComponentBase
{
    private Texture2D _texture;
    private Rectangle _sourceRectangle;

    private readonly TransformComponent _transform;

    public SpriteComponent(TransformComponent transform, Texture2D texture, Rectangle sourceRectangle)
    {
        _transform = transform;
        _texture = texture;
        _sourceRectangle = sourceRectangle;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _transform.Position, _sourceRectangle, Color.White);
    }
}
