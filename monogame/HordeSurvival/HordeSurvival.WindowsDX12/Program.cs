using HordeSurvival.Core;

internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new HordeSurvivalGame();
        game.Run();
    }
}
