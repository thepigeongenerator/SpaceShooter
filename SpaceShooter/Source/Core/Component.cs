#nullable enable

using SpaceShooter.Source.Core.Components;
using System;

namespace SpaceShooter.Source.Core;
internal abstract class Component : IDisposable {
    private bool _disposed = false;
    private GameObject? _gameObject;

    public bool Disposed {
        get => _disposed;
    }

    public GameObject GameObject {
        get {
            if (_gameObject == null) {
                throw new NullReferenceException($"'{nameof(_gameObject)}' has yet to be assigned a value, try getting this value later.");
            }

            return _gameObject;
        }
        set {
            if (_gameObject != null) {
                throw new InvalidOperationException($"{nameof(_gameObject)} was already assigned a value; a value can only be assigned once.");
            }

            _gameObject = value;
        }
    }

    public Transform Transform {
        get => GameObject.Transform;
    }

    public T AddComponent<T>() where T : Component {
        return GameObject.AddComponent<T>();
    }

    public T? GetComponent<T>() where T : Component {
        return GameObject.GetComponent<T>();
    }

    public void Dispose() {
        _disposed = false;
        GameObject.RemoveComponent(this);
    }
}
