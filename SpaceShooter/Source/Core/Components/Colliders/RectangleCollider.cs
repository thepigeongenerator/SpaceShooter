using Microsoft.Xna.Framework;
using System;

namespace SpaceShooter.Source.Core.Components.Colliders;
internal class RectangleCollider : Collider {
    public Vector2 collisionArea = Vector2.One / 2; //1x1 collider

    public Vector2 ScaledCollisionArea => collisionArea * Transform.scale;
    public Vector2 CornerA => new Vector2(-ScaledCollisionArea.X, -ScaledCollisionArea.Y) + Transform.position;
    public Vector2 CornerB => new Vector2(ScaledCollisionArea.X, -ScaledCollisionArea.Y) + Transform.position;
    public Vector2 CornerC => new Vector2(-ScaledCollisionArea.X, ScaledCollisionArea.Y) + Transform.position;
    public Vector2 CornerD => new Vector2(ScaledCollisionArea.X, ScaledCollisionArea.Y) + Transform.position;

    public override Vector2 GetContactPoint(Vector2 position) {
        float GetSurfaceArea(Vector2 globalCornerPos) {
            Vector2 difference = globalCornerPos - position;
            return MathF.Abs(difference.X) * MathF.Abs(difference.Y);
        }

        Vector2[] corners = new Vector2[4] {
            CornerA, CornerB, CornerC, CornerD
        };

        //get which corner is the closest using surface area
        Vector2 nearestCorner = Vector2.Zero;
        float leastSurfaceArea = -1f;
        for (int i = 0; i < 4; i++) {
            float surfaceArea = GetSurfaceArea(corners[i]);
            if (leastSurfaceArea < surfaceArea) {
                nearestCorner = corners[i];
            }
        }

        return nearestCorner;
    }

    public override bool InCollider(Vector2 position) {
        //convert the position to be relative to Transform.position
        Vector2 relativePos = -(Transform.position - position);

        return
            ScaledCollisionArea.X >= MathF.Abs(relativePos.X) &&
            ScaledCollisionArea.Y >= MathF.Abs(relativePos.Y);
    }
}
