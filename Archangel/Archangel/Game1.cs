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
    // T 4/3/15- changed all relevant classes to implement an inactive bullet queue, fixed draw issues, fixed collision issues, fixed more issues, removed floatscale
    // T 4/7/15- added sword code to update
    // B 4/14/15 - created the platform object and increased charcter speed
    // B 4/17/15 - added the file for the platform sprite
    // T 4/22/15- renamed skyplayer to player and flyingsprites to playersprites, fixed the clientbounds issue
    // T 4/24/15- added enemy collisions with platforms
    // T 4/28/15- fixed bounds issues (again), added fullscreen
    // T 5/4/15- added dash
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D platformSprite; // platform sprite
        Texture2D[] enemySprites; // Texture2D arrays and variables to pass into the methods for drawing of each object
        Texture2D[] playerSprites;
        Texture2D[] playerSmallBullet;
        Texture2D[] enemySmallBullet;
        public static int clientWidth; // Lets other methods know window bounds
        public static int clientHeight;
        Player player; // Player and enemies and hud and map reader
        List<Enemy> enemies;
        HeadsUpDisplay hud;
        SpriteFont mainfont;
        Encounters encounter;
        int encounterDelay;


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
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width; // 1800
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height; // 1000
            graphics.IsFullScreen = true; //I want to make it run full screen
            graphics.ApplyChanges();

            clientWidth = graphics.GraphicsDevice.DisplayMode.Width ; // Lets other methods know window bounds
            clientHeight = graphics.GraphicsDevice.DisplayMode.Height;
            //platformSprite
            enemySprites = new Texture2D[9];
            playerSprites = new Texture2D[18]; // Initialize arrays
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
            platformSprite = Content.Load<Texture2D>("Platform");
            playerSprites[17] = Content.Load<Texture2D>("Dash"); // Stick dash on the endo of the player's sprite array to save space and code

            for (int i = 0; i < 17; i++) // For loop poulates entire arrays with 1 sprite for testing purposes
            {
                if (i < 4)
                {
                    playerSprites[i] = Content.Load<Texture2D>("Main Character Pose 1"); // Right sprites
                    enemySprites[i] = Content.Load<Texture2D>("Enemy Pose 1"); // Left Sprites
                    playerSmallBullet[i] = Content.Load<Texture2D>("Player Bullet 1");
                    enemySmallBullet[i] = Content.Load<Texture2D>("Enemy Bullet 1");
                }
                else if (i > 3 && i < 6)
                {
                    playerSprites[i] = Content.Load<Texture2D>("Main Character Pose 4"); // Up sprites
                    enemySprites[i] = Content.Load<Texture2D>("Enemy Pose 4");
                }
                else if (i > 5 && i < 8)
                {
                    playerSprites[i] = Content.Load<Texture2D>("Main Character Pose 3"); // Down sprites
                    enemySprites[i] = Content.Load<Texture2D>("Enemy Pose 3");
                }
                else if (i == 8)
                {
                    playerSprites[i] = Content.Load<Texture2D>("Player Hitspark");
                    enemySprites[i] = Content.Load<Texture2D>("Enemy Hitspark");
                }
                else if (i > 8 && i < 11)
                {
                    playerSprites[i] = Content.Load<Texture2D>("Main Character SlashR");
                }
                else if (i > 10 && i < 13)
                {
                    playerSprites[i] = Content.Load<Texture2D>("Main Character Pose SlashD");
                }
                else if (i > 12)
                {
                    playerSprites[i] = Content.Load<Texture2D>("Main Character Ground");
                }
            }
            player = new Player(20, 500, 0, 8, playerSprites, playerSmallBullet);
            hud = new HeadsUpDisplay();
            hud.Thought = 1; // Skye's first thoughts/lines
            hud.SkyeThink();
            hud.SkyeTalk();
            encounter = new Encounters(player);
            encounter.ReadEncounter(enemySprites, enemySmallBullet, hud);
            enemies = encounter.enemies; // Populate the enemy list
            player.Platform = encounter.PlatformSpawn(platformSprite);
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

            if (player.lives < 0)
            {
                Environment.Exit(69);
            }
            if (enemies.Count <= 0)
            {
                encounter.ReadEncounter(enemySprites, enemySmallBullet, hud);
                enemies = encounter.enemies; // Populate the enemy list
                
            }
            if (enemies.Count > 0)
            {
                try
                {
                    player.Update(); // Update the player
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
                    if (enemies[i].deathTimer >= 20)
                    {
                        enemies.RemoveAt(i); // Remove the enemy from the game after their death has been viewed
                    }
                }

                player.Platform.Update();

                // Collision detection
                for (int i = 0; i < enemies.Count; i++) // For each enemy
                {
                    if (enemies[i].spritePos.Intersects(player.Platform.spritePos))
                    {
                        switch (enemies[i].direction)
                        {
                            case 1:
                                enemies[i].spritePos = new Rectangle(player.Platform.spritePos.Left - enemies[i].spritePos.Width, enemies[i].spritePos.Y, enemies[i].spritePos.Width, enemies[i].spritePos.Height); // Collides with left side
                                break;
                            case 3:
                                enemies[i].spritePos = new Rectangle(player.Platform.spritePos.Right, enemies[i].spritePos.Y, enemies[i].spritePos.Width, enemies[i].spritePos.Height); // Collides with right side
                                break;
                            case 5:
                                enemies[i].spritePos = new Rectangle(enemies[i].spritePos.X, player.Platform.spritePos.Bottom, enemies[i].spritePos.Width, enemies[i].spritePos.Height); // Collides with bottom
                                break;
                            default: // Catches both moveDown and falling
                                enemies[i].spritePos = new Rectangle(enemies[i].spritePos.X, player.Platform.spritePos.Top - enemies[i].spritePos.Height, enemies[i].spritePos.Width, enemies[i].spritePos.Height); // Collides with top
                                break;
                        }
                    }
                    for (int z = 0; z < player.bullets.Length; z++) // For each player bullet
                    {
                        if (player.bullets[z].isActive && enemies[i].spritePos.Intersects(player.bullets[z].spritePos)) // If the bullet is active
                        {
                            enemies[i].TakeHit(player.bullets[z].damage); //If the bullet is active and the enemy and bullet intersect, take a hit and kill the bullet
                            player.bullets[z].isActive = false;
                            player.ReloadBullet(z); // Add the bullet back to the inactive queue
                        }
                    }

                    for (int z = 0; z < enemies[i].bullets.Length; z++) // For each enemy bullet
                    {
                        if (player.direction > 8 && player.direction < 13)
                        {
                            if (enemies[i].bullets[z].isActive && enemies[i].bullets[z].spritePos.Intersects(player.swordBox))
                            {
                                enemies[i].bullets[z].Reflect(); // Reflect the bullet if the sword hitbox is up and the bullet hits it
                            }
                        }
                        else if (enemies[i].bullets[z].isActive && player.spritePos.Intersects(enemies[i].bullets[z].spritePos) && player.direction != 8) // Don't hurt the player if it hits the sword
                        {
                            player.TakeHit(enemies[i].bullets[z].damage); // If the bullet is active and the player and bullet intersect, take a hit and kill the bullet
                            enemies[i].bullets[z].isActive = false;
                            enemies[i].ReloadBullet(z); // Add the bullet back to the inactive queue
                        }
                        // check to see if a reflected bullet hits the enemy
                        if (enemies[i].bullets[z].isActive && enemies[i].bullets[z].Reflected == true && enemies[i].spritePos.Intersects(enemies[i].bullets[z].spritePos)) // Don't hurt the player if it hits the sword
                        {
                            enemies[i].TakeHit(enemies[i].bullets[z].damage); // If the bullet is active and the player and bullet intersect, take a hit and kill the bullet
                            enemies[i].bullets[z].isActive = false;
                            enemies[i].bullets[z].Reflected = false;
                            enemies[i].ReloadBullet(z); // Add the bullet back to the inactive queue
                        }
                    }

                    if (player.direction > 8) // If the sword is active, check the hitbox
                    {
                        if (enemies[i].spritePos.Intersects(player.swordBox) && player.slashFrames == 2) // Only hit once. Pick a frame.
                        {
                            enemies[i].TakeHit(player.swordDamage); // Sword damage
                            if (enemies[i].Blessing == 3)
                            {
                                enemies[i].Shielded = false; // deactivates sheield
                                enemies[i].BlessingColor = Color.White;
                            }
                        }
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

            player.Platform.Draw(spriteBatch);
            //spriteBatch.Draw(platformSprite, new Rectangle(800, 700, 512, 128), Color.White);

            //spriteBatch.DrawString(mainfont, player.dash.dashFrame.ToString() + "," + player.dashActive.ToString(), new Vector2(500, 500), Color.Blue);
            player.Draw(spriteBatch); // Draw player
            for (int i = 0; i < enemies.Count; i++) // Draw enemies
            {
                enemies[i].Draw(spriteBatch); // NOTE: bullet draws are in the draw method for the character class
            }

            hud.DrawHUD(spriteBatch, mainfont, player);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
