using HordeSurvival.Core.Physics;
using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Components;

public class HitboxComponent : ComponentBase
{
    public IShape Shape { get; set; }
    public Vector2 Offset { get; set; }
    public float Damage { get; set; }
    public bool Enabled { get; set; }

    private readonly TransformComponent _transform;

    public Vector2 WorldPosition => _transform.Position + Offset;

    public HitboxComponent(TransformComponent transform, IShape shape, Vector2 offset, float damage)
    {
        _transform = transform;
        Shape = shape;
        Offset = offset;
        Damage = damage;
    }
}
