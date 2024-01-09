using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Source.Core.Utils;
/// <summary>
/// randomizer static class for consistent random behaviour around the progran
/// </summary>
internal static class Randomizer {
    private static Random _random;

    public static void CreateNew(int seed) => _random = new Random(seed);
    public static int Next() => _random.Next();
    public static int Next(int exclMax) => _random.Next(exclMax);
    public static int Next(int inclMin, int exclMax) => _random.Next(inclMin, exclMax);
    public static float NextFloat() => _random.NextSingle() * float.MaxValue;
    public static float NextFloat(float exclMax) => _random.NextSingle() * exclMax;
    public static float NextFloat(float inclMin, float exclMax) => (_random.NextSingle() * (exclMax - inclMin)) + inclMin;
    public static float NextSingle() => _random.NextSingle();
}
