using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Source.Game;
internal class Astroid : Component, IUpdate, IInitialize
#if DEBUG
    , IDraw
#endif
    {
    public const float MAX_SIZE = 3f;
    private const float DEFAULT_SPEED = 10f;
    private float _speed;
    private bool _indestructible;
    private PlayerHealth _playerHealth;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;

    public bool Indestructible {
        get => _indestructible;
    }

    public void Initialize() {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = _playerHealth.Transform;

        //set a random rotation
        Transform.rotation = Randomizer.NextFloat(0f, MathF.PI * 2);
        float scale;

        if (Randomizer.NextSingle() < 0.1f) { //1% chance
            _indestructible = true;
            scale = MAX_SIZE;
            Transform.position.X += (_playerTransform.position.X - Transform.position.X) / 1.5f;
            _spriteRenderer.spriteData.tint = new Color(0xFF888888);
        }
        else {
            _indestructible = false;
            scale = Randomizer.NextFloat(1f, MAX_SIZE);
        }

        Transform.scale = Vector2.One * scale;
        _speed = DEFAULT_SPEED * (MAX_SIZE - scale + 1f); //calculate speed depending on the scale (higher size = lower speed)
    }

    public void Update(GameTime gameTime) {
        {
            //get the direction towards the player
            Vector2 direction = Transform.position - _playerTransform.position;
            direction.Normalize();

            //update the transform of the astroid so it moves down and slightly towards the player
            Transform.rotation += MathF.PI / 180 * 20 * Time.deltaTime; //rotate the astroid
            Transform.position.Y += _speed * Time.deltaTime; //move the astroid down
            Transform.position.X += direction.X * 0.1f * Time.deltaTime; //move the astroid slightly towards the player
        }

        //check whether the player is within the bounds of the astroid
        {
            Vector2 position = _playerTransform.position;
            float xPos = _playerTransform.position.X;
            float xScale = _playerTransform.scale.X;
            position.X = Math.Clamp(Transform.position.X, xPos - xScale, xPos + xScale); //BUG: texture size not calculated
            if (IsWithinBounds(_playerTransform.position)) {
                _playerHealth.Damage(1f); //remove 1 health from the player
                GameObject.Dispose(); //dispose of ourselves
            }
        }

        //dispose if the astroid exits the screen
        GameManager game = GameManager.Instance;
        int height = game.GraphicsDevice.Viewport.Height;
        if (Transform.position.Y - (_spriteRenderer.TextureSize.Y / 2) > height) {
            GameObject.Dispose();
        }
    }

    //checks whether the point is within the bounds of the circle
    public bool IsWithinBounds(Vector2 point) {
        float radius = (_spriteRenderer.UnscaledTextureSize.X - 10) * Transform.scale.X;
        Vector2 local = point - Transform.position; //calculate the local position

        //if the local point is outside the square that the radius fits in
        if ((MathF.Abs(local.X) <= radius &&
            MathF.Abs(local.Y) <= radius) == false) {
            return false;
        }

        Vector2 circlePoint = local;
        circlePoint *= 1f / MathF.Sqrt((circlePoint.X * circlePoint.X) + (circlePoint.Y * circlePoint.Y));
        circlePoint *= radius;

        return
            MathF.Abs(local.X) <= MathF.Abs(float.IsFinite(circlePoint.X) ? circlePoint.X : 0f) &&
            MathF.Abs(local.Y) <= MathF.Abs(float.IsFinite(circlePoint.Y) ? circlePoint.Y : 0f);
    }

#if DEBUG
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        float radius = (_spriteRenderer.UnscaledTextureSize.X - 10) * Transform.scale.X;
        int iRadius = (int)MathF.Round(radius);
        Texture2D texture;
        if (textures.TryGetValue(iRadius, out texture) == false) {
            texture = GetTexture(iRadius);
            textures.Add(iRadius, texture);
        }

        spriteBatch.Draw(texture, Transform.position, null, new(0xFFFFFFFF), 0f, Transform.origin * _spriteRenderer.UnscaledTextureSize, 1f, SpriteEffects.None, -1f);
    }

    static Dictionary<int, Texture2D> textures = new();
    static Texture2D GetTexture(int radius) {
        Color[,] colorData = new Color[radius, radius];
        Texture2D textureOut = new(GameManager.Instance.GraphicsDevice, radius * 2, radius);

        for (int x = 0; x < radius; x++) {
            for (int y = 0; y < radius; y++) {
                if ()
            }
        }

        textureOut.SetData(colorData);
        return textureOut;
    }
}
