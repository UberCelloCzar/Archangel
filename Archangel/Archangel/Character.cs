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
    // Cheshire Games, Bierre, March 14, 2015
    // Contains necessary items for enemies and player objects

    // Change Log
    // T 3/28/15- added fire method
    // T 3/29/15- added bullet array
    // T 3/31/15- added fireRate and cooldown for firing, added color variable for hit showing
    // T 4/2/15- added Draw and Update to hand bullet moving and drawing code to both enemies and player, moved fire method to here, moved color method up to movablegameobject
    public abstract class Character:MovableGameObject
    {
        private int health; // HP for character and properties
        public int charHealth
        {
            get { return health; }
            set { health = value; }
        }

        private int fireRate; // Wait time after firing
        public int cooldown
        {
            get { return fireRate; }
            set { fireRate = value; }
        }

        private Bullet[] blt; // Array of bullets and properties
        public Bullet[] bullets
        {
            get { return blt; }
            set { blt = value; }
        }

        private int[] bltq; // Queue of inactive bullets
        public int[] bulletQueue
        {
            get { return bltq; }
            set { bltq = value; }
        }

        public int head; // Pointers to head and tail of queue
        public int tail;
        protected int bul; // Current bullet

        public Character(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, loadSprite)
        {
            spriteArray = loadSprite; // Bring in all the sprites to draw with
            head = 0;
            tail = 0; // Initialize queue tracers
        }

        public virtual void TakeHit(int dmg) // Using passed damage, calculate new health; add code to lose a life in child class for player
        {
            health = health - dmg;
            color = Color.Red; // Flash red for a frame when hit
        }

        public virtual void Fire() // Fires a bullet
        {
            if (NoBullets) // If all bullets are active (ideally not possible)
            {
                throw new IndexOutOfRangeException(); // If it tries to fire and there are no bullets, throw up
            }

            bul = bulletQueue[head]; // Move the bullet to the character's position
            bullets[bul].spritePos = new Rectangle(spritePos.Center.X, spritePos.Center.Y, bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
            bullets[bul].isActive = true;
            bulletQueue[head] = 69;
            head++;
            if (head>bulletQueue.Length-1)
            {
                head = 0;
            }
        }

        public bool NoBullets
        {
            get
            {
                for (int i = 0; i < bulletQueue.Length; i++)
                {
                    if (bulletQueue[i] != 69)
                    {
                        return false; // If at least one bullet is inactive, say so
                    }
                }
                return true; // Otherwise, say there are no bullets
            }
        }

        public override void Update()
        {
            for (int i = 0; i < bullets.Length; i++) // Update active bullets
            {
                if (bullets[i].isActive)
                {
                    bullets[i].Update();
                    if (!bullets[i].isActive) // If the bullet went out of bounds, add it back to the inactive queue
                    {
                        ReloadBullet(i);
                    }
                }
            }
        }

        public void ReloadBullet(int i) // Adds the index i of a freshly inactive bullet back to the inactive queue and moves the tail
        {
            bulletQueue[tail] = i;
            tail++;
            if (tail > bulletQueue.Length - 1)
            {
                tail = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
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
    }
}
