using HordeSurvival.Core.Extensions;
using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Components;

public class MovementComponent : ComponentBase
{
    public Vector2 Velocity;
    public Vector2 TargetVelocity;
    public float Speed = 300f;
    public float Acceleration => Speed * 3f;

    private TransformComponent _transform;
    private TransformComponent Transform => _transform ??= Owner.GetComponent<TransformComponent>();

    public override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Velocity = Velocity.MoveTowards(TargetVelocity, Acceleration * dt);

        Transform.Position += Velocity * dt;
    }
}
