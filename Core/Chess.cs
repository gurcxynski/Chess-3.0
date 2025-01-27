using Chess.Core.UI.Menus;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using MonoGame.Extended.Input.InputListeners;
namespace Chess.Core;
public class Chess : Game
{
    internal static DisplayMode currentMode;
    readonly internal GraphicsDeviceManager graphics;
    internal static KeyboardListener keyboardListener;

    SpriteBatch spriteBatch;
    static internal Chess Instance { get; private set; }
    public Chess()
    {
        Instance = this;
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        keyboardListener = new KeyboardListener();

        var mode = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last();
        currentMode = (new(mode.Width, mode.Height, true, true));
        ApplyDisplayModeChanges();
    }
    internal void ApplyDisplayModeChanges()
    {
        graphics.PreferredBackBufferWidth = currentMode.Width;
        graphics.PreferredBackBufferHeight = currentMode.Height;
        graphics.IsFullScreen = currentMode.Fullscreen;
        Window.IsBorderless = currentMode.Borderless;
        graphics.ApplyChanges();

        CenterWindow();
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

        StateMachine.ToMenu<StartMenu>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
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