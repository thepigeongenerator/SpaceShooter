using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;

namespace SpaceShooter.Source.Game;
internal class Spinner : Component, IUpdate {
    public void Update(GameTime gameTime) {
        Transform.rotation += MathF.PI / 180 * 20 * Time.deltaTime;
    }
}
