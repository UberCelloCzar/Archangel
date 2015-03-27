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
    // T 3/26/15- removed enumeration, changed initDir to an int
    class SkyPlayer:Player
    {
        private int initDir; // Stores initial direction

        public SkyPlayer(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, direction, and sprite for character
            : base(X, Y, dir, spd, loadSprite)
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
                direction = initDir;
            }
        }

        public override void Update()
        {
            
        }
    }
}
