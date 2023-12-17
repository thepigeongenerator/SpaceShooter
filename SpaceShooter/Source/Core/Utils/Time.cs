using Microsoft.Xna.Framework;

namespace SpaceShooter.Source.Core.Utils;
internal static class Time {
    public static float timeScale = 1f;

    public static float GetDeltaTime(GameTime gameTime) {
        return (float)gameTime.ElapsedGameTime.TotalSeconds * timeScale;
    }
}
