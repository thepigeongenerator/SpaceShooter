using Microsoft.Xna.Framework;
using System;

namespace SpaceShooter.Source.Core.Components.Colliders;
internal class CircleCollider : Collider {
    public float radius = 1f;

    public float ScaledRadius {
        get => radius * Transform.scale.X; //BUG: using scale.X for scaled radius ._.
    }

    public override Vector2 GetContactPoint(Vector2 position) {
        Vector2 relativePos = -(Transform.position - position);
        relativePos.Normalize();
        relativePos *= ScaledRadius;
        return relativePos + Transform.position;
    }

    public override bool InCollider(Vector2 position) {
        Vector2 relativePos = -(Transform.position - position);

        //check whether the position is not within the radius
        if ((ScaledRadius >= MathF.Abs(relativePos.X) ||
            ScaledRadius >= MathF.Abs(relativePos.Y)) == false) {
            return false;
        }

        //calculate where on the circle the point would end up
        Vector2 hitCirclePos = relativePos;
        hitCirclePos.Normalize();
        hitCirclePos *= ScaledRadius;

        return
            MathF.Abs(hitCirclePos.X) >= MathF.Abs(relativePos.X) &&
            MathF.Abs(hitCirclePos.Y) >= MathF.Abs(relativePos.Y);
        //MathF.Abs(hitCirclePos.X) >= MathF.Abs(0) &&
        //MathF.Abs(hitCirclePos.Y) >= MathF.Abs(0);
    }
}
