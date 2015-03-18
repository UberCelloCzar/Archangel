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
    class Sword:MovableGameObject
    {
        private int dealtDamage; // Variable for bullet's damage and properties
        public int damage
        {
            get { return dealtDamage; }
            set { dealtDamage = value; }
        }

        public Sword(int X, int Y, Texture2D loadSprite) // Creates sword at X Y with the loaded sprite
            : base(X, Y, loadSprite)
        {
            
        }
        public abstract void Update() { } // Movement stub
        //Not sure what to do with this yet, I'll think it over on spring break
    }
}
