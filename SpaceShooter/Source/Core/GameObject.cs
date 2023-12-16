#nullable enable
using Source.Core;
using SpaceShooter.Source.Core.Components;
using SpaceShooter.Source.Core.ScriptComponent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter.Source.Core;
internal class GameObject : IDisposable {
    private readonly List<Component> _components;
    private readonly Transform _transform;
    private bool _disposed = false;

    public GameObject() {
        _components = new List<Component>();
        _transform = AddComponent<Transform>();
        GameManager.Instance.AddGameObject(this);
    }

    public bool Disposed {
        get => _disposed;
    }

    public Transform Transform {
        get => _transform;
    }

    public T AddComponent<T>() where T : Component {
        GameManager game = GameManager.Instance;
        T component = Activator.CreateInstance<T>();
        component.GameObject = this;

        if (game.Initialized && component is IInitialize initialize) {
            initialize.Initialize();
        }

        if (game.ContentLoaded && component is ILoadContent loadContent) {
            loadContent.LoadContent();
        }

        if (game.ContentLoaded && component is ILoad load) {
            load.Load();
        }

        _components.Add(component);
        return component;
    }

    public void RemoveComponent(Component component) {
        //if the component isn't disposed
        if (component.Disposed == false) {
            //dispose the component and exit, since the component calls this itself
            component.Dispose();
            return;
        }

        _components.Remove(component);
    }

    public T? GetComponent<T>() where T : Component {
        return GetComponents<T>()?.FirstOrDefault();
    }

    #region component finding
    public IEnumerable<T>? GetComponents<T>() where T : Component {
        return
            from component in _components
            where component is T
            select component as T;
    }

    public IReadOnlyCollection<Component> GetComponents() {
        return _components;
    }
    #endregion //component finding

    public void Dispose() {
        _disposed = true;
        foreach (Component component in _components) {
            component.Dispose();
        }

        GameManager.Instance.DisposeGameObject(this);
    }
}
