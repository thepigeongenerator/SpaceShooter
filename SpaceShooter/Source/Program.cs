using Source.Core;

internal class Program {
    private static void Main() {
        using var game = GameManager.Instance;
        game.Run();
    }
}