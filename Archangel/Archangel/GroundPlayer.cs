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
    // Contains methods and code for the player object when landed on ground and in turret control mode

    // Change Log
    class GroundPlayer:Player
    {
        public enum CharState { faceUp, moveRight1, moveLeft1, fire1, dead } // Enumeration for movement and sprite updates, public until I remember a better protection method
        private CharState charState; // One state for each sprite (one per frame for animations)

        public GroundPlayer(int X, int Y, Texture2D[] loadSprite) // Sets x,y, and sprite for character
            : base(X, Y, loadSprite) { }

        public override void TakeHit(int dmg)
        {
            base.TakeHit(dmg);

            if (charHealth <= 0)
            {
                lives--; // Take away a life
                charHealth = 3; // Reset health and position and direction
                spritePos = resetPos;
                charState = CharState.faceUp;
            }
        }

        public override void Update()
        {

        }
    }
}
