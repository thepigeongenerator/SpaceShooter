using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Source.Core.Components;
using Source.Core.Data;
using SpaceShooter.Source.Core.Data;
using SpaceShooter.Source.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Source.Core;
internal class SpaceInvadersGame : Game {
    private readonly List<GameObject> _gameObjects;
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public SpaceInvadersGame() {
        _gameObjects = new();
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        GameObject gameObject = new();
        AddGameObject(gameObject);
        gameObject.AddComponent<Spinner>();
    }
    public void AddGameObject(GameObject gameObject) {
        _gameObjects.Add(gameObject);
    }

    protected override void Initialize() {
        UpdateGameObjects((events) => events.init?.Invoke());

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);


        SpriteRenderer renderer = _gameObjects[0].AddComponent<SpriteRenderer>();
        renderer.spriteData.textureData.name = "HighPigeon";
        renderer.spriteData.textureData.texture2D = Content.Load<Texture2D>(renderer.spriteData.textureData.name);

        UpdateGameObjects((events) => events.load?.Invoke(Content));
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        UpdateGameObjects((events) => events.update?.Invoke(gameTime));

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        UpdateGameObjects((events) => events.draw?.Invoke(_spriteBatch));
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateGameObjects(Action<GameEvents> callEvent) {
        List<Task> updateTasks = new();
        for (int i = 0; i < _gameObjects.Count; i++) {
            IReadOnlyCollection<Component> components = _gameObjects[i].GetComponents();

            for (int j = 0; j < components.Count; j++) {
                if (components.ElementAt(j) is ScriptComponent component) {
                    //add the call to the task list
                    Task task = Task.Run(() => callEvent.Invoke(component.gameEvents));
                    updateTasks.Add(task);
                }
            }
        }
        
        Task.WhenAll(updateTasks).Wait();
    }
}
