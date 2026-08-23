using System;
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
    private Texture2D _playerTexture;
    private Vector2 _playerPosition;
    private Vector2 _playerVelocity;
    private const float Speed = 300f;

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

        _playerTexture = Content.Load<Texture2D>("Sprites/Warrior/Warrior_Idle");
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

        ProcessMovementInputs();

        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _playerPosition += _playerVelocity * dt;

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

        _spriteBatch.Draw(_playerTexture, _playerPosition, new Rectangle(0, 0, 192, 192), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void ProcessMovementInputs()
    {
        var playerDirection = Vector2.Zero;
        if (Keyboard.GetState().IsKeyDown(Keys.W))
            playerDirection.Y -= 1;
        if (Keyboard.GetState().IsKeyDown(Keys.S))
            playerDirection.Y += 1;

        if (Keyboard.GetState().IsKeyDown(Keys.A))
            playerDirection.X -= 1;
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            playerDirection.X += 1;

        if (playerDirection != Vector2.Zero)
        {
            playerDirection.Normalize();
        }

        _playerVelocity = playerDirection * Speed;
    }
}
