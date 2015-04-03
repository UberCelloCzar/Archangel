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
    public abstract class Player:Character
    {
        private int livesLeft; // Lives character has and properties
        public int lives
        {
            get { return livesLeft; }
            set { livesLeft = value; }
        }

        protected KeyboardState kstate = Keyboard.GetState(); // Hold pressed keys

        protected Rectangle resetPos; // Rectangle to hold position for sprite reset upon death of character
        
        public Player(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y, and sprite for character
            : base(X, Y, dir, spd, loadSprite)
        {
            cooldown = 0; // Let the player shoot
            charHealth = 3; // Default health
            livesLeft = 3; // Default lives
            resetPos = new Rectangle(X, Y, loadSprite[0].Width, loadSprite[0].Height); // Sets position to return to when player dies
        }

        public override void Update() // This handles the input, and since it can be called using base.Update it eliminates the need for an input class
        {
            Keys[] pressedKeys = kstate.GetPressedKeys(); // Get pressed keys

            if (direction == 1 && !kstate.IsKeyDown(Keys.Right)) // If direction is moveRight and right key is not down, revert to facing right
            {
                direction = 0;
            }
            else if (kstate.IsKeyDown(Keys.Right) && direction == 0) // Move right on second press 
            {
                direction = 1;
            }
            else if (kstate.IsKeyDown(Keys.Right)) // Face right on first press
            {
                direction = 0;
            }

            if (direction == 3 && !kstate.IsKeyDown(Keys.Left)) // If direction is moveLeft and left key is not pressed, revert to facing left
            {
                direction = 2;
            }
            else if (kstate.IsKeyDown(Keys.Left) && direction == 2) // Move left on second press
            {
                direction = 3;
            }
            else if (kstate.IsKeyDown(Keys.Left)) // Face left on first press
            {
                direction = 2;
            }

            if (direction == 5 && !kstate.IsKeyDown(Keys.Up)) // If direction is moveUp and up key is not down, revert to facing up
            {
                direction = 4;
            }
            else if (kstate.IsKeyDown(Keys.Up) && direction == 4) // Move up on second press
            {
                direction = 5;
            }
            else if (kstate.IsKeyDown(Keys.Up)) // Face up on first press
            {
                direction = 4;
            }

            if (direction == 7 && !kstate.IsKeyDown(Keys.Down)) // If direction is moveDown and down key is not pressed, revert to facing down
            {
                direction = 6;
            }
            else if (kstate.IsKeyDown(Keys.Down) && direction == 6) // Move down on second press
            {
                direction = 7;
            }
            else if (kstate.IsKeyDown(Keys.Down)) // Face down on first press
            {
                direction = 6;
            }

            if (kstate.IsKeyDown(Keys.Space) && cooldown == 0) // Fire, then go into cooldown
            {
                Fire();
                cooldown = 10; // Start the cooldown
            }

            cooldown--; // Increment cooldown to allow firing again
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
