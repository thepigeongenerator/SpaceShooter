using SpaceShooter.Source.Core;

namespace SpaceShooter.Source;

internal class Program {
    private static void Main() {
        using var game = GameManager.Instance;
        game.Run();
    }
}