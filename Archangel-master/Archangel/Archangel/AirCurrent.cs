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
using System.Threading;

namespace Archangel
{
    // Cheshire Games, Bierre, May 4, 2015
    // The ari current the player dashes off of
    public class AirCurrent: MovableGameObject
    {
        private int frame; // Is the dash out
        public int dashFrame
        {
            get { return frame; }
            set { frame = value; }
        }

        public AirCurrent(int X, int Y, int dir, int spd, Texture2D[] loadSprite)
            :base(X, Y, dir, spd, loadSprite)
        {
            frame = 0; // Initially inactive
        }

        public override void Update()
        {
            if (frame == 120)
            {
                frame = 0; // Turn off the dash
            }
            else if (frame > 0) // Move the dash current
            {
                switch (direction) // Move based on the direction
                {
                    case 0: spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spriteImg.Width, spriteImg.Height); // Right
                        break;
                    case 1: spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spriteImg.Width, spriteImg.Height); // Left
                        break;
                    case 2: spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spriteImg.Width, spriteImg.Height); // Up
                        break;
                    case 3: spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spriteImg.Width, spriteImg.Height); // Down
                        break;
                }

                if (spritePos.X + spritePos.Width < 0 || spritePos.X >= Game1.clientWidth || spritePos.Y + spritePos.Height < 0 || spritePos.Y >= Game1.clientHeight) // Check to see if off the edge
                {
                    frame = 0; // The spritePos width and height are usable here because we just changed them to the correct values
                }
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (dashFrame > 0) // Draw the dash current
            {
                spritePos = new Rectangle(spritePos.X, spritePos.Y, spriteImg.Width, spriteImg.Height);
                spriteBatch.Draw(spriteImg, spritePos, color);
                frame++; // Move to the next frame
            }
        }
    }
}
