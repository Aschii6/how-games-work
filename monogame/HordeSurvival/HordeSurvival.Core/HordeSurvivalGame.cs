using System;
using HordeSurvival.Core.Components;
using HordeSurvival.Core.Entities;
using HordeSurvival.Core.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HordeSurvival.Core;

/// <summary>
/// The main class for the game, responsible for managing game components, settings, and platform-specific configurations.
/// </summary>
public class HordeSurvivalGame : Game
{
    // Resources for drawing.
    private GraphicsDeviceManager _graphics;

    public static readonly bool IsMobile = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS();

    public static readonly bool IsDesktop =
        OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

    private SpriteBatch _spriteBatch;
    private Entity _player;

    /// <summary>
    /// Initializes a new instance of the game. Configures platform-specific settings.
    /// </summary>
    public HordeSurvivalGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;

        // Share GraphicsDeviceManager as a service.
        Services.AddService(_graphics);

        Content.RootDirectory = "Content";

        _graphics.SupportedOrientations =
            DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
    }

    /// <summary>
    /// Initializes the game.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        // You should load this based on what the user or operating system selected.
        const string selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
        LocalizationManager.SetCulture(selectedLanguage);
    }

    /// <summary>
    /// Loads game content, such as textures and particle systems.
    /// </summary>
    protected override void LoadContent()
    {
        base.LoadContent();

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var playerTexture = Content.Load<Texture2D>("Sprites/Warrior/Warrior_Idle");

        _player = new Entity();
        _player.AddComponent(new TransformComponent());
        _player.AddComponent(new PlayerInputComponent());
        _player.AddComponent(new MovementComponent());
        _player.AddComponent(new SpriteComponent
        {
            Texture = playerTexture,
            SourceRectangle = new Rectangle(0, 0, 192, 192)
        });
    }

    /// <summary>
    /// Updates the game's logic, called once per frame.
    /// </summary>
    /// <param name="gameTime">
    /// Provides a snapshot of timing values used for game updates.
    /// </param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _player.Update(gameTime);

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the game's graphics, called once per frame.
    /// </summary>
    /// <param name="gameTime">
    /// Provides a snapshot of timing values used for rendering.
    /// </param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSlateGray);

        _spriteBatch.Begin();

        _player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
