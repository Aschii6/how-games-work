using HordeSurvival.Core;
using Foundation;
using UIKit;

namespace HordeSurvival.Ios;

[Register("AppDelegate")]
internal class Program : UIApplicationDelegate
{
    private static HordeSurvivalGame _game;

    internal static void RunGame()
    {
        _game = new HordeSurvivalGame();
        _game.Run();
    }

    public override void FinishedLaunching(UIApplication app)
    {
        RunGame();
    }

    static void Main(string[] args)
    {
        UIApplication.Main(args, null, typeof(Program));
    }
}
