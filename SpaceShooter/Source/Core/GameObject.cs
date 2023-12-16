using Microsoft.Xna.Framework;
using Source.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Source.Core;
internal sealed class GameObject {
    public readonly Transform transform;
    private readonly List<Component> _components = new();

    public GameObject() {
        transform = new Transform();
        transform.position = Vector2.Zero;
        transform.rotation = 0f;
        transform.scale = Vector2.One;
    }

    public T AddComponent<T>() where T : Component {
        T component = Activator.CreateInstance<T>();
        component.gameObject = this; //set the gameObject to this
        _components.Add(component); //add the component to the component list
        return component;
    }

    public T GetComponent<T>() where T : Component {
        return (
            from component in _components
            where component is T
            select component as T).FirstOrDefault();
    }

    public IReadOnlyCollection<Component> GetComponents() {
        return _components;
    }
}
