using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using HordeSurvival.Core;

namespace HordeSurvival.Android;

/// <summary>
/// The main activity for the Android application. It initializes the game instance,
/// sets up the rendering view, and starts the game loop.
/// </summary>
[Activity(
    Label = "HordeSurvival",
    MainLauncher = true,
    Icon = "@drawable/icon",
    Theme = "@style/Theme.Splash",
    AlwaysRetainTaskState = true,
    LaunchMode = LaunchMode.SingleInstance,
    ScreenOrientation = ScreenOrientation.SensorLandscape,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden
)]
public class MainActivity : AndroidGameActivity
{
    private HordeSurvivalGame _game;
    private View _view;

    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        _game = new HordeSurvivalGame();
        _view = _game.Services.GetService(typeof(View)) as View;

        SetContentView(_view);
        _game.Run();
    }
}
