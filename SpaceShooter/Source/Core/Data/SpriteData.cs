using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Source.Core.Data;
internal struct SpriteData {
    public TextureData textureData;
    public Color tint;
    public Vector2 origin;
    public SpriteEffects effects;
    public float layerDepth;
}
