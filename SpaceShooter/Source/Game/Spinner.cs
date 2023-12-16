using Microsoft.Xna.Framework;
using Source.Core.Components;
using System;

namespace SpaceShooter.Source.Game;
internal class Spinner : ScriptComponent {
    protected override void Update(GameTime gameTime) {
        float deltaTime = gameTime.ElapsedGameTime.Seconds;
        Transform.rotation += MathF.PI / 180 * deltaTime;
    }
}
