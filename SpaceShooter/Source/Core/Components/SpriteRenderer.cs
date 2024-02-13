using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapeDrawer;
using SpaceShooter.Source.Core.Data;
using SpaceShooter.Source.Core.ScriptComponent;

namespace SpaceShooter.Source.Core.Components;
internal class SpriteRenderer : Component, IDraw, ILoadContent {
    public SpriteData spriteData = new() {
        textureData = new(),
        tint = Color.White,
        effects = SpriteEffects.None,
        layerDepth = 0,
    };

    /// <summary>
    /// gets the texture size <see cref="Transform.scale"/>, use <see cref="TextureSize"/> for scaled texture sizes
    /// </summary>
    public Vector2 UnscaledTextureSize {
        get => new(
                spriteData.textureData.texture2D.Width,
                spriteData.textureData.texture2D.Height
            );
    }

    /// <summary>
    /// gets the texture size which is scaled with <see cref="Transform.scale"/>, use <see cref="UnscaledTextureSize"/> for unscaled
    /// </summary>
    public Vector2 TextureSize {
        get => Transform.scale * UnscaledTextureSize;
    }

    public void LoadContent() {
        //load the texture
        spriteData.textureData.texture2D ??= GameManager.Instance.Content.Load<Texture2D>(spriteData.textureData.name);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        //draw the sprite using _spriteData to the screen
        spriteBatch.Draw(
            spriteData.textureData.texture2D,
            Transform.position,
            null,
            spriteData.tint,
            Transform.rotation,
            Transform.origin * UnscaledTextureSize,
            Transform.scale,
            spriteData.effects,
            spriteData.layerDepth
            );

#if DEBUG
        DrawShape.Point(spriteBatch, Transform.position, new Color(0xFF0000FF), 1F);
        DrawShape.Box(spriteBatch, Transform.position - (Transform.origin * TextureSize), Transform.position + (Transform.origin * TextureSize), new Color(0xFF333333));
#endif
    }
}
