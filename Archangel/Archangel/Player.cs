using System;
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
    // T 4/25/15- fixed sprite aspect ratios, fixed death timers
    // T 4/28/15- fixed gun and slash positions, fixed spritepos issue, fixed slash speed issue, added manual fall mechanic
    // T 5/4/15- fixed takeoff and same-key inputs, fixed draw and gun positions for new sprites, removed some properties, added dash
    public class Player: Character
    {
        private int initDir; // Stores initial direction
        private int deadTime; // Timer for death sprite
        private int slashTime; // Timer for slash cooldown
        private bool outOfStamina = false;
        private bool onPlatform = false;
        public long score; // Player score
        private int dashCD; // Timer for dash cooldown
        public int dashActive; // If player has dash status or not
        private Platform platform;
        private KeyboardState kstate; // Hold pressed keys
        private Rectangle resetPos; // Rectangle to hold position for sprite reset upon death of character
        private int damage; // Holds damage for the sword
        private int livesLeft; // Lives character has
        private double stamina; // the amount of stamina the character has
        private int slashFrame; // Is the character slashing
        private Rectangle sBox; // Position of the hitbox for the sword
        private int aPressed; // How many frames ago was the A button pressed, counts to 10 frames, then lets the a button do something again
        public AirCurrent dash; // Air current the dash is based on
        private bool controlledFall = false;

        public int lives // Properties to access all the variables needed outside this class
        {
            get { return livesLeft; }
        }
        public double Stamina
        {
            get { return stamina; }
        }
        public Platform Platform
        {
            get { return platform; }
            set { platform = value; }
        }
        public bool OnPlatform
        {
            get { return onPlatform; }
            set { onPlatform = value; }
        }
        public int swordDamage
        {
            get { return damage; }
        }
        public Rectangle swordBox
        {
            get { return sBox; }
        }
        public int slashFrames
        {
            get { return slashFrame; }
        }

        public Player(int X, int Y, int dir, int spd, Texture2D[] charSprite, Texture2D[] bulletSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, charSprite)
        {
            cooldown = 0; // Let the player shoot
            charHealth = 5; // Default health
            livesLeft = 3; // Default lives
            stamina = 100; // Default stamina
            resetPos = new Rectangle(X, Y, charSprite[0].Width, charSprite[0].Height); // Sets position to return to when player dies
            damage = 2; // Set sword damage
            aPressed = 0; // Set the key to unpressed
            Texture2D[] dashSprite = new Texture2D[1]; // Pass in an array of 1, to eliminate extra code
            dashSprite[0] = charSprite[17];
            dash = new AirCurrent(0, 0, 0, 15, dashSprite); // Create the current

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
                livesLeft--; // Take away a life
                stamina = 100; // Reset health and position and direction and stamina
                charHealth = 5; 
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
                if (stamina > 0 && controlledFall == false)
                {
                    outOfStamina = false; // Decrease stamina while flying
                    stamina -= .03;
                }
                if (stamina <= 0 && outOfStamina == false)
                {
                    outOfStamina = true; // If the character runs out of stamina, put them in the fall state
                    direction = 16;
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

                if (outOfStamina != true && dashActive == 0)
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
                    
                    if (stamina > 0 && direction != 16 && kstate.IsKeyDown(Keys.A) && aPressed == 0)
                    {
                        direction = 16;
                        controlledFall = true;
                        aPressed = 1; // Start the counter
                    }
                    else if (direction == 16 && kstate.IsKeyDown(Keys.A) && aPressed == 0) // Let the player fall and recover on command (if there is stamina left)
                    {
                        direction = 0;
                        aPressed = 1; // Start the counter
                    }
                    else if (aPressed > 0 && aPressed < 10)
                    {
                        aPressed++; // Add to the counter
                    }
                    else if (aPressed > 0)
                    {
                        aPressed = 0; // Reset the counter
                    }
                    // END INPUT
                }
                else if (outOfStamina != true && dashActive > 0)
                {
                    switch (direction) // Make sure player is moving if in the dash
                    {
                        case 0:
                            direction = 1; // Right
                            break;
                        case 2:
                            direction = 3; // Left
                            break;
                        case 4:
                            direction = 5; // Up
                            break;
                        case 6:
                            direction = 7; // Down
                            break;
                    }

                    if (dashActive == 1) // Suck the player into the center
                    {
                        spritePos = new Rectangle(dash.spritePos.Center.X - (spritePos.Width / 2), dash.spritePos.Center.Y - spritePos.Height - 50, spritePos.Width, spritePos.Height);
                    }
                }

                switch (direction) // Move the sprites
                {
                    case 1: // Moving right
                        controlledFall = false;
                        if (dashActive >= 21)
                        {
                            dashActive = 0;
                            this.objSpeed = 8;
                            spritePos = new Rectangle(dash.spritePos.Left - spritePos.Width - 5, spritePos.Y, spritePos.Width, spritePos.Height);
                        }
                        else
                        {
                            spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                        }
                        break;
                    case 3: // Move left
                        controlledFall = false;
                        if (dashActive >= 21)
                        {
                            dashActive = 0;
                            this.objSpeed = 8;
                            spritePos = new Rectangle(dash.spritePos.Right + 5, spritePos.Y, spritePos.Width, spritePos.Height);
                        }
                        else
                        {
                            spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                        }
                        break;
                    case 5: // Moving up
                        controlledFall = false;
                        if (dashActive >= 21)
                        {
                            dashActive = 0;
                            this.objSpeed = 8;
                            spritePos = new Rectangle(spritePos.X, dash.spritePos.Bottom + 5, spritePos.Width, spritePos.Height);
                        }
                        else
                        {
                            spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spritePos.Width, spritePos.Height);
                        }
                        break;
                    case 7: //case 16: // Move down
                        controlledFall = false;
                        if (dashActive >= 21)
                        {
                            dashActive = 0;
                            this.objSpeed = 8;
                            spritePos = new Rectangle(spritePos.X, dash.spritePos.Top - spritePos.Height - 5, spritePos.Width, spritePos.Height);
                        }
                        else
                        {
                            spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spritePos.Width, spritePos.Height);
                        }
                        break;
                    case 16: // Falling
                        if (dashActive >= 21)
                        {
                            dashActive = 0;
                            this.objSpeed = 8;
                            spritePos = new Rectangle(spritePos.X, dash.spritePos.Top - spritePos.Height - 5, spritePos.Width, spritePos.Height);
                        }
                        else
                        {
                            spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spritePos.Width, spritePos.Height);
                        }
                        break;
                }
                if (dashActive > 0) // Continue dash if active
                {
                    dashActive++;
                }

                dash.Update();

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
                else if (spritePos.Top > Game1.clientHeight && direction == 16) // If falling down and it puts you beyond the bounds
                {
                    TakeHit(25); // Kills you
                }
                else if (spritePos.Bottom > Game1.clientHeight && direction != 16) // If moving down and it puts you beyond the bounds
                {
                    spritePos = new Rectangle(spritePos.X, Game1.clientHeight - this.spritePos.Height, spritePos.Width, spritePos.Height);
                }

                // Slashing
                if (kstate.IsKeyDown(Keys.S) && slashTime <= 0 && direction < 8) // Wait for cooldown to end
                {
                    if (direction == 2 || direction == 3) // Pop up a hitbox in front of the character and put them in the appropriate slashing state
                    {
                        sBox = new Rectangle(spritePos.X, spritePos.Y, 40, spritePos.Height);
                        direction = 10; // Left
                    }
                    else if (direction == 4 || direction == 5)
                    {
                        sBox = new Rectangle(spritePos.X, spritePos.Y, spritePos.Width, 26);
                        direction = 11; // Up
                    }
                    else if (direction == 6 || direction == 7)
                    {
                        sBox = new Rectangle(spritePos.X, spritePos.Bottom - 26, spritePos.Width, 26);
                        direction = 12; // Down
                    }
                    else
                    {
                        sBox = new Rectangle(spritePos.Right - 40, spritePos.Y, 40, spritePos.Height);
                        direction = 9; // Right
                    }
                    slashFrame = 0;
                    slashTime = 20; // Go into cooldown
                    stamina = stamina - 2;
                }

                // Dashing
                //***right now, it dashes without regards to the Current that pushes him, 
                //so we'll need to create that object and check for its interception with
                //the player then give the speed boost

                if (kstate.IsKeyDown(Keys.D) && dashCD <= 0 && direction < 8) // Dash, then go into cooldown
                {
                    dash.spritePos = new Rectangle(this.spritePos.Center.X, this.spritePos.Center.Y, dash.spritePos.Width, dash.spritePos.Height); // Spawn the dash box
                    switch (direction)
                    {
                        case 0: case 1:
                            dash.direction = 0; // Right
                            break;
                        case 2: case 3:
                            dash.direction = 1; // Left
                            break;
                        case 4: case 5:
                            dash.direction = 2; // Up
                            break;
                        case 6: case 7:
                            dash.direction = 3; // Down
                            break;
                    }
                    dash.dashFrame = 1;
                    dashCD = 240; // Go into cooldown
                    stamina = stamina - 5;
                }

                // Dash collision
                if (dash.dashFrame > 0 && this.spritePos.Intersects(dash.spritePos) && dashActive <= 0)
                {
                    this.objSpeed = 20; // Speed up and start counting
                    dashActive = 1;
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
                            spritePos = new Rectangle(spritePos.X, (platform.spritePos.Top + 12) - spritePos.Height, spritePos.Width, spritePos.Height); // Collides with top
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
                else if (stamina != 0 && kstate.IsKeyDown(Keys.A) && aPressed == 0) // Allow takeoff
                {
                    onPlatform = false;
                    spritePos = new Rectangle(spritePos.X, spritePos.Y - 30, spritePos.Width, spritePos.Height);
                    direction = 5;
                    aPressed = 1; // Start the counter
                }
                else if (aPressed > 0 && aPressed < 10)
                {
                    aPressed++; // Add a frame to the counter
                }
                else if (aPressed > 0)
                {
                    aPressed = 0; // Reset the counter
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
            switch (direction) // Draw the sprites
            {
                case 0: case 1:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[0].Width, spriteArray[0].Height);
                    spriteBatch.Draw(spriteArray[0], new Rectangle(spritePos.X, spritePos.Y, spriteArray[0].Width, spriteArray[0].Height), color); // Right
                    break;
                case 2: case 3:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[0].Width, spriteArray[0].Height);
                    spriteBatch.Draw(spriteArray[0], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0); // Left
                    break;
                case 4: case 5:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[4].Width, spriteArray[4].Height);
                    spriteBatch.Draw(spriteArray[4], spritePos, color); // Up
                    break;
                case 6: case 7:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[6].Width, spriteArray[6].Height);
                    spriteBatch.Draw(spriteArray[6], spritePos, color); // Down
                    break;
                case 8:
                    spriteBatch.Draw(spriteArray[8], new Rectangle(spritePos.X, spritePos.Y, spriteArray[8].Width, spriteArray[8].Height), color);
                    break;
                case 16:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[6].Width, spriteArray[6].Height);
                    spriteBatch.Draw(spriteArray[6], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0); // Falling
                    break;
                // This is how animation works in C#: we have the entry in the sprite array contain a row of the frames, the rectangle is the source rectangle from the image- so frame # times the width of a single frame is the distance from the left at which the source box is located
                case 9:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[9].Width, spriteArray[9].Height);
                    spriteBatch.Draw(spriteArray[9], spritePos, color);
                    slashFrame++; // Move to the next frame
                    break; // Right Slash
                case 10:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[9].Width, spriteArray[9].Height);
                    spriteBatch.Draw(spriteArray[9], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    slashFrame++; // Move to the next frame
                    break; // Left Slash
                case 11:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[11].Width, spriteArray[11].Height);
                    spriteBatch.Draw(spriteArray[11], spritePos, null, color, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                    slashFrame++; // Move to the next frame
                    break; // Up Slash
                case 12:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[11].Width, spriteArray[11].Height);
                    spriteBatch.Draw(spriteArray[11], spritePos, color);
                    slashFrame++; // Move to the next frame
                    break; // Down Slash
                case 13: case 14: case 15:
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteArray[13].Width, spriteArray[13].Height);
                    spriteBatch.Draw(spriteArray[13], spritePos, color);
                    break;
            }

            dash.Draw(spriteBatch); // Draw the dash

            base.Draw(spriteBatch);
            color = Color.White; // And reset the color
        }

        public override void Fire() // Fires a bullet NOTE- ALL POSITIONS WILL HAVE TO BE CHANGED WHEN NEW SPRITES ARE USED
        {
            base.Fire(); // Get a bullet
            if (direction == 2 || direction == 3) // Change bullet to match gun's position and character's direction
            {
                bullets[bul].spritePos = new Rectangle(spritePos.Left - bullets[bul].spritePos.Width, spritePos.Y + 30 - (bullets[bul].spritePos.Height / 2), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 1; // Left
            }
            else if (direction == 4 || direction == 5)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 48 - (bullets[bul].spritePos.Width / 2), spritePos.Y + 2 - bullets[bul].spritePos.Height, bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 2; // Up
            }
            else if (direction > 12)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 66 - (bullets[bul].spritePos.Width / 2), spritePos.Y + 14 - bullets[bul].spritePos.Height, bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 2; // Ground (bullet is still going up)
            }
            else if (direction == 6 || direction == 7)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 72 - (bullets[bul].spritePos.Width / 2), spritePos.Y + 78, bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 3; // Down
            }
            else
            {
                bullets[bul].spritePos = new Rectangle(spritePos.Right, spritePos.Y + 30 - (bullets[bul].spritePos.Height / 2), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 0; // Right
            }
            bullets[bul].isActive = true;
        }
    }
}
