using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Source.Core;
using SpaceShooter.Source.Core.ScriptComponent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Source.Core.Components;
using SpaceShooter.Source.Game;

namespace Source.Core;
internal class GameManager : Game {
    private static GameManager _instance;
    private readonly List<GameObject> _gameObjects;
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private bool _initialized;
    private bool _loaded;

    private GameManager() {
        //init fields
        _gameObjects = new List<GameObject>();
        _graphics = new GraphicsDeviceManager(this);
        _initialized = false;
        _loaded = false;

        //init obj
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    public static GameManager Instance {
        get {
            _instance ??= new GameManager();
            return _instance;
        }
    }

    public bool Initialized => _initialized;
    public bool Loaded => _loaded;

    public void AddGameObject(GameObject gameObject) {
        _gameObjects.Add(gameObject);
    }

    //called after the constructor
    protected override void Initialize() {
        Console.WriteLine("Making a GameObject");

        //add a gameObject
        GameObject gameObject = new();
        gameObject.AddComponent<Spinner>();
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.spriteData.textureData.name = "HighPigeon";

        Console.WriteLine("finished GameObject!");

        Console.WriteLine("started calling initialize...");
        UpdateGameObjects(EventType.INITIALIZE);
        _initialized = true;
        Console.WriteLine("finished initialize!");

        base.Initialize();
    }

    //called after initialize; loads the content
    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Content.Load<Texture2D>("HighPigeon");

        Console.WriteLine("started calling load...");
        UpdateGameObjects(EventType.LOAD);
        _loaded = true;
        Console.WriteLine("finished load!");
    }

    //called on every game update
    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        UpdateGameObjects(EventType.UPDATE, gameTime);

        base.Update(gameTime);
    }

    //draws every frame
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        UpdateGameObjects(EventType.DRAW, gameTime, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateGameObjects(EventType eventType, params object[] args) {
        //get the type which will be used to check which event will be implemented
        Type checkType = eventType switch {
            EventType.INITIALIZE => typeof(IInitialize),
            EventType.LOAD => typeof(ILoad),
            EventType.UPDATE => typeof(IUpdate),
            EventType.DRAW => typeof(IDraw),
            _ => throw new NotImplementedException(),
        };

        List<Task> callEvents = new(); //list of the tasks which is calling the methods within the gameObjects
                                       //loop through the gameObjects within the game
        foreach (GameObject gameObject in _gameObjects) {
            IReadOnlyCollection<Component> components = gameObject.GetComponents(); //get the components from the gameObject
                                                                                    //loop through the components in the gameObject
            foreach (Component component in components) {
                //if the component's type doesn't implement any of the scriptable types, continue to the next element
                if (component.GetType().GetInterface(checkType.FullName) == null) {
                    continue;
                }

                //create a task of calling the method
                Task task = eventType switch {
                    EventType.INITIALIZE //call the initialize method
                        => Task.Run(() => ((IInitialize)component).Initialize()), //calling Initialize()
                    EventType.LOAD //call the load content method
                        => Task.Run(() => ((ILoad)component).Load()), //calling LoadContent()
                    EventType.UPDATE //call the update method with the correct arguments
                        => Task.Run(() => ((IUpdate)component).Update((GameTime)args[0])), //calling Update()
                    EventType.DRAW //call the drawing method with the correct arguments
                        => Task.Run(() => ((IDraw)component).Draw((GameTime)args[0], (SpriteBatch)args[1])), //calling Draw()
                    _ => throw new NotImplementedException(),
                };

                //store the task in the list
                callEvents.Add(task);
            }
        }

        //await all the tasks being done
        Task.WhenAll(callEvents).Wait();
    }
}
