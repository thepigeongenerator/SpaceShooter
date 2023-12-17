using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;

namespace SpaceShooter.Source.Game;
internal class PlayerInput : Component, IUpdate, IInitialize {
    private const float SPEED = 500f;
    private const float SPEEDBOOST = 3f;

    public void Initialize() {
        GameManager game = GameManager.Instance;
        Transform.origin.Y = 1; //set origin Y to 0 to be at the bottom of the sprite
        Transform.position.X = game.GraphicsDevice.Viewport.Bounds.Width / 2;
        Transform.position.Y = game.GraphicsDevice.Viewport.Bounds.Height; //set the Y position to the height of the screen; align with the bottom
    }

    public void Update(GameTime gameTime) {
        //move left handling
        float speed = SPEED * Time.deltaTime;

        if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
            speed *= SPEEDBOOST;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) {
            Transform.position.X -= 1 * speed;
        }

        //move right handling
        if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) {
            Transform.position.X += 1 * speed;
        }

        GameManager game = GameManager.Instance;
        int windowWidth = game.GraphicsDevice.Viewport.Bounds.Width;

        //hit left bound
        if (Transform.position.X < 0) {
            Transform.position.X += windowWidth;
        }
        //hit right bound
        if (Transform.position.X > windowWidth) {
            Transform.position.X -= windowWidth;
        }
    }
}
