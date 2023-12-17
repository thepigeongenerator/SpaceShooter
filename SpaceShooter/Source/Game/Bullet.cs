using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;

namespace SpaceShooter.Source.Game;
internal class Bullet : Component, IUpdate {
    private const float SPEED = 500;

    public void Update(GameTime gameTime) {
        Transform.position.Y -= 1 * SPEED * Time.deltaTime;

        //destroy self once it is no-longer visible
        if (Transform.position.Y < 0) {
            GameObject.Dispose();
        }
    }
}
