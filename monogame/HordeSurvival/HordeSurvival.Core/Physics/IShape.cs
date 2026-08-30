using Microsoft.Xna.Framework;

namespace HordeSurvival.Core.Physics;

public interface IShape
{
    bool Overlaps(Vector2 position, IShape other, Vector2 otherPosition);
}
