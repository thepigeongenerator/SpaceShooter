using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Source.Core.Data;
using SpaceShooter.Source.Core.ScriptComponent;

namespace SpaceShooter.Source.Core.Components;
internal class SpriteRenderer : Component, IDraw, ILoadContent {
    //TODO: add easy texture sizing which includes scaling
    private Vector2 _origin;
    public SpriteData spriteData = new() {
        textureData = new(),
        tint = Color.White,
        effects = SpriteEffects.None,
        layerDepth = 0,
    };

    public void LoadContent() {
        //load the texture
        spriteData.textureData.texture2D = GameManager.Instance.Content.Load<Texture2D>(spriteData.textureData.name);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        //draw the sprite using _spriteData to the screen
        spriteBatch.Draw(
            spriteData.textureData.texture2D,
            Transform.position,
            null,
            spriteData.tint,
            Transform.rotation,
            Transform.origin * new Vector2(
                spriteData.textureData.texture2D.Width,
                spriteData.textureData.texture2D.Height),
            Transform.scale,
            spriteData.effects,
            spriteData.layerDepth
            );
    }
}
