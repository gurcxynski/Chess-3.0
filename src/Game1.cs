using Chess.Core;
using Chess.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private readonly Scene scene = new();
    internal readonly Textures textures = new();
    internal static Game1 self;
    public static readonly int Size = 100;
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        self = this;
        graphics.PreferredBackBufferWidth = 8 * Size;
        graphics.PreferredBackBufferHeight = 8 * Size;
    }

    protected override void Initialize()
    {

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        textures.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        scene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();
        scene.Draw(spriteBatch);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
