using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Source.Core.ScriptComponent;
internal interface IDraw {
    /// <summary>
    /// called before every frame drawn
    /// </summary>
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}
