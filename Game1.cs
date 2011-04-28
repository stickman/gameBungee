using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace gameBungee
{

    static public class EnvironmentVariable
    {
        static public GraphicsDeviceManager graphics;
        static public int width;
        static public int height;
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteBatch spriteBatch;

        private Level level;
        private SamplerState _clampTextureAddressMode;

        public Game1()
        {
            EnvironmentVariable.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            EnvironmentVariable.graphics.IsFullScreen = false;
            //this.graphics.PreferredBackBufferWidth = 800;
            //this.graphics.PreferredBackBufferHeight = 480;
            EnvironmentVariable.graphics.ApplyChanges();
         
            this.Window.Title = "Premier programme";
            this.Window.AllowUserResizing = false;

            level = new Level(Services, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            base.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(EnvironmentVariable.graphics.GraphicsDevice);

            EnvironmentVariable.width = GraphicsDevice.DisplayMode.Width;
            EnvironmentVariable.height= GraphicsDevice.DisplayMode.Height;

            CameraController.Inialized(new Rectangle(EnvironmentVariable.width / 4, EnvironmentVariable.height / 4, EnvironmentVariable.width / 2, EnvironmentVariable.height / 2), EnvironmentVariable.graphics.GraphicsDevice);

            _clampTextureAddressMode = new SamplerState
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp
            };

            level.LoadContent(EnvironmentVariable.graphics.GraphicsDevice, Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            level.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            EnvironmentVariable.graphics.GraphicsDevice.SamplerStates[0] = _clampTextureAddressMode; 

            level.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
