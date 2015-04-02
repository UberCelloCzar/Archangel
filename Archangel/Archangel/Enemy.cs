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
    // Contains all methods and code for enemies and enemy AI

    // Change Log
    // T 3/28/15- fixed constructor, added draw and update code
    // T 3/30/15- added fire method and updated constructor for it
    // T 3/31/15- added fire code to Update
    public class Enemy:Character
    {

        public Enemy(int X, int Y, int dir, int spd, Texture2D[] loadSprite, Texture2D[] bulletSprite) // Sets x,y, direction, and sprite for character
            : base (X, Y, dir, spd, loadSprite)
        {
            bullets = new Bullet[50]; // Initialize bullet array
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Bullet(0, 0, 0, 5, 1, bulletSprite);
            }
            direction = dir; // Initial direction
            charHealth = 2; // Enemy health
        }

        public override void TakeHit(int dmg) // Enemy takes a hit and possibly dies
        {
            base.TakeHit(dmg);

            if (charHealth <= 0) // Deactivate enemy when killed
            {
                direction = 8; // Changes state to show death sprite, disappearance is after takehit method is called in calling class
                Thread.Sleep(200);
            }
        }

        public override void Update()
        {
            // Add code here if you want to have enemy move in any direction other than the passed in direction

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

            if (cooldown == 0) // Fire at a set rate until AI is implemented
            {
                Fire();
                cooldown = 10; // Go into cooldown
            }

            cooldown--; // Countdown to fire again
        }

        public override void Draw(SpriteBatch spriteBatch) // Draws the ememies
        {
            base.Draw(spriteBatch);
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
                if (i == bullets.Length - 1) // If all bullets are active (ideally not possible)
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
