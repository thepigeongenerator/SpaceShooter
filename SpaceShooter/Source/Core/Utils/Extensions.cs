using Microsoft.Xna.Framework;

namespace SpaceShooter.Source.Core.Utils;
internal static class Extensions {
    public static float GetDeltaTime(this GameTime gameTime) {
        return (float)gameTime.ElapsedGameTime.TotalSeconds + 1;
    }
}
