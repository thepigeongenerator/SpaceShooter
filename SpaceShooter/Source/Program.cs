using SpaceShooter.Source.Core;

namespace SpaceShooter.Source;

internal class Program {
    //entry point
    private static void Main() {
        //run the game
        using var game = GameManager.Instance;
        game.Run();
    }
}