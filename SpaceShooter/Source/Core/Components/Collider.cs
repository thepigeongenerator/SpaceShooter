using Microsoft.Xna.Framework;
using SpaceShooter.Source.Core.ScriptComponent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter.Source.Core.Components;
internal abstract class Collider : Component, IUpdate {
    public void Update(GameTime gameTime) {
        ColliderManager collisionManager = ColliderManager.Instance;
        for (int i = 0; i < collisionManager.Colliders.Count(); i++) {
            Collider collider = collisionManager.Colliders.ElementAt(i);
            if (collider == this) {
                continue;
            }

            if (InCollider(collider.GetContactPoint(Transform.position))) {
                CollisionActivator(GameObject, collider);
                CollisionActivator(collider.GameObject, this);
            }
        }
    }

    private static void CollisionActivator(GameObject gameObject, Collider collider) {
        IReadOnlyCollection<Component> components = gameObject.GetComponents();
        for (int i = 0; i < components.Count; i++) {
            Component component = components.ElementAt(i);
            if (component is ICollisionEnter collisionEnter) {
                collisionEnter.CollisionEnter(collider);
            }
        }
    }

    /// <summary>
    /// used to get a global-scoped vector for a point on the collider which is nearest to <paramref name="position"/>
    /// </summary>
    public abstract Vector2 GetContactPoint(Vector2 position);

    /// <summary>
    /// used to check whether a position is within the collider
    /// </summary>
    public abstract bool InCollider(Vector2 position);

    public override void Dispose() {
    }
}
