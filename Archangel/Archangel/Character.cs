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

        protected Texture2D[] spriteArray; // I went for protected on this array for all the sprites to avoid any weird errors properties might generate

        public Character(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, loadSprite)
        {
            spriteArray = loadSprite; // Bring in all the sprites to draw with
        }

        public virtual void TakeHit(int dmg) // Using passed damage, calculate new health; add code to lose a life in child class for player
        {
            charHealth -= dmg;
            color = Color.Red; // Flash red for a frame when hit
        }

        public void Fire() // Fires a bullet
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
                    throw new IndexOutOfRangeException(); // If it tries to fire and there are no bullets, throw up
                }

            }
            bullets[i].direction = direction; // Move the bullet to the character's position and direction (middle of the character sprite)
            bullets[i].spritePos = new Rectangle(spriteArray[direction].Bounds.Left + spriteArray[direction].Width / 2, spriteArray[direction].Bounds.Top + spriteArray[direction].Height / 2, spriteArray[direction].Width, spriteArray[direction].Height);
            bullets[i].isActive = true;
        }

        public override void Update()
        {
            for (int i = 0; i < bullets.Length; i++) // Update active bullets
            {
                if (bullets[i].isActive)
                {
                    bullets[i].Update();
                }
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
