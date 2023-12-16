namespace SpaceShooter.Source.Core.ScriptComponent;
internal interface ILoadContent {
    /// <summary>
    /// called after initialization, used for loading with <see cref="Core.GameManager.Content.Load{T}(string)"/>
    /// </summary>
    public abstract void LoadContent();
}
