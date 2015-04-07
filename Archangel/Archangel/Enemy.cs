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
    // Contains all methods and code for enemies and enemy AI

    // Change Log
    // T 3/28/15- fixed constructor, added draw and update code
    // T 3/30/15- added fire method and updated constructor for it
    // T 3/31/15- added fire code to Update
    // T 4/2/15- moved fire method to Character, added death timer and mechanic
    // T 4/7/15- readded fire method to move bullet to gun's position and character's direction on firing
    public class Enemy:Character
    {
        private int deadTime; // Timer for how long enemy is drawn as dead before disappearing
        public int deathTimer
        {
            get { return deadTime; }
        }

        public Enemy(int X, int Y, int dir, int spd, Texture2D[] loadSprite, Texture2D[] bulletSprite) // Sets x,y, direction, and sprite for character
            : base (X, Y, dir, spd, loadSprite)
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

            cooldown = 2; // Don't let them fire immediately
            direction = dir; // Initial direction
            charHealth = 2; // Enemy health
        }

        public override void TakeHit(int dmg) // Enemy takes a hit and possibly dies
        {
            base.TakeHit(dmg);

            if (charHealth <= 0) // Deactivate enemy when killed
            {
                direction = 8; // Changes state to show death sprite, disappearance is after takehit method is called in calling class
            }
        }

        public override void Update()
        {
            base.Update();
            if (direction == 8)
            {
                deadTime++;
                return; // Don't let the enemy move while dead
            }

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

            if (cooldown <= 0) // Fire at a set rate until AI is implemented
            {
                try
                {
                    Fire();
                }
                catch (IndexOutOfRangeException noBullets)
                {
                    throw new IndexOutOfRangeException(); // If it tries to fire and there are no bullets, throw up further
                }
                cooldown = 80; // Go into cooldown
            }

            cooldown--; // Countdown to fire again
        }

        public override void Draw(SpriteBatch spriteBatch) // Draws the ememies
        {
            base.Draw(spriteBatch);
        }

        public override void Fire()
        {
            base.Fire();
            if (direction == 2 || direction == 3) // Move the bullet to the character's direction and gun's position
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X - bullets[bul].spritePos.Width, spritePos.Y + (34 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 0; // Right
            }
            else if (direction == 4 || direction == 5)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X - bullets[bul].spritePos.Width, spritePos.Y + (34 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 2; // Up
            }
            else if (direction == 6 || direction == 7)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X - bullets[bul].spritePos.Width, spritePos.Y + (34 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 3; // Down
            }
            else
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X - bullets[bul].spritePos.Width, spritePos.Y + (34 - (bullets[bul].spritePos.Height / 2)), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 1; // Left
            }
            bullets[bul].isActive = true;
        }
    }
}
