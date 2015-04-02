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
    public class SkyPlayer:Player
    {
        private int initDir; // Stores initial direction

        public SkyPlayer(int X, int Y, int dir, int spd, Texture2D[] charSprite, Texture2D[] bulletSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, charSprite)
        {
            bullets = new Bullet[50]; // Initialize bullet array
            for (int i = 0; i < bullets.Length; i++)
            {                                                          
                bullets[i] = new Bullet(0, 0, 0, 5, 1, bulletSprite);
            }
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
                Thread.Sleep(200);
                spritePos = resetPos;
                direction = initDir;
            }
        }

        public override void Update()
        {
            base.Update();

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
            else if (direction == 1 && spritePos.X > (Game1.clientBounds.Width)) // If moving right and it puts you beyond the bounds
            {
                spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
            }

            if (direction == 5 && spritePos.Y < 0) // If moving up and it puts you beyond the bounds
            {
                spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spritePos.Width, spritePos.Height);
            }
            else if (direction == 7 && spritePos.Y > (Game1.clientBounds.Height)) // If moving down and it puts you beyond the bounds
            {
                spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spritePos.Width, spritePos.Height);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) // Draw the character's sprite
        {
            base.Draw(spriteBatch);
            for (int i = 0; i < bullets.Length; i++) // Draw active bullets
            {
                if (bullets[i].isActive)
                {
                    bullets[i].Draw(spriteBatch);
                }
            }
        }

        public override void Fire()
        {
            int i; // Must be able to use the found bullet
            for (i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].isActive) // Find and use an inactive bullet
                {
                    break;
                }
                if (i == bullets.Length-1) // If all bullets are active (ideally not possible)
                {
                    throw new Exception(); // I don't want to add code for a new type of exception, so just be general
                } // NOTE: I want to catch this in a try-catch block when fire is called, wherever that will be

            }
            bullets[i].direction = direction; // Move the bullet to the character's position and direction (middle of the character sprite)
            bullets[i].spritePos = new Rectangle(spriteArray[direction].Bounds.Left + spriteArray[direction].Width / 2, spriteArray[direction].Bounds.Top + spriteArray[direction].Height / 2, spriteArray[direction].Width, spriteArray[direction].Height);
            bullets[i].isActive = true;
        }
    }
}
