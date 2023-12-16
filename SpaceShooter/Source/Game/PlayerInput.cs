using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.ScriptComponent;

namespace SpaceShooter.Source.Game;
internal class PlayerInput : Component, IUpdate {
    const float SPEED = 1f;

    public void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.A)) {
            Transform.position.X -= 1 * SPEED;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D)) {
            Transform.position.X += 1 * SPEED;
        }
    }
}
