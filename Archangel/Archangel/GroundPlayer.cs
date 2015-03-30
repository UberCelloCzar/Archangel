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
    // T 3/229/15- fixed constuctor, removed enumeration
    class GroundPlayer:Player
    {

        public GroundPlayer(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, and sprite for character
            : base(X, Y, dir, spd, loadSprite) { }

        public override void TakeHit(int dmg)
        {
            base.TakeHit(dmg);

            if (charHealth <= 0)
            {
                lives--; // Take away a life
                charHealth = 3; // Reset health and position and direction
                spritePos = resetPos;
                direction = 5;
            }
        }

        public override void Update()
        {

        }
    }
}
