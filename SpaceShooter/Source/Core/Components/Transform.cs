using Microsoft.Xna.Framework;
using System;

namespace SpaceShooter.Source.Core.Components;
internal class Transform : Component {
    private const float GET_UP = MathF.PI * 1f;
    private const float GET_RIGHT = MathF.PI * 0.5f;
    private const float GET_DOWN = MathF.PI * 0f;
    private const float GET_LEFT = MathF.PI * 1.25f;

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

    /// <summary>gets a normalized vector based upon the rotation of this object in the up direction</summary>
    public Vector2 Up => GetDirection(rotation + GET_UP);

    /// <summary>gets a normalized vector based upon the rotation of this object in the right direction</summary>
    public Vector2 Right => GetDirection(rotation + GET_RIGHT);

    /// <summary>gets a normalized vector based upon the rotation of this object in the down direction</summary>
    public Vector2 Down => GetDirection(rotation + GET_DOWN);

    /// <summary>gets a normalized vector based upon the rotation of this object in the left direction</summary>
    public Vector2 Left => GetDirection(rotation + GET_LEFT);

    private static Vector2 GetDirection(float rotation) {
        (float x, float y) = MathF.SinCos(rotation);
        return new Vector2(x, y);
    }
}
