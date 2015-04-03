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
    // T 4/2/15- added collision detection, drawing, and updates; removed GameLogic class, added loop to populate sprite arrays for testing
    // B 4/2/15 - Added code to populate the sprite arrays, intantiated enemyList and sprite arrays, created an Encounters object to load an encounter
                     // T (B+T) 4/2/15- Fixed Merge issues
    // T 4/3/15- changed all relevant classes to implement an inactive bullet queue, fixed draw issues, fixed collision issues
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D[] enemySprites; // Texture2D arrays and variables to pass into the methods for drawing of each object
        Texture2D[] flyingPlayerSprites;
        Texture2D[] playerSmallBullet;
        Texture2D[] enemySmallBullet;
        public static int clientWidth; // Lets other methods know window bounds
        public static int clientHeight;
        SkyPlayer skyPlayer; // Player and enemies
        List<Enemy> enemies;
        HeadsUpDisplay hud;
        SpriteFont mainfont;
        Encounters encounter;

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
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();

            clientWidth = graphics.PreferredBackBufferWidth; // Lets other methods know window bounds
            clientHeight = graphics.PreferredBackBufferHeight;
            enemySprites = new Texture2D[9];
            flyingPlayerSprites = new Texture2D[9]; // Initialize arrays
            playerSmallBullet = new Texture2D[4];
            enemySmallBullet = new Texture2D[4];
            
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

            for (int i = 0; i < 9; i++) // For loop poulates entire arrays with 1 sprite for testing purposes
            {
                enemySprites[i] = Content.Load<Texture2D>("Enemy Pose 1");
                flyingPlayerSprites[i] = Content.Load<Texture2D>("Main Character Pose 1");
                if (i < 4)
                {
                    playerSmallBullet[i] = Content.Load<Texture2D>("Player Bullet 1");
                    enemySmallBullet[i] = Content.Load<Texture2D>("Enemy Bullet 1");
                }
            }
            skyPlayer = new SkyPlayer(20, 20, 0, 3, flyingPlayerSprites, playerSmallBullet);
            hud = new HeadsUpDisplay();
            encounter = new Encounters();
            
            
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

            encounter.ReadEncounter(enemySprites, enemySmallBullet, hud);
            enemies = encounter.enemies; // Populate the enemy list
            try
            {
                skyPlayer.Update(); // Update the player
            }
            catch (IndexOutOfRangeException noBullets) // Catch the throw up if no active bullets are found on a fire attempt
            {
                hud.Skyesays = "You're out of bullets! Wait for them to replenish before trying to fire.";
            }

            for (int i = 0; i < enemies.Count; i++) // Update all the enemies
            {
                try
                {
                    enemies[i].Update(); // NOTE: bullet updates are in the update method in the Character class
                }
                catch (IndexOutOfRangeException noBullets) 
                {
                    hud.Skyesays = "An enemy is out of bullets!"; // Catch the throw up if no active bullets are found on a fire attempt
                }
                if (enemies[i].deathTimer >= 5)
                {
                    enemies.RemoveAt(i); // Remove the enemy from the game after their death has been viewed
                }
            }

            // Collision detection
            for (int i = 0; i < enemies.Count; i++) // For each enemy
            {
                for (int z = 0; z < skyPlayer.bullets.Length; z++) // For each player bullet
                {
                    if (skyPlayer.bullets[z].isActive && enemies[i].spritePos.Intersects(skyPlayer.bullets[z].spritePos)) // If the bullet is active
                    {
                        enemies[i].TakeHit(skyPlayer.bullets[z].damage); //If the bullet is active and the enemy and bullet intersect, take a hit and kill the bullet
                        skyPlayer.bullets[z].isActive = false;
                        skyPlayer.ReloadBullet(z); // Add the bullet back to the inactive queue
                    }
                }

                for (int z = 0; z < enemies[i].bullets.Length; z++) // For each enemy bullet
                {
                    if (enemies[i].bullets[z].isActive && skyPlayer.spritePos.Intersects(enemies[i].bullets[z].spritePos)) // 
                    {
                        skyPlayer.TakeHit(enemies[i].bullets[z].damage); // If the bullet is active and the player and bullet intersect, take a hit and kill the bullet
                        enemies[i].bullets[z].isActive = false;
                        enemies[i].ReloadBullet(z); // Add the bullet back to the inactive queue
                    }
                }
            }
            // End Collision detection
            
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

            hud.DrawHUD(spriteBatch, mainfont, skyPlayer);
            //spriteBatch.Draw(player, playerPos, new Rectangle(0,20,player.Width, player.Height), Color.White, 0, new Vector2(), floatScale, SpriteEffects.None, 0);
            int z;
            skyPlayer.Draw(spriteBatch); // Draw player
            for (int i = 0; i < 50; i++)
            {
                z = 10+(50*i);
                spriteBatch.DrawString(mainfont, skyPlayer.bullets[i].direction.ToString(), new Vector2(z,40), Color.Blue);
                spriteBatch.DrawString(mainfont, skyPlayer.bullets[i].isActive.ToString(), new Vector2(z, 70), Color.Blue);
                spriteBatch.DrawString(mainfont, skyPlayer.bulletQueue[i].ToString(), new Vector2(z, 100), Color.Blue);
            }
                for (int i = 0; i < enemies.Count; i++) // Draw enemies
                {
                    enemies[i].Draw(spriteBatch); // NOTE: bullet draws are in the draw method for the character class
                }
            spriteBatch.DrawString(mainfont, encounter.enemies.Count.ToString(), new Vector2(300, 300), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
