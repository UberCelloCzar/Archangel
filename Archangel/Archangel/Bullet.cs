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
    public abstract class Bullet:MovableGameObject
    {
        private int dealtDamage; // Variable for bullet's damage and properties
        public int damage
        {
            get { return dealtDamage; }
            set { dealtDamage = value; }
        }

        private int facedDirection; // Direction of bullet and properties
        public int direction // up=0,right=1,down=2,left=3
        {
            get { return facedDirection; } 
            set { facedDirection = value; }
        }

        private bool active; 
        public bool isActive
        {
            get { return active; }
            set { active = value; }
        }

        public Bullet(int X, int Y, int dir, Texture2D loadSprite) // Creates bullet at xy with direction for loaded sprite
            : base(X, Y, loadSprite)
        {
            active = false; // Start out inactive
            facedDirection = dir; // Start with specified direction
        }
    }
}
