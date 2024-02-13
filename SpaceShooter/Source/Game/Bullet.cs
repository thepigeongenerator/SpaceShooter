using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapeDrawer;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;

namespace SpaceShooter.Source.Game;
internal class Bullet : Component, IUpdate
#if DEBUG
    , IDraw
#endif
    {
    private const float SPEED = 500;

    public void Update(GameTime gameTime) {
        Transform.position.Y -= 1 * SPEED * Time.deltaTime;

        //destroy self once it is no-longer visible
        if (Transform.position.Y < 0) {
            GameObject.Dispose();
        }

        foreach (Astroid astroid in FindObjectsOfType<Astroid>()) {
            if (astroid.IsWithinBounds(Transform.position)) {
                if (astroid.Indestructible == false) {
                    astroid.GameObject.Dispose();
                }

                GameObject.Dispose();
            }
        }
    }

#if DEBUG
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        DrawShape.Point(spriteBatch, Transform.position, new Color(0xFF00FF00), 5F);
    }
#endif
}
