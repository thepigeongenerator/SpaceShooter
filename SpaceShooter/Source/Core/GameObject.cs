#nullable enable
using SpaceShooter.Source.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter.Source.Core;
internal class GameObject : IDisposable {
    private readonly List<Component> _components;   //holds components of the GameObject
    private readonly Transform _transform;          //holds the GameObject's position
    private bool _disposing = false;                //whether the GameObjects disposing process has started
    private bool _disposed = false;                 //whether the GameObject has been disposed

    public GameObject() {
        _components = new List<Component>();
        _transform = AddComponent<Transform>(); //add the transform component in
        GameManager.Instance.AddGameObject(this);
    }

    public bool Disposing {
        get => _disposing;
    }

    public bool Disposed {
        get => _disposed;
    }

    public Transform Transform {
        get => _transform;
    }

    //adds a component to the GameObject
    public T AddComponent<T>() where T : Component {
        T component = Activator.CreateInstance<T>();
        component.GameObject = this;

        //add the components to the GameObject
        _components.Add(component);
        return component;
    }

    //remove the component from the GameObject
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
    public IEnumerable<T> GetComponents<T>() where T : Component {
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
        if (_disposed || _disposing) {
            return;
        }

        _disposing = true;


        GameManager.Instance.DisposeGameObject(this);
    }

    public void FinalizeDispose() {
        if (_disposing == false) {
            throw new Exception($"The GameObject's dispose finalize has been called before {nameof(Dispose)}");
        }

        if (_disposed == true) {
            throw new Exception("The GameObject has already been disposed!");
        }

        _disposed = true;

        while (_components.Count > 0) {
            _components[0].Dispose();
        }
    }
}
