using Source.Core;

internal class Program {
    private static void Main(string[] args) {
        using var game = new SpaceInvadersGame();
        game.Run();
    }
}