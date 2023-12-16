#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using SpaceShooter.Source.Core.ScriptComponent;
using SpaceShooter.Source.Core.Components;
using Source.Core;

namespace SpaceShooter.Source.Core;
internal class GameObject {
    private List<Component> _components;
    private Transform _transform;

    public GameObject() {
        _components = new List<Component>();
        _transform = AddComponent<Transform>();
        GameManager.Instance.AddGameObject(this);
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

        if (game.Loaded && component is ILoad loadContent) {
            loadContent.Load();
        }

        _components.Add(component);
        return component;
    }

    public T? GetComponent<T>() where T : Component {
        return (
            from comp in _components
            where comp is T
            select comp as T
            ).FirstOrDefault();
    }

    public IReadOnlyCollection<Component> GetComponents() {
        return _components;
    }
}
