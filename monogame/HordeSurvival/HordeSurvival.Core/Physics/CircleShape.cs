using System;
using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Physics;

public readonly struct CircleShape(float radius) : IShape
{
    public float Radius { get; } = radius;

    public bool Overlaps(Vector2 position, IShape other, Vector2 otherPosition)
    {
        if (other is CircleShape circle)
        {
            var radiusSum = Radius + circle.Radius;
            return Vector2.DistanceSquared(position, otherPosition) <= radiusSum * radiusSum;
        }

        throw new NotSupportedException(
            $"Overlap between {nameof(CircleShape)} and {other.GetType().Name} is not supported.");
    }
}
