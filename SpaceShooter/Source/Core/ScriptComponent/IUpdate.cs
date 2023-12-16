using Microsoft.Xna.Framework;

namespace SpaceShooter.Source.Core.ScriptComponent;
internal interface IUpdate {
    /// <summary>
    /// called on every game update
    /// </summary>
    public abstract void Update(GameTime gameTime);
}
