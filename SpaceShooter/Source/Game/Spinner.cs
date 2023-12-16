using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;

namespace SpaceShooter.Source.Game;
internal class Spinner : Component, IUpdate, IInitialize {
    public void Initialize() {
        Transform.position = Vector2.One * 5;
    }

    public void Update(GameTime gameTime) {
        Transform.rotation += MathF.PI / 180 * 0.1f * gameTime.GetDeltaTime();
    }
}
