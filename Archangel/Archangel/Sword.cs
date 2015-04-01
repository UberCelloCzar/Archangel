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
    // Contains necessary methods and code for the functional sword

    // Change Log
    // T 3/31/15- Changed to inherit from gameobject, nulled the sprites
    class Sword:GameObject
    {
        private int dealtDamage; // Variable for bullet's damage and properties
        public int damage
        {
            get { return dealtDamage; }
            set { dealtDamage = value; }
        }

        public Sword(int X, int Y) // Creates sword hitbox at X Y 
            : base(X, Y, null)
        {
            
        }
        public void Draw(SpriteBatch spriteBatch) { } // Draw stub
    }
}
