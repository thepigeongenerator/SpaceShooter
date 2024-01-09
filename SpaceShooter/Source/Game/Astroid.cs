using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;

namespace SpaceShooter.Source.Game;
internal class Astroid : Component, IUpdate, IInitialize {
    private const float MAX_SIZE = 3f;
    private const float SPEED = 10f;
    private PlayerHealth _playerHealth;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;

    public void Initialize() {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = _playerHealth.Transform;

        Transform.scale = Vector2.One * Randomizer.NextFloat(1f, MAX_SIZE);
    }

    public void Update(GameTime gameTime) {
        {
            //get the direction towards the player
            Vector2 direction = Transform.position - _playerTransform.position;
            direction.Normalize();

            //update the transform of the astroid so it moves down and slightly towards the player
            Transform.rotation += MathF.PI / 180 * 20 * Time.deltaTime; //rotate the astroid
            Transform.position.Y += SPEED * Time.deltaTime; //move the astroid down
            Transform.position.X += direction.X * 0.1f * Time.deltaTime; //move the astroid slightly towards the player
        }

        //check whether the player is within the bounds of the astroid
        if (IsWithinBounds(_playerTransform.position)) {
            _playerHealth.Damage(1f); //remove 1 health from the player
            GameObject.Dispose(); //dispose of ourselves
        }

        //dispose if the astroid exits the screen
        GameManager game = GameManager.Instance;
        int height = game.GraphicsDevice.Viewport.Height;
        if (Transform.position.Y - (_spriteRenderer.TextureSize.Y / 2) > height) {
            GameObject.Dispose();
        }
    }

    //checks whether the point is within the bounds of the circle
    public bool IsWithinBounds(Vector2 point) {
        float radius = _spriteRenderer.TextureSize.X;
        Vector2 local = point - Transform.position; //calculate the local position

        //if the local point is outside the square that the radius fits in
        if ((MathF.Abs(local.X) <= radius &&
            MathF.Abs(local.Y) <= radius) == false) {
            return false;
        }

        Vector2 circlePoint = local;
        circlePoint *= 1f / MathF.Sqrt((circlePoint.X * circlePoint.X) + (circlePoint.Y * circlePoint.Y));
        circlePoint *= radius;

        return
            MathF.Abs(local.X) <= MathF.Abs(float.IsFinite(circlePoint.X) ? circlePoint.X : 0f) &&
            MathF.Abs(local.Y) <= MathF.Abs(float.IsFinite(circlePoint.Y) ? circlePoint.Y : 0f);
    }
}
