using HordeSurvival.Core.Components;

namespace HordeSurvival.Core.Combat;

public static class CombatSystem
{
    public static void TryHit(HitboxComponent hitbox, HurtboxComponent hurtbox)
    {
        if (!hitbox.Enabled) return;

        if (hitbox.Shape.Overlaps(hitbox.WorldPosition, hurtbox.Shape, hurtbox.WorldPosition))
            hurtbox.TakeDamage(hitbox.Damage);
    }
}
