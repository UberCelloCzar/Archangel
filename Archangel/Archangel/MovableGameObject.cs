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
    // Cheshire Games, Bierre, March 13, 2015
    // MoveableGameObject defines properties for children such as Bullet, Character, and Sword

    // Change Log
    public abstract class MovableGameObject:GameObject
    {
        private int speed; // Variable for object speed and properties
        public int objSpeed
        {
            get { return speed; }
            set { speed = value; }
        }

        public MovableGameObject(int X, int Y, Texture2D loadSprite) // Sets x,y,and sprite for object
            : base(X, Y, loadSprite){}

        public abstract void Update() { } // Requires a movement method for all children
    }
}
