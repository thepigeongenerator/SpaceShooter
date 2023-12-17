using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Utils;
using SpaceShooter.Source.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceShooter.Source.Core;
internal class GameManager : Microsoft.Xna.Framework.Game {
    private static GameManager _instance = null;        //holds this instance of the GameManager
    private readonly List<GameObject> _gameObjects;     //the GameObjects active in the game
    private readonly GraphicsDeviceManager _graphics;   //graphics device
    private SpriteBatch _spriteBatch;                   //used for drawing sprites to the screen
    private bool _initialized;      //true when the initialize function has been called
    private bool _contentLoaded;    //true when the loadcontent function was called
    private bool _loaded;           //true when the load event was called

    #region initialization
    private GameManager() {
        //init fields
        _gameObjects = new List<GameObject>();
        _graphics = new GraphicsDeviceManager(this);
        _initialized = false;
        _contentLoaded = false;

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
    #endregion //initializiation

    public bool Initialized => _initialized;
    public bool ContentLoaded => _contentLoaded;
    public bool Loaded => _loaded;

    #region game object management
    public void AddGameObject(GameObject gameObject) {
        _gameObjects.Add(gameObject);
    }

    public void DisposeGameObject(GameObject gameObject) {
        //if the gameObject isn't disposed
        if (gameObject.Disposed == false) {
            //dispose the gameObject and exit, since the gameObject calls this itself
            gameObject.Dispose();
            return;
        }

        _gameObjects.Remove(gameObject);
    }
    #endregion //object management

    #region object finding
    public T FindObjectOfType<T>() where T : Component {
        return FindObjectsOfType<T>()?.FirstOrDefault();
    }

    public IEnumerable<T> FindObjectsOfType<T>() where T : Component {
        return
            from gameObject in _gameObjects
            let components = gameObject.GetComponents<T>()
            where components != null
            from component in components
            select component;
    }
    #endregion //object finding

    #region monogame events
    //called after the constructor
    protected override void Initialize() {

        //TEMPORARY: add a gameObject
        GameObject gameObject = new();
        gameObject.AddComponent<PlayerInput>();
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.spriteData.textureData.name = "spaceship/spaceship_0";
        Animator animator = gameObject.AddComponent<Animator>();
        animator.frames.Add(new Data.TextureData() { name = "spaceship/spaceship_0" });
        animator.frames.Add(new Data.TextureData() { name = "spaceship/spaceship_1" });
        animator.frames.Add(new Data.TextureData() { name = "spaceship/spaceship_2" });
        gameObject.Transform.scale = Vector2.One * 5;

        //set graphics settings
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 960;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        //call Initialize() on the GameObjects
        _initialized = true;
        UpdateGameObjects(EventType.INITIALIZE);

        base.Initialize();
    }

    //called after initialize; loads the content
    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //load all textures that will be loaded
        {
            string[] loadTextures = JsonUtils.DeserializeFromFile<string[]>(@"Content\load_content.json");
            foreach (string name in loadTextures) {
                Content.Load<Texture2D>(name);
            }
        }

        //call the GameObjects to run LoadContent()
        _contentLoaded = true;
        UpdateGameObjects(EventType.LOADCONENT);

        //call the GameObjects to run Load()
        _loaded = true;
        UpdateGameObjects(EventType.LOAD);
    }

    //called on every game update
    protected override void Update(GameTime gameTime) {
        //exit if the escape key was pressed
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //call Update() on all GameObjects
        UpdateGameObjects(EventType.UPDATE, gameTime);

        base.Update(gameTime);
    }

    //draws every frame
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //call the Draw() functions of the gameObjects
        UpdateGameObjects(EventType.DRAW, gameTime, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
    #endregion //monogame events

    private void UpdateGameObjects(EventType eventType, params object[] args) {
        //get the type which will be used to check which event will be implemented
        Type checkType = eventType switch {
            EventType.INITIALIZE => typeof(IInitialize),
            EventType.LOADCONENT => typeof(ILoadContent),
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
                    EventType.LOADCONENT //call the load content method
                        => Task.Run(() => ((ILoadContent)component).LoadContent()), //calling LoadContent()
                    EventType.LOAD //call the load content method
                        => Task.Run(() => ((ILoad)component).Load()), //calling Load()
                    EventType.UPDATE //call the update method with the correct arguments
                        => Task.Run(() => ((IUpdate)component).Update((GameTime)args[0])), //calling Update()
                    EventType.DRAW //call the drawing method with the correct arguments
                        => Task.Run(() => ((IDraw)component).Draw((GameTime)args[0], (SpriteBatch)args[1])), //calling Draw()
                    _ => throw new NotImplementedException($"{eventType} was not implemented"),
                };

                //store the task in the list
                callEvents.Add(task);
            }
        }

        //await all the tasks being done
        Task.WhenAll(callEvents).Wait();
    }
}
