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
    // Contains code for the "platforms", mostly to be fleshed out later as part of the turret movement switch

    // Change Log
    // B 4/14/15 - Added code for platforms

    public class Platform:MovableGameObject
    {
        public Platform(int X, int Y, int dir, int spd, Texture2D[] loadSprite)
            : base(X, Y, dir, spd, loadSprite)
        {
            
            
        }
        public override void Update()
        {
            throw new NotImplementedException();
        }
        public override void Draw(SpriteBatch spriteBatch) 
        {
            base.Draw(spriteBatch);
        }
    }
}
