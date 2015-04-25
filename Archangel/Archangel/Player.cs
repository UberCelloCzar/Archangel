﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework; // NOTE: necessary to use stuff
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Threading;

namespace Archangel
{
    // Cheshire Games, Bierre, March 16, 2015
    // Contains necessary code and methods for the player object while flying through the air

    // Change Log
    // T 3/26/15- removed enumeration, changed initDir to an int
    // T 3/27/15- added draw and update code
    // T 3/29/15- added bullets to the draw, added fire method, initialized bullet array in the constructor
    // T 4/2/15- moved fire method to Character, added deadTime and death mechanic, moved intput into this update method
    // T 4/7/15- readded fire method to move bullet to gun's position and character's direction on firing, added variables and code in update and draw for sword
    // B 4/14/15 - added code to control the player's stamina, including the outOfStamina and onPlatform attributes
    // T 4/19/15- merged GroundPlayer, SkyPlayer, and Player; renamed it player, added code stubs to draw, update, and fire to differentiate between turret and flying; added code to update and fire
    // T 4/22/15- fixed up the draw and updates to reflect new values of direction for ground player
    // T 4/24/15- added platform collisions, fixed shit
    public class Player: Character
    {
        private int initDir; // Stores initial direction
        private int deadTime; // Timer for death sprite
        private int slashTime; // Timer for slash cooldown
        private bool outOfStamina = false;
        private bool onPlatform = false;
        public long score; // player score
        private int dashCD; // timer for dash cooldown
        private bool dashActive; // if player has dash status or not
        private Platform platform;
        private KeyboardState kstate; // Hold pressed keys
        private Rectangle resetPos; // Rectangle to hold position for sprite reset upon death of character
        private int damage; // Holds damage for the sword
        private int livesLeft; // Lives character has
        private double stamina; // the amount of stamina the character has
        private int slashFrame; // Is the character slashing
        private Rectangle sBox; // Position of the hitbox for the sword

        public int lives // Properties to access all the variables needed outside this class
        {
            get { return livesLeft; }
            set { livesLeft = value; }
        }
        public double Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        }
        public Platform Platform
        {
            get { return platform; }
            set { platform = value; }
        }
        public bool OutOfStamina
        {
            get { return outOfStamina; }
            set { outOfStamina = value; }
        }
        public bool OnPlatform
        {
            get { return onPlatform; }
            set { onPlatform = value; }
        }
        public int swordDamage
        {
            get { return damage; }
            set { damage = value; }
        }
        public Rectangle swordBox
        {
            get { return sBox; }
            set { sBox = value; }
        }
        public int slashFrames
        {
            get { return slashFrame; }
        }

        public Player(int X, int Y, int dir, int spd, Texture2D[] charSprite, Texture2D[] bulletSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, charSprite)
        {
            cooldown = 0; // Let the player shoot
            charHealth = 3; // Default health
            livesLeft = 3; // Default lives
            stamina = 100; // Default stamina
            resetPos = new Rectangle(X, Y, charSprite[0].Width, charSprite[0].Height); // Sets position to return to when player dies

            bullets = new Bullet[50]; // Initialize bullet array
            for (int i = 0; i < bullets.Length; i++)
            {                                                          
                bullets[i] = new Bullet(0, 0, 0, 10, 1, bulletSprite);
            }

            bulletQueue = new int[50];
            for (int i = 0; i < bullets.Length; i++)
            {
                bulletQueue[i] = i; // Initialize the queue
            }

            deadTime = 0;
            initDir = dir; // Sets direction to return to upon death
            slashFrame = 0; // Counts up the frames in the animation
            slashTime = 0;
        }

        public override void TakeHit(int dmg)
        {
            base.TakeHit(dmg);

            if (charHealth <= 0)
            {
                onPlatform = false;
                lives--; // Take away a life
                stamina = 100; // Reset health and position and direction and stamina
                charHealth = 4; 
                direction = 8; // Dead sprite
            }
        }

