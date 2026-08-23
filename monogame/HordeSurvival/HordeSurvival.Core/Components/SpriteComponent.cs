using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Components;

public class SpriteComponent : ComponentBase
{
    public Texture2D Texture;
    public Rectangle SourceRectangle;

    private TransformComponent _transform;
    private TransformComponent Transform => _transform ??= Owner.GetComponent<TransformComponent>();

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Transform.Position, SourceRectangle, Color.White);
    }
}
