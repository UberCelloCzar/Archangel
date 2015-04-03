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
    // Contains all things necessary for child classes defined by bullet type

    // Change Log
    // T 3/25/15- added update method and logic, changed constructor to accept texture array, removed abstract keyword
    // T 3/26/15- added draw and fixed cnstructor to accept speed and damage
    // T 3/31/15- added old position variable for raycasting
    // T 4/2/15- removed all raycasting tools (bullet is too big for that and too slow for it to matter)
    public class Bullet:MovableGameObject
    {
        private int dealtDamage; // Variable for bullet's damage and properties
        public int damage
        {
            get { return dealtDamage; }
            set { dealtDamage = value; }
        }

        private bool active;
        public bool isActive
        {
            get { return active; }
            set { active = value; }
        }

        public Bullet(int X, int Y, int dir, int spd, int dmg, Texture2D[] loadSprite) // Creates bullet at xy with direction for loaded sprite
            : base(X, Y, dir, spd, loadSprite)
        {
            active = false; // Start out inactive
        }

        public override void Update() // Moves bullet in faced direction
        {
            switch (direction) // Move based on the direction
            {
                case 0: spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spriteArray[0].Width, spriteArray[0].Height); // Right
                        break;
                case 1: spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spriteArray[1].Width, spriteArray[1].Height); // Left
                        break;
                case 2: spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spriteArray[2].Width, spriteArray[2].Height); // Up
                        break;
                case 3: spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spriteArray[3].Width, spriteArray[3].Height); // Down
                        break;
            }

            if (spritePos.X + spritePos.Width < 0 || spritePos.X >= Game1.clientBounds.Width || spritePos.Y + spritePos.Height < 0 || spritePos.Y >= Game1.clientBounds.Height) // Check to see if off the edge
            {
                isActive = false; // The spritePos width and height are usable here because we just changed them to the correct values
            }
        }

        public override void Draw(SpriteBatch spriteBatch) // Draws the bullet
        {
            base.Draw(spriteBatch);
        }
    }
}
