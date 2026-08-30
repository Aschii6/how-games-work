using System;
using HordeSurvival.Core.Physics;
using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Components;

public class HurtboxComponent : ComponentBase
{
    public IShape Shape { get; set; }
    public Vector2 Offset { get; set; }

    public float MaxHp { get; }
    public float Hp { get; private set; }
    public float InvulnPeriod { get; }

    public event Action Damaged;
    public event Action Died;

    private readonly TransformComponent _transform;
    private float _invulnRemaining;

    public Vector2 WorldPosition => _transform.Position + Offset;

    public HurtboxComponent(TransformComponent transform, IShape shape, Vector2 offset, float maxHp, float invulnPeriod)
    {
        _transform = transform;
        Shape = shape;
        Offset = offset;
        MaxHp = maxHp;
        Hp = maxHp;
        InvulnPeriod = invulnPeriod;
    }

    public override void Update(GameTime gameTime)
    {
        _invulnRemaining = MathF.Max(0, _invulnRemaining - (float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void TakeDamage(float damage)
    {
        if (_invulnRemaining > 0 || Hp <= 0) return;

        Hp -= damage;
        _invulnRemaining = InvulnPeriod;
        Damaged?.Invoke();

        if (Hp <= 0)
            Died?.Invoke();
    }
}
