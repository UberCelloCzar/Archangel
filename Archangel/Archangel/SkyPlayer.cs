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

namespace Archangel
{
    // Cheshire Games, Bierre, March 16, 2015
    // Contains necessary code and methods for the player object while flying through the air

    // Change Log
    // T 3/26/15- removed enumeration, changed initDir to an int
    // T 3/27/15- added draw and update code
    class SkyPlayer:Player
    {
        private int initDir; // Stores initial direction

        public SkyPlayer(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, loadSprite)
        {
            initDir = dir; // Sets direction to return to upon death
        }

        public override void TakeHit(int dmg)
        {
            base.TakeHit(dmg);

            if (charHealth <= 0)
            {
                lives--; // Take away a life
                charHealth = 3; // Reset health and position and direction
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

        public override void Draw() // Draw the character's sprite
        {
            base.Draw();
        }
    }
}
