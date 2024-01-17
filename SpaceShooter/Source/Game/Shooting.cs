using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;

namespace SpaceShooter.Source.Game;
internal class Shooting : Component, ILoad, ILoadContent, IUpdate {
    private const int DELAY_MILISECONDS = 200;
    private TimeSpan _timedOutTill = TimeSpan.FromMilliseconds(1000); //initial delay so the player doesn't immediately start shooting
    private Vector2 _bulletPos = Vector2.Zero;
    private Texture2D _bulletTexture;

    public void LoadContent() {
        GameManager game = GameManager.Instance;
        _bulletTexture = game.Content.Load<Texture2D>("bullet");
    }

    public void Load() {
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        _bulletPos.Y = -spriteRender.TextureSize.Y + _bulletTexture.Height;
    }

    public void Update(GameTime gameTime) {
        if (gameTime.TotalGameTime < _timedOutTill) {
            return;
        }

        {
            GameObject bullet = new();
            bullet.Transform.position = Transform.position + _bulletPos;
            bullet.Transform.scale = Vector2.One * 4;
            bullet.Transform.origin = new Vector2(0.5f, 1f);
            SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
            spriteRenderer.spriteData.textureData.texture2D = _bulletTexture;
            bullet.AddComponent<Bullet>();
        }

        _timedOutTill = gameTime.TotalGameTime + (TimeSpan.FromMilliseconds(DELAY_MILISECONDS) / Time.timeScale);
    }
}
