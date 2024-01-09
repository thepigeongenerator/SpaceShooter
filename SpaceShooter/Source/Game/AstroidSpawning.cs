using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;
using System.Diagnostics;

namespace SpaceShooter.Source.Game;
internal class AstroidSpawning : Component, IUpdate, ILoadContent {
    private const int MAX_SPAWNDELAY = 100;
    private int _spawnDelay = 1000; //spawn delay in miliseconds
    private TimeSpan _timedOutTill = TimeSpan.FromMilliseconds(1000); //set initial delay; so the player isn't bombarded from the start
    private Texture2D _astroidTexture;

    public void LoadContent() {
        GameManager game = GameManager.Instance;
        _astroidTexture = game.Content.Load<Texture2D>("astroid");
    }

    public void Update(GameTime gameTime) {
        if (gameTime.TotalGameTime < _timedOutTill) {
            return;
        }

        //astroid spawning
        {
            GameManager game = GameManager.Instance;
            int maxX = game.GraphicsDevice.Viewport.Width;
            Vector2 position = Vector2.Zero;

            //get position
            position.X = Randomizer.Next(0, maxX + 1);

            SpawnAstroid(position);
        }

        //slowly decrease the delay
        if (_spawnDelay > MAX_SPAWNDELAY) {
            _spawnDelay -= 1;
        }

        _timedOutTill = gameTime.TotalGameTime + (TimeSpan.FromMilliseconds(_spawnDelay) / Time.timeScale);
    }

    public void SpawnAstroid(Vector2 position) {
        GameObject astroid = new();
        astroid.Transform.position = position;
        astroid.AddComponent<Astroid>();
        SpriteRenderer spriteRenderer = astroid.AddComponent<SpriteRenderer>();
        spriteRenderer.spriteData.textureData.texture2D = _astroidTexture;
    }
}
