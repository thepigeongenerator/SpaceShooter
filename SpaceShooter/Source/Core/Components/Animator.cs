using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Source.Core.Data;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Source.Core.Components;

internal class Animator : Component, IUpdate, ILoadContent {
    //public fields
    public int delayMiliSeconds = 42;           //~24 fps
    public bool enabled = true;                 //whether the animator is enabled or not
    public Func<bool> condition = () => true;   //a potential condition to disable the animator more dynamically
    public List<TextureData> frames = new();    //a collection of the frames of the animator

    //private fields
    private TimeSpan _timedOutTill = TimeSpan.Zero;
    private SpriteRenderer _spriteRenderer;
    private int _frameIndex = 0; //the index of the frame we're on

    public void LoadContent() {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //load the textures
        GameManager game = GameManager.Instance;
        for (int i = 0; i < frames.Count; i++) {
            TextureData frame = frames[i];
            frame.texture2D = game.Content.Load<Texture2D>(frames[i].name);
            frames[i] = frame;
        }
    }

    //called every frame update
    public void Update(GameTime gameTime) {
        //conditions to pass to run the animator
        if (enabled && condition.Invoke() && (gameTime.TotalGameTime * Time.timeScale) < _timedOutTill) {
            return;
        }
        
        //timeout over
        _spriteRenderer.spriteData.textureData = frames[_frameIndex]; //swap out the current texture with the texture in the animator

        _frameIndex++; //incriment the frame index by 1

        //set the frameIndex back to 0 if it becomes out of bounds
        if (_frameIndex >= frames.Count) {
            _frameIndex = 0;
        }

        //reset the timeout
        _timedOutTill = (gameTime.TotalGameTime * Time.timeScale) + TimeSpan.FromMilliseconds(delayMiliSeconds);
    }
}
