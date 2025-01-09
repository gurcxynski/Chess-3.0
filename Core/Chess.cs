using Chess.Core.Util;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Core;
public partial class Chess : Game
{
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public Chess()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        graphics.PreferredBackBufferWidth = 800;
        graphics.PreferredBackBufferHeight = 800;
    }

    protected override void Initialize()
    {
        UserInterface.Initialize(Content, "chess");
        IsMouseVisible = true;

        StateMachine.Init();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Textures.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        UserInterface.Active.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        UserInterface.Active.Draw(spriteBatch);

        base.Draw(gameTime);
    }
}