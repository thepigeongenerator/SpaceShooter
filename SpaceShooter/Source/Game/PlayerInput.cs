using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;

namespace SpaceShooter.Source.Game;
internal class PlayerInput : Component, IUpdate, IInitialize {
    private const float SPEED = 5f;

    public void Initialize() {
        GameManager game = GameManager.Instance;
        Transform.origin.Y = 1; //set origin Y to 0 to be at the bottom of the sprite
        Transform.position.X = game.GraphicsDevice.Viewport.Bounds.Width / 2;
        Transform.position.Y = game.GraphicsDevice.Viewport.Bounds.Height; //set the Y position to the height of the screen; align with the bottom
    }

    public void Update(GameTime gameTime) {
        //move left handling
        if (Keyboard.GetState().IsKeyDown(Keys.A)) {
            Transform.position.X -= 1 * SPEED;
        }

        //move right handling
        if (Keyboard.GetState().IsKeyDown(Keys.D)) {
            Transform.position.X += 1 * SPEED;
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
