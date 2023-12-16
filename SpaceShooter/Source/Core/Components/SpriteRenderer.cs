using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Source.Core.Data;
using System;
using System.Threading.Tasks;

namespace Source.Core.Components;
internal class SpriteRenderer : ScriptComponent {
    public SpriteData spriteData = new() {
        textureData = new(),
        tint = Color.White,
        origin = Vector2.Zero,
        effects = SpriteEffects.None,
        layerDepth = 0,
    };

    protected override void Load(ContentManager content) {
        //load the texture
        //spriteData.textureData.texture2D = content.Load<Texture2D>(spriteData.textureData.name);
    }

    protected override void Draw(SpriteBatch spriteBatch) {
        //draw the sprite using _spriteData to the screen
        spriteBatch.Draw(
            spriteData.textureData.texture2D,
            gameObject.transform.position,
            null,
            spriteData.tint,
            gameObject.transform.rotation,
            spriteData.origin,
            gameObject.transform.scale,
            spriteData.effects,
            spriteData.layerDepth
            );
    }
}
