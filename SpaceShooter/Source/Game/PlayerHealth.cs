using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;

namespace SpaceShooter.Source.Game;
internal class PlayerHealth : Component, IInitialize, IUpdate {
    private SpriteRenderer _spriteRenderer;
    private float _health = 10f;

    public float Health {
        get => _health;
    }

    //damages the playerdd
    public void Damage(float value) {
        _health -= value;
        _spriteRenderer.spriteData.tint = Color.Red;

        //if the health is now 0; end the game
        if (_health <= 0f) {
            GameObject.Dispose();
            GameManager.Instance.Exit();
        }
    }

    public void Initialize() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update(GameTime gameTime) {
        ref Color tint = ref _spriteRenderer.spriteData.tint;

        if (tint == Color.White) {
            return;
        }

        tint = Color.Lerp(tint, Color.White, 0.1f);
    }
}
