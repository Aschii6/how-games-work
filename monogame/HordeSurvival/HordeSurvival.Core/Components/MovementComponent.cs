using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Components;

public class MovementComponent : ComponentBase
{
    public Vector2 Velocity;
    public float Speed = 300f;

    private TransformComponent _transform;
    private TransformComponent Transform => _transform ??= Owner.GetComponent<TransformComponent>();

    public override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Transform.Position += Velocity * dt;
    }
}
