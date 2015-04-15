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
    // Contains necessary methods and attributes (etc.) for player's character object

    // Change Log
    // T 3/27/15- added key press array holder, added update and code
    // T 3/31/15- added fire code to input system
    // T 4/2/15- fixed fire code to handle if there are no bullets, moved fire code to skyPlayer, moved input down to SkyPlayer update
    // B 4/14/15 - added a stamina attribute and hard coded it to start at 100 
    public abstract class Player:Character
    {
        private int livesLeft; // Lives character has and properties
        private double stamina = 100; // the amount of stamina the character has

        public double Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        }
        public int lives
        {
            get { return livesLeft; }
            set { livesLeft = value; }
        }

        protected KeyboardState kstate; // Hold pressed keys

        protected Rectangle resetPos; // Rectangle to hold position for sprite reset upon death of character
        
        public Player(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, and sprite for character
            : base(X, Y, dir, spd, loadSprite)
        {
            
            cooldown = 0; // Let the player shoot
            charHealth = 3; // Default health
            livesLeft = 3; // Default lives
            resetPos = new Rectangle(X, Y, loadSprite[0].Width, loadSprite[0].Height); // Sets position to return to when player dies
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