        public override void Update() // Inputs and movement
        {
            base.Update();

            if (direction == 8 && deadTime == 100) // Death works the same on land and air
            {
                direction = 0;
                spritePos = resetPos; // Reset after the player has sufficiently suffered for their failure
                direction = initDir;
            }
            else if (direction == 8)
            {
                deadTime++;
                return; // Don't let the player move while viewing their death
            }

            if (onPlatform == false) // Sky update
            {
                // stamina code begins here
                if (Stamina > 0)
                {
                    outOfStamina = false; // Decrease stamina while flying
                    Stamina -= .03;
                }
                if (Stamina <= 0 && outOfStamina == false)
                {
                    outOfStamina = true; // If the character runs out of stamina, put them in the fall state
                    direction = 16;
                    spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spritePos.Width, spritePos.Height); // Move down
                    return;
                }
                // stamina code ends here

                if (direction > 8 && direction < 13) // If the slash animation isn't over, don't let the user screw it up
                {
                    if (slashFrame >= 20) // If the animation is over, go back to the original state and reset the animation counter (this is 5 frames by the way, it starts at 0)
                    {
                        slashFrame = 0; // Reset the # of frames
                        switch (direction)
                        {
                            case 10:
                                direction = 2; // Left
                                break;
                            case 11:
                                direction = 4; // Right
                                break;
                            case 12:
                                direction = 6; // Up
                                break;
                            default:
                                direction = 0; // Down
                                break;
                        }
                    }
                    else
                    {
                        return; // Otherwise, wait for the animation to finish
                    }
                }

                if (outOfStamina != true)
                {
                    // INPUT
                    kstate = Keyboard.GetState(); // Get pressed keys
                    Keys[] pressedKeys = kstate.GetPressedKeys();

                    if (direction == 1 && !kstate.IsKeyDown(Keys.Right)) // If direction is moveRight and right key is not down, revert to facing right
                    {
                        direction = 0;
                    }
                    else if (kstate.IsKeyDown(Keys.Right) && direction == 0) // Move right on second press 
                    {
                        direction = 1;
                    }
                    else if (kstate.IsKeyDown(Keys.Right)) // Face right on first press
                    {
                        direction = 0;
                    }

                    if (direction == 3 && !kstate.IsKeyDown(Keys.Left)) // If direction is moveLeft and left key is not pressed, revert to facing left
                    {
                        direction = 2;
                    }
                    else if (kstate.IsKeyDown(Keys.Left) && direction == 2) // Move left on second press
                    {
                        direction = 3;
                    }
                    else if (kstate.IsKeyDown(Keys.Left)) // Face left on first press
                    {
                        direction = 2;
                    }

                    if (direction == 5 && !kstate.IsKeyDown(Keys.Up)) // If direction is moveUp and up key is not down, revert to facing up
                    {
                        direction = 4;
                    }
                    else if (kstate.IsKeyDown(Keys.Up) && direction == 4) // Move up on second press
                    {
                        direction = 5;
                    }
                    else if (kstate.IsKeyDown(Keys.Up)) // Face up on first press
                    {
                        direction = 4;
                    }

                    if (direction == 7 && !kstate.IsKeyDown(Keys.Down)) // If direction is moveDown and down key is not pressed, revert to facing down
                    {
                        direction = 6;
                    }
                    else if (kstate.IsKeyDown(Keys.Down) && direction == 6) // Move down on second press
                    {
                        direction = 7;
                    }
                    else if (kstate.IsKeyDown(Keys.Down)) // Face down on first press
                    {
                        direction = 6;
                    }
                    // END INPUT
                }

                switch (direction) // Move the sprites
                {
                    case 1: // Moving right
                        spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spriteArray[0].Width, spriteArray[0].Height);
                        dashActive = false;
                        this.objSpeed = 8;
                        break;
                    case 3: // Move left
                        spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spriteArray[0].Width, spriteArray[0].Height);
                        dashActive = false;
                        this.objSpeed = 8;
                        break;
                    case 5: // Moving up
                        spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spriteArray[4].Width, spriteArray[4].Height);
                        dashActive = false;
                        this.objSpeed = 8;
                        break;
                    case 7: case 16: // Move down or falling
                        spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spriteArray[4].Width, spriteArray[4].Height);
                        dashActive = false;
                        this.objSpeed = 8;
                        break;
                    case 8: // Dead
                        spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[8].Width, spriteArray[8].Height);
                        break;
                }

                // Return to positions
                if (spritePos.Left < 0) // If moving left and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(1, spritePos.Y, spritePos.Width, spritePos.Height);
                }
                else if (spritePos.Right > Game1.clientWidth) // If moving right and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(Game1.clientWidth - this.spritePos.Width, spritePos.Y, spritePos.Width, spritePos.Height);
                }

                if (spritePos.Top < 0) // If moving up and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(spritePos.X, 1, spritePos.Width, spritePos.Height);
                }
                else if (spritePos.Top > Game1.clientHeight && outOfStamina == true) // If falling down and it puts you beyond the bounds
                {
                    TakeHit(25); // Kills you
                }
                else if (spritePos.Bottom > Game1.clientHeight) // If moving down and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(spritePos.X, Game1.clientHeight - this.spritePos.Height, spritePos.Width, spritePos.Height);
                }

                // Slashing
                if (kstate.IsKeyDown(Keys.S) && slashTime <= 0 && direction < 8) // Wait for cooldown to end
                {
                    if (direction == 2 || direction == 3) // Pop up a hitbox in front of the character and put them in the appropriate slashing state
                    {
                        sBox = new Rectangle(spritePos.X, spritePos.Y, 56, 136);
                        direction = 10; // Left
                    }
                    else if (direction == 4 || direction == 5)
                    {
                        sBox = new Rectangle(spritePos.X, spritePos.Y, 56, 136);
                        direction = 11; // Up
                    }
                    else if (direction == 6 || direction == 7)
                    {
                        sBox = new Rectangle(spritePos.Left, spritePos.Bottom - 20, 225, 61);
                        direction = 12; // Down
                    }
                    else
                    {
                        sBox = new Rectangle(spritePos.Right - 20, spritePos.Top, 225, 61);
                        direction = 9; // Right
                    }
                    slashTime = 15; // Go into cooldown
                    Stamina = Stamina - 2;
                }

                // Dashing
                //***right now, it dashes without regards to the Current that pushes him, 
                //so we'll need to create that object and check for its interception with
                //the player then give the speed boost

                if (kstate.IsKeyDown(Keys.D) && dashCD <= 0 && direction < 8) // Dash, then go into cooldown
                {
                    //try
                    //{
                    this.objSpeed = 250;
                    //}
                    dashCD = 120; // Go into cooldown
                    Stamina = Stamina - 5;
                }

                // Platform collision
                if (this.spritePos.Intersects(platform.spritePos))
                {
                    switch (direction)
                    {
                        case 1:
                            spritePos = new Rectangle(platform.spritePos.Left - spritePos.Width, spritePos.Y, spritePos.Width, spritePos.Height); // Collides with left side
                            break;
                        case 3:
                            spritePos = new Rectangle(platform.spritePos.Right, spritePos.Y, spritePos.Width, spritePos.Height); // Collides with right side
                            break;
                        case 5:
                            spritePos = new Rectangle(spritePos.X, platform.spritePos.Bottom, spritePos.Width, spritePos.Height); // Collides with bottom
                            break;
                        default: // Catches both moveDown and falling
                            spritePos = new Rectangle(spritePos.X, platform.spritePos.Top - spritePos.Height, spritePos.Width, spritePos.Height); // Collides with top
                            onPlatform = true;
                            direction = 13;
                            break;
                    }
                }
            }
            else // Ground update
            {
                if (stamina < 99.99)
                {
                    stamina += .1; // Regen stamina when landed
                }
                else if (stamina >= 99.99)
                {
                    stamina = 100; // Cap
                }
                // INPUT
                kstate = Keyboard.GetState(); // Get pressed keys
                Keys[] pressedKeys = kstate.GetPressedKeys();

                if (direction == 14 && !kstate.IsKeyDown(Keys.Right)) // Go neutral if moving right and key is released
                {
                    direction = 13;
                }
                else if (direction == 13 && kstate.IsKeyDown(Keys.Right)) // Go right if neutral and right is pressed
                {
                    direction = 14;
                }

                if (direction == 15 && !kstate.IsKeyDown(Keys.Left)) // Go neutral if moving left and key is released
                {
                    direction = 13;
                }
                else if (direction == 13 && kstate.IsKeyDown(Keys.Left)) // Go left if neutral an left is pressed
                {
                    direction = 15;
                }
                else if (stamina != 0 && kstate.IsKeyDown(Keys.A)) // Allow takeoff
                {
                    onPlatform = false;
                    direction = 5;
                }
                // END INPUT

                switch (direction) // Move the sprites
                {
                    case 14: // Moving right
                        spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                        break;
                    case 15: // Move left
                        spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                        break;
                }

                // Return to positions
                if (spritePos.X < platform.spritePos.Left) // If moving left and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(platform.spritePos.Left, spritePos.Y, spritePos.Width, spritePos.Height);
                }
                else if (spritePos.Right > Game1.clientWidth) // If moving right and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(Game1.clientWidth - spritePos.Width, spritePos.Y, spritePos.Width, spritePos.Height);
                }
                else if (spritePos.Right > platform.spritePos.Right) // If moving right and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(platform.spritePos.Right - this.spritePos.Width, spritePos.Y, spritePos.Width, spritePos.Height);
                }
                else if (spritePos.X < 0) // If moving left and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(1, spritePos.Y, spritePos.Width, spritePos.Height);
                }
            }

            // Firing - the return in the death block prevents firing while dead- fire method is called the same way regardless of platform bool
            if (kstate.IsKeyDown(Keys.F) && cooldown <= 0) // Fire, then go into cooldown
            {
                try
                {
                    Fire();
                }
                catch (IndexOutOfRangeException noBullets)
                {
                    throw new IndexOutOfRangeException(); // If it tries to fire and there are no bullets, throw up further
                }
                cooldown = 25; // Go into cooldown
            }

            dashCD--;
            slashTime--;
            cooldown--; // Increment cooldowns to allow dashing, slashing, and firing again
        }

        public override void Draw(SpriteBatch spriteBatch) // Draw the character's sprite
        {
            switch (direction) // Draw the slashes
            {
                case 0: case 1:
                    spriteBatch.Draw(spriteArray[0], spritePos, color); // Right
                    break;
                case 2: case 3:
                    spriteBatch.Draw(spriteArray[0], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0); // Left
                    break;
                case 4: case 5: case 16:
                    spriteBatch.Draw(spriteArray[4], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0); // Up or falling
                    break;
                case 6: case 7:
                    spriteBatch.Draw(spriteArray[4], spritePos, color); // Down
                    break;
                case 8:
                    spriteBatch.Draw(spriteArray[8], spritePos, color);
                    break;
                // This is how animation works in C#: we have the entry in the sprite array contain a row of the frames, the rectangle is the source rectangle from the image- so frame # times the width of a single frame is the distance from the left at which the source box is located
                case 9:
                    spriteBatch.Draw(spriteArray[9], spritePos, color);
                    break; // Right Slash
                case 10:
                    spriteBatch.Draw(spriteArray[9], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    break; // Left Slash
                case 11:
                    spriteBatch.Draw(spriteArray[11], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                    break; // Up Slash
                case 12:
                    spriteBatch.Draw(spriteArray[11], spritePos, color);
                    break; // Down Slash
                case 13: case 14: case 15:
                    spriteBatch.Draw(spriteArray[13], spritePos, color);
                    break;
            }
            base.Draw(spriteBatch);
            slashFrame++; // Move to the next frame
            color = Color.White; // And reset the color
        }

        public override void Fire() // Fires a bullet NOTE- ALL POSITIONS EXCEPT RIGHT WILL HAVE TO BE CHANGED WHEN NEW SPRITES ARE USED
        {
            base.Fire(); // Get a bullet
            if (direction == 2 || direction == 3) // Change bullet to match gun's position and character's direction
            {
                bullets[bul].spritePos = new Rectangle(spritePos.Left, spritePos.Y + (40 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 1; // Left
            }
            else if (direction == 4 || direction == 5)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 165, spritePos.Y + (196 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 2; // Up
            }
            else if (direction > 12)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 136, spritePos.Y + (28 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 2; // Ground/up
            }
            else if (direction == 6 || direction == 7)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 165, spritePos.Bottom + (196 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 3; // Down
            }
            else
            {
                bullets[bul].spritePos = new Rectangle(spritePos.Right, spritePos.Y + (40 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 0; // Right
            }
            bullets[bul].isActive = true;
        }
    }
}
