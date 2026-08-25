using System;
using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Extensions;

public static class Vector2Extensions
{
    public static Vector2 MoveTowards(this Vector2 current, Vector2 target, float maxDistanceDelta)
    {
        var toTarget = target - current;
        var distSquared = toTarget.LengthSquared();

        if (distSquared == 0f || distSquared <= maxDistanceDelta * maxDistanceDelta)
            return target;

        return current + toTarget / MathF.Sqrt(distSquared) * maxDistanceDelta;
    }
}
