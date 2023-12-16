#nullable enable
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooter.Source.Core.Data;
internal struct GameEvents {
    public Action? init;
    public Action<ContentManager>? load;
    public Action<GameTime>? update;
    public Action<SpriteBatch>? draw;
}