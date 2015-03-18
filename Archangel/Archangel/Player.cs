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
    public abstract class Player:Character
    {
        private int livesLeft; // Lives character has and properties
        public int lives
        {
            get { return livesLeft; }
            set { livesLeft = value; }
        }

        protected Rectangle resetPos; // Rectangle to hold position for sprite reset upon death of character
        
        public Player(int X, int Y, Texture2D[] loadSprite) // Sets x,y, and sprite for character
            : base(X, Y, loadSprite)
        {
            charHealth = 3; // Default health
            livesLeft = 3; // Default lives
            resetPos = new Rectangle(X, Y, loadSprite[0].Width, loadSprite[0].Height); // Sets position to return to when player dies
        }
    }
}
