using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;

namespace SpaceShooter.Source.Game;
internal class Astroid : Component, IUpdate, IInitialize {
    private const float SPEED = 10f;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;

    public void Initialize() {
        PlayerInput input = FindObjectOfType<PlayerInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = input.Transform;
    }

    public void Update(GameTime gameTime) {
        //get the direction towards the player
        Vector2 direction = _playerTransform.position - Transform.position;
        direction.Normalize();

        //update the transform of the astroid
        Transform.rotation += MathF.PI / 180 * 20 * Time.deltaTime; //rotate the astroid
        Transform.position.Y += 1 * SPEED * Time.deltaTime; //move the astroid down
        Transform.position.X += direction.X * 1f * Time.deltaTime; //move the astroid slightly towards the player

        GameManager game = GameManager.Instance;
        int height = game.GraphicsDevice.Viewport.Height;
        if ((_spriteRenderer.TextureSize.Y / 2) - Transform.position.Y > height) {
            GameObject.Dispose();
        }
    }
}
