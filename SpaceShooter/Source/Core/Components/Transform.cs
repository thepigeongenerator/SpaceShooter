using Microsoft.Xna.Framework;

namespace SpaceShooter.Source.Core.Components;
internal class Transform : Component {
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public Vector2 origin;

    public Transform() {
        position = Vector2.Zero;
        rotation = 0;
        scale = Vector2.One;
        origin = Vector2.One / 2;
    }
}
