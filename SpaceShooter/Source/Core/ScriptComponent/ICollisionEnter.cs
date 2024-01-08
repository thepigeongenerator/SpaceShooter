using SpaceShooter.Source.Core.Components;

namespace SpaceShooter.Source.Core.ScriptComponent;
internal interface ICollisionEnter {
    /// <summary>
    /// called when this component's collider enters the collider of another component
    /// </summary>
    public abstract void CollisionEnter(Collider collider);
}
