using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Source.Core.Data;
using System;
using System.Reflection;

namespace Source.Core.Components;
internal abstract class ScriptComponent : Component {
    public GameEvents gameEvents = new();

    public Transform Transform => gameObject.transform;

    public ScriptComponent() {
        gameEvents.init = IsDefined(nameof(Init)) ? Init : null;
        gameEvents.load = IsDefined(nameof(Load)) ? Load : null;
        gameEvents.update = IsDefined(nameof(Update)) ? Update : null;
        gameEvents.draw = IsDefined(nameof(Draw)) ? Draw : null;

        bool IsDefined(string methodName) {
            MethodInfo methodInfo = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic); //get the method with the given methodName
            Type declaringType = methodInfo.DeclaringType;

            return declaringType != typeof(ScriptComponent);
        }
    }

    protected virtual void Init() => throw new NotImplementedException();
    protected virtual void Load(ContentManager content) => throw new NotImplementedException();
    protected virtual void Update(GameTime gameTime) => throw new NotImplementedException();
    protected virtual void Draw(SpriteBatch spriteBatch) => throw new NotImplementedException();
}
