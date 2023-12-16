using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.Core.Data;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.ScriptComponent;
using System;

namespace Source.Core.Components;
internal class SpriteRenderer : Component, IDraw, ILoad {
    public SpriteData spriteData = new() {
        textureData = new(),
        tint = Color.White,
        origin = Vector2.Zero,
        effects = SpriteEffects.None,
        layerDepth = 0,
    };

    public SpriteRenderer() {
        Console.WriteLine("constructin'");
    }

    public void Load() {
        //load the texture
        Console.WriteLine("loadin'");
        spriteData.textureData.texture2D = GameManager.Instance.Content.Load<Texture2D>(spriteData.textureData.name);
        Console.WriteLine("stopped loadin'");
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        //draw the sprite using _spriteData to the screen
        spriteBatch.Draw(
            spriteData.textureData.texture2D,
            Transform.position,
            null,
            spriteData.tint,
            Transform.rotation,
            spriteData.origin,
            Transform.scale,
            spriteData.effects,
            spriteData.layerDepth
            );
    }
}
