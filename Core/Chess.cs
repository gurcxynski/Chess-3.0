using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Chess.Core;

public class Chess : Game
{
    internal static DisplayMode displaySettings;
    readonly internal GraphicsDeviceManager graphics;
    internal static MonoGame.Extended.Input.InputListeners.KeyboardListener keyboardListener = new();

    internal SoundEffect moveSound;
    internal SoundEffect captureSound;

    SpriteBatch spriteBatch;
    static internal Chess Instance { get; private set; }
    public Chess()
    {
        Instance = this;
        graphics = new(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        var mode = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last();
        // displaySettings = (new(mode.Width, mode.Height, true, true));
        displaySettings = (new(1200, 800, false, false));
        ApplyDisplaySettings();
        Window.ClientSizeChanged += (object sender, EventArgs e) => { if (IsActive) ApplyDisplaySettings(); };
    }
    internal void ApplyDisplaySettings()
    {
        graphics.PreferredBackBufferWidth = displaySettings.Width;
        graphics.PreferredBackBufferHeight = displaySettings.Height;
        graphics.IsFullScreen = displaySettings.Fullscreen;
        Window.IsBorderless = displaySettings.Borderless;
        graphics.ApplyChanges();

        if (displaySettings.Borderless) CenterWindow();
    }
    private void CenterWindow()
    {
        var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        int screenWidth = displayMode.Width;
        int screenHeight = displayMode.Height;

        int windowWidth = graphics.PreferredBackBufferWidth;
        int windowHeight = graphics.PreferredBackBufferHeight;

        int windowX = (screenWidth - windowWidth) / 2;
        int windowY = (screenHeight - windowHeight) / 2;

        Window.Position = new Point(windowX, windowY);
    }

    protected override void Initialize()
    {
        UserInterface.Initialize(Content, "chess");

        StateMachine.Start();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice); 
        moveSound = Content.Load<SoundEffect>("move");
        captureSound = Content.Load<SoundEffect>("capture");

    }

    protected override void Update(GameTime gameTime)
    {
        UserInterface.Active.Update(gameTime);
        keyboardListener.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        UserInterface.Active.Draw(spriteBatch);

        base.Draw(gameTime);
    }
}