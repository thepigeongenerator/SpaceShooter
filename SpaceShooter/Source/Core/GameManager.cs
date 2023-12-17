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
    private static GameManager _instance = null;            //holds this instance of the GameManager
    private readonly List<GameObject> _loadedGameObjects;   //the GameObjects active in the game
    private readonly List<GameObject> _loadGameObjectQue;   //the GameObjects that still need to be loaded
    private readonly GraphicsDeviceManager _graphics;       //graphics device
    private SpriteBatch _spriteBatch;                       //used for drawing sprites to the screen
    private bool _initialized = false;

    #region initialization
    private GameManager() {
        //init fields
        _loadedGameObjects = new List<GameObject>();
        _loadGameObjectQue = new List<GameObject>();
        _graphics = new GraphicsDeviceManager(this);

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

    #region game object management
    public void AddGameObject(GameObject gameObject) {
        if (_initialized == false) {
            _loadedGameObjects.Add(gameObject);
        }
        else {
            _loadGameObjectQue.Add(gameObject);
        }
    }

    public void DisposeGameObject(GameObject gameObject) {
        //if the gameObject isn't disposed
        if (gameObject.Disposed == false) {
            //dispose the gameObject and exit, since the gameObject calls this itself
            gameObject.Dispose();
            return;
        }

        _loadedGameObjects.Remove(gameObject);
    }
    #endregion //object management

    #region object finding
    public T FindObjectOfType<T>() where T : Component {
        return FindObjectsOfType<T>()?.FirstOrDefault();
    }

    public IEnumerable<T> FindObjectsOfType<T>() where T : Component {
        return
            from gameObject in _loadedGameObjects
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
        gameObject.AddComponent<Shooting>();
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.spriteData.textureData.name = "spaceship/spaceship_0";
        Animator animator = gameObject.AddComponent<Animator>();
        animator.frames.Add(new Data.TextureData() { name = "spaceship/spaceship_0" });
        animator.frames.Add(new Data.TextureData() { name = "spaceship/spaceship_1" });
        animator.frames.Add(new Data.TextureData() { name = "spaceship/spaceship_2" });
        animator.delayMiliSeconds = 100;
        gameObject.Transform.scale = Vector2.One * 5;

        //set graphics settings
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 960;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        //call Initialize() on the GameObjects
        _initialized = true;
        UpdateGameObjects(EventType.INITIALIZE, _loadedGameObjects);

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
        UpdateGameObjects(EventType.LOADCONENT, _loadedGameObjects);

        //call the GameObjects to run Load()
        UpdateGameObjects(EventType.LOAD, _loadedGameObjects);
    }

    //called on every game update
    protected override void Update(GameTime gameTime) {
        //exit if the escape key was pressed
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //calculate timings
        gameTime.ElapsedGameTime *= Time.timeScale; //calculate the elapsed time using the timescale
        Time.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; //calculate the deltaTime

        //load the qued gameObjects
        LoadQue();

        //call Update() on all GameObjects
        UpdateGameObjects(EventType.UPDATE, _loadedGameObjects, gameTime);

        base.Update(gameTime);
    }

    //draws every frame
    protected override void Draw(GameTime gameTime) {
        //calculate timings
        gameTime.ElapsedGameTime *= Time.timeScale; //calculate the elapsed time using the timescale
        Time.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; //calculate the deltaTime

        LoadQue();

        //clear the screen
        GraphicsDevice.Clear(new Color(0.16f, 0.150f, 0.165f));

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //call the Draw() functions of the gameObjects
        UpdateGameObjects(EventType.DRAW, _loadedGameObjects, gameTime, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
    #endregion //monogame events

    //loads the gameObjects which have been qued up
    private void LoadQue() {
        //don't bother if there is nothing in the que
        if (_loadGameObjectQue.Count == 0) {
            return;
        }

        //initialize components that may have been added between updates
        UpdateGameObjects(EventType.INITIALIZE, _loadGameObjectQue);
        UpdateGameObjects(EventType.LOADCONENT, _loadGameObjectQue);
        UpdateGameObjects(EventType.LOAD, _loadGameObjectQue);
        _loadGameObjectQue.ForEach((obj) => _loadedGameObjects.Add(obj)); //add the gameObjects to the loaded list
        _loadGameObjectQue.Clear(); //clear the que list
    }

    /// <summary>
    /// calls the <paramref name="eventType"/> on each component in <paramref name="gameObjects"/> asynchronously. Blocks thread until all Components have been Updated.
    /// </summary>
    private static void UpdateGameObjects(EventType eventType, IReadOnlyCollection<GameObject> gameObjects, params object[] args) {
        #region event handling
        void InitializeComponent(Component comp) {
            if (comp.initialized == false && comp is IInitialize obj) {
                obj.Initialize();
                comp.initialized = true;
            }
        }

        void LoadContentComponent(Component comp) {
            if (comp.contentLoaded == false && comp is ILoadContent obj) {
                obj.LoadContent();
                comp.contentLoaded = true;
            }
        }

        void LoadComponent(Component comp) {
            if (comp.loaded == false && comp is ILoad obj) {
                obj.Load();
                comp.loaded = true;
            }
        }

        void UpdateComponent(Component comp, GameTime gameTime) {
            if (comp is IUpdate obj) {
                obj.Update(gameTime);
            }
        }

        void DrawComponent(Component comp, GameTime gameTime, SpriteBatch spriteBatch) {
            if (comp is IDraw obj) {
                obj.Draw(gameTime, spriteBatch);
            }
        }
        #endregion //event handling

        List<Task> callEvents = new(); //list of the tasks which is calling the methods within the gameObjects
        //loop through the gameObjects within the game
        for (int i = 0; i < gameObjects.Count; i++) {
            GameObject gameObject = gameObjects.ElementAt(i);

            if (gameObject == null) {
                continue; //BUG: idk why this check is even nessecary, but at some point a gameObject == null
            }

            IReadOnlyCollection<Component> components = gameObject.GetComponents(); //get the components from the gameObject
            //loop through the components in the gameObject
            for (int j = 0; j < components.Count; j++) {

                Component component = components.ElementAt(j); //get the element at j and store it's referenced
                if (component == null) {
                    continue;
                }

                Task task = Task.Run(eventType switch {
                    EventType.INITIALIZE => () => InitializeComponent(component),
                    EventType.LOADCONENT => () => LoadContentComponent(component),
                    EventType.LOAD => () => LoadComponent(component),
                    EventType.UPDATE => () => UpdateComponent(component, (GameTime)args[0]),
                    EventType.DRAW => () => DrawComponent(component, (GameTime)args[0], (SpriteBatch)args[1]),
                    _ => throw new NotImplementedException(),
                });

                callEvents.Add(task); //store the task in the list
            }
        }

        //await all the tasks being done
        Task.WhenAll(callEvents).Wait();
    }
}
