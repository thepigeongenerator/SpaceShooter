using SpaceShooter.Source.Core.Components;
using System.Collections.Generic;

namespace SpaceShooter.Source.Core;
internal class ColliderManager {
    private static ColliderManager _instance;
    private IEnumerable<Collider> _colliders;

    //private constructor for singleton
    private ColliderManager() {
        GameManager game = GameManager.Instance;
        game.GameObjectsChanged += UpdateList;
        UpdateList();
    }

    public static ColliderManager Instance {
        get {
            _instance ??= new ColliderManager();
            return _instance;
        }
    }

    public IEnumerable<Collider> Colliders {
        get => _colliders;
    }

    private void UpdateList() {
        GameManager game = GameManager.Instance;
        _colliders = game.FindObjectsOfType<Collider>();
    }
}
