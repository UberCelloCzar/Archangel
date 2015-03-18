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
    // Contains necessary code and methods for the player object while flying through the air

    // Change Log
    class SkyPlayer:Player
    {
        public enum CharState { faceRight, moveRight, faceLeft, moveLeft, faceUp, moveUp, faceDown, moveDown, slash1, charge1, dead } // Enumeration for movement and sprite updates, public until I remember a better protection method
        private CharState charState; // One state for each sprite (one per frame for animations)
        private CharState initDir; // Holds initial direction of player

        public SkyPlayer(int X, int Y, CharState dir, Texture2D[] loadSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, loadSprite)
        {
            initDir = dir; // Sets direction to return to upon death
        }

        public override void TakeHit(int dmg)
        {
            base.TakeHit(dmg);

            if (charHealth <= 0)
            {
                lives--; // Take away a life
                charHealth = 3; // Reset health and position and direction
                spritePos = resetPos;
                charState = initDir;
            }
        }

        public override void Update()
        {
            
        }
    }
}
