#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Archangel
{
    // Cheshire Games, Bierre, March 13, 2015
    // Main form for the game, functions similar to Game class in previous projects, base class from Monogame

    // General Notes:
    // - Not all properties may be necessary in final product, I just included full getsets just in case most places

    // Change Log (first initial+date- summary of changes) - Change logs start upon first upload March 18, 2015
    // T 3/25/15- added clientBounds variable for checking if something is offscreen, initialized it
    // T 3/28+30/15- added more texture variables
    // T 3/31/15- fixed all draw methods to accept the Game1 spriteBatch
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D[] enemySprites; // Texture2D arrays and variables to pass into the methods for drawing of each object
        Texture2D[] flyingPlayerSprites;
        Texture2D[] playerSmallBullet;
        Texture2D[] enemySmallBullet;
        public static Rectangle clientBounds; // Lets other methods know window bounds
        SkyPlayer skyPlayer; // Player and enemies
        List<Enemy> enemyList;
        HeadsUpDisplay hud;
        SpriteFont mainfont;
        GameLogic gameLogic; // Logic handler


        public Game1():base()
        {
            graphics = new GraphicsDeviceManager(this);
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
            clientBounds = Window.ClientBounds; // Lets other methods know window bounds
            skyPlayer = new SkyPlayer(90, 0, 0, 1, flyingPlayerSprites, playerSmallBullet);
            hud = new HeadsUpDisplay(skyPlayer);
            gameLogic = new GameLogic();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mainfont = Content.Load<SpriteFont>("mainFont");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Goldenrod);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            hud.DrawHUD(spriteBatch, mainfont);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
