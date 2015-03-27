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
    public abstract class Character:MovableGameObject
    {
        private int health; // HP for character and properties
        public int charHealth
        {
            get { return health; }
            set { health = value; }
        }

        protected Texture2D[] spriteArray; // I went for protected on this array for all the sprites to avoid any weird errors properties might generate

        public Character(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, loadSprite) // 0 will be the default starting direction image for the chracter
        {
            spriteArray = loadSprite; // Bring in all the sprites to draw with
        }

        public void TakeHit(int dmg) // Using passed damage, calculate new health; add code to lose a life in child class for player
        {
            charHealth -= dmg;
        }
    }
}
