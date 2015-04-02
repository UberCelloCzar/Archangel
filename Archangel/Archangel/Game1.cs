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
    // B 4/1/15 - Added basic sprites to content folder, updated draw method and SpriteBatch
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
        Texture2D player;
        Texture2D enemy;
        Texture2D playerBullet;
        Texture2D enemyBullet;
        static double doubleScale = 0.25;
        float floatScale = (float)doubleScale;
        KeyboardState kState;
        Vector2 playerPos = new Vector2(0, 20);

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
            clientBounds = Window.ClientBounds; // Lets other methods know window bounds\
            enemySprites = new Texture2D[8];
            flyingPlayerSprites = new Texture2D[8];
            playerSmallBullet = new Texture2D[4];
            enemySmallBullet = new Texture2D[4];
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
            enemySprites[0] = Content.Load<Texture2D>("Enemy Pose 1");
            flyingPlayerSprites[0] = Content.Load<Texture2D>("Main Character Pose 1");
            playerSmallBullet[0] = Content.Load<Texture2D>("Player Bullet 1");
            enemySmallBullet[0] = Content.Load<Texture2D>("Enemy Bullet 1");
            skyPlayer = new SkyPlayer(90, 0, 0, 1, flyingPlayerSprites, playerSmallBullet);
            hud = new HeadsUpDisplay(skyPlayer);
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
            //spriteBatch.Draw(player, playerPos, new Rectangle(0,20,player.Width, player.Height), Color.White, 0, new Vector2(), floatScale, SpriteEffects.None, 0);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
