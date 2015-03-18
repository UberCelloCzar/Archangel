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

    class Enemy:Character
    {
        public enum CharState { faceLeft, moveLeft, faceRight, moveRight, faceUp, moveUp, faceDown, moveDown, dead } // Enumeration for movement and sprite updates, public until I remember a better protection method
        private CharState charState; // One state for each sprite (one per frame for animations)

        public Enemy(int X, int Y, CharState dir, Texture2D[] loadSprite) // Sets x,y, direction, and sprite for character
            : base (X, Y, loadSprite)
        {
            charState = dir; // Initial direction
            charHealth = 2; // Enemy health
        }

        public override void TakeHit(int dmg) // Enemy takes a hit and possibly dies
        {
            base.TakeHit(dmg);

            if (charHealth <= 0) // Deactivate enemy when killed
            {
                charState = CharState.dead; // Changes state to show death sprite, actual disappearance is in update method with finite state machine logic
            }
        }

        public override void Update()
        {
            
        }
    }
}
