using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;

namespace SpaceShooter.Source.Game;
internal class Shooting : Component, IUpdate {
    private const int DELAY_MILISECONDS = 1000;
    private TimeSpan _timedOutTill = TimeSpan.Zero;

    public void Update(GameTime gameTime) {
        if (gameTime.TotalGameTime < _timedOutTill) {
            return;
        }

        {
            GameObject bullet = new();
            bullet.Transform.position = Transform.position;
            SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
            spriteRenderer.spriteData.textureData.name = "spaceship/spaceship_0";
            bullet.AddComponent<Bullet>();
        }

        _timedOutTill = gameTime.TotalGameTime + (TimeSpan.FromMilliseconds(DELAY_MILISECONDS) / Time.timeScale);
    }
}
