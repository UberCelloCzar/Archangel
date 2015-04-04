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
    public class SkyPlayer:Player
    {
        private int initDir; // Stores initial direction
        public int deadTime; // Timer for death sprite

        public SkyPlayer(int X, int Y, int dir, int spd, Texture2D[] charSprite, Texture2D[] bulletSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, charSprite)
        {
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
        }

        public override void TakeHit(int dmg)
        {
            base.TakeHit(dmg);

            if (charHealth <= 0)
            {
                lives--; // Take away a life
                charHealth = 3; // Reset health and position and direction
                direction = 8; // Dead sprite
            }
        }

        public override void Update()
        {
            base.Update();
            if (direction == 8 && deadTime == 5)
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

            switch (direction) // Move the sprites
            {
                case 1: // Moving right
                    spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                    break;
                case 3: // Move left
                    spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                    break;
                case 5: // Moving up
                    spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spritePos.Width, spritePos.Height);
                    break;
                case 7: // Move down
                    spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spritePos.Width, spritePos.Height);
                    break;
            }

            // Return to positions
            if (direction == 3 && spritePos.X < 0) // If moving left and it puts you beyond the bounds
            {
                spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
            }
            else if (direction == 1 && spritePos.X > (Game1.clientWidth)) // If moving right and it puts you beyond the bounds
            {
                spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
            }

            if (direction == 5 && spritePos.Y < 0) // If moving up and it puts you beyond the bounds
            {
                spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spritePos.Width, spritePos.Height);
            }
            else if (direction == 7 && spritePos.Y > (Game1.clientHeight)) // If moving down and it puts you beyond the bounds
            {
                spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spritePos.Width, spritePos.Height);
            }

            if (kstate.IsKeyDown(Keys.Space) && cooldown <= 0) // Fire, then go into cooldown
            {
                try
                {
                    Fire();
                }
                catch (IndexOutOfRangeException noBullets)
                {
                    throw new IndexOutOfRangeException(); // If it tries to fire and there are no bullets, throw up further
                }
                cooldown = 15; // Go into cooldown
            }

            cooldown--; // Increment cooldown to allow firing again
        }

        public override void Draw(SpriteBatch spriteBatch) // Draw the character's sprite
        {
            base.Draw(spriteBatch);
        }

        public override void Fire()
        {
            base.Fire();
            if (direction == 0 || direction == 1) // Move the bullet to the character's direction (middle of the character sprite)
            {
                bullets[bul].direction = 0; // Right
            }
            else if (direction == 2 || direction == 3)
            {
                bullets[bul].direction = 1; // Left
            }
            else if (direction == 4 || direction == 5)
            {
                bullets[bul].direction = 2; // Up
            }
            else
            {
                bullets[bul].direction = 3; // Down
            }
        }
    }
}
