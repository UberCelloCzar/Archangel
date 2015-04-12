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
    // B 4/12/15 - Created code for platforms
    public class Platform:GameObject
    {
        public Texture2D platformSprite;

        public Texture2D PlatformSprite
        {
            get { return platformSprite; }
            set { platformSprite = value; }
        }

        public Platform(int X, int Y, Texture2D loadSprite)
            : base(X, Y, loadSprite)
        {
            platformSprite = loadSprite;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
