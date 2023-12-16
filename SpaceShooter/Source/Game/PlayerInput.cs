using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Source.Core;
using Source.Core.Components;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.ScriptComponent;

namespace SpaceShooter.Source.Game;
internal class PlayerInput : Component, IUpdate, ILoad {
    private const float SPEED = 5f;
    private int _distanceToCentre;

    public void Load() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Texture2D texture = spriteRenderer.spriteData.textureData.texture2D;
        _distanceToCentre = texture.Width / 2;
    }

    public void Update(GameTime gameTime) {
        //move left
        if (Keyboard.GetState().IsKeyDown(Keys.A)) {
            Transform.position.X -= 1 * SPEED;
        }

        //move right
        if (Keyboard.GetState().IsKeyDown(Keys.D)) {
            Transform.position.X += 1 * SPEED;
        }

        GameManager game = GameManager.Instance;
        int windowWidth = game.GraphicsDevice.Viewport.Bounds.Width;
        //hit left
        if ((Transform.position.X + _distanceToCentre) < 0) {
            Transform.position.X += windowWidth;
        }
        //hit right
        if ((Transform.position.X + _distanceToCentre) > windowWidth) {
            Transform.position.X -= windowWidth;
        }

    }
}
