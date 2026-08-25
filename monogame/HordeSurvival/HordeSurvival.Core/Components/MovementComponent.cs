using HordeSurvival.Core.Extensions;
using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Components;

public class MovementComponent : ComponentBase
{
    public Vector2 Velocity;
    public Vector2 TargetVelocity;

    public float MaxSpeed { get; set; }
    public float TimeToMaxSpeed { get; set; }
    private float Acceleration => MaxSpeed / TimeToMaxSpeed;

    private readonly TransformComponent _transform;

    public MovementComponent(TransformComponent transform, float maxSpeed, float timeToMaxSpeed)
    {
        _transform = transform;
        MaxSpeed = maxSpeed;
        TimeToMaxSpeed = timeToMaxSpeed;
    }

    public override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Velocity = Velocity.MoveTowards(TargetVelocity, Acceleration * dt);

        _transform.Position += Velocity * dt;
    }
}
