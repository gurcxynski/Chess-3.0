using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// using GeonBit UI elements
using GeonBit.UI;
using Chess.UI;
using Chess.Util;

namespace Chess;
    /// This is the main class for your game.
    public class Chess : Game
    {
        // graphics and spritebatch
        GraphicsDeviceManager graphics;
        internal readonly StateMachine machine = new();
        SpriteBatch spriteBatch;

		/// Game constructor.
        public Chess()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// Allows the game to perform any initialization it needs to before starting to run.
        /// here we create and init the UI manager.
        protected override void Initialize()
        {
            // GeonBit.UI: Init the UI manager using the "hd" built-in theme
            UserInterface.Initialize(Content, "chess");

            // GeonBit.UI: tbd create your GUI layouts here..
            
            UserInterface.Active.AddEntity(new NewGameMenu());
            
            // call base initialize func
            base.Initialize();
        }

        /// LoadContent will be called once per game and is the place to load.
        /// here we init the spriteBatch (this is code you probably already have).
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Load(this);
        }

        /// Allows the game to run logic such as updating the world.
        /// here we call the UI manager update() function to update the UI.
        protected override void Update(GameTime gameTime)
        {
            // GeonBit.UIL update UI manager
            UserInterface.Active.Update(gameTime);

            // tbd add your own update() stuff here..

            // call base update
            base.Update(gameTime);
        }

        /// This is called when the game should draw itself.
        /// here we call the UI manager draw() function to render the UI.
        protected override void Draw(GameTime gameTime)
        {
            // clear buffer
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // GeonBit.UI: draw UI using the spriteBatch you created above
            UserInterface.Active.Draw(spriteBatch);

            // call base draw function
            base.Draw(gameTime);
        }
    }