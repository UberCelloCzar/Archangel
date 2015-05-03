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
        // attributes
        bool active = false;
        int frequency = 0;
        Player player;
        Texture2D[] platforms;
        int delay = 0;
        int lifetime = 1200; // 20 seconds after player lands on it
        bool decay = false;
        int shake = 1;

        //properties
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public Platform(int X, int Y, int dir, int spd, Texture2D[] loadSprite, int frq, Player play)
            : base(X, Y, dir, spd, loadSprite)
        {
            frequency = frq;
            player = play;
            platforms = loadSprite;
        }
        public override void Update()
        {
            delay++;
            if (delay >= 60)
            {
                Random rand = new Random();
                //int spawnDeterminant = ((int)Math.Round(player.Stamina) / 10) - frequency;
                int spawnDeterminant = ((int)Math.Round(player.Stamina)) - frequency;

                if (spawnDeterminant < 2)
                { spawnDeterminant = 2; }

                if (rand.Next(1, spawnDeterminant) == 1)
                {
                    active = true;
                }
                delay = 0;
            }

            if (active == true && player.OnPlatform != true && lifetime > 0)
            {
                spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
            }
            if (active == true && player.OnPlatform == true)
            {
                decay = true;
            }

            if(decay == true)
            {
                lifetime--;
            }

            if(lifetime <= 180 && lifetime > 0)
            {
                if (shake == 1)
                {
                    spritePos = new Rectangle(spritePos.X, spritePos.Y + 3, spritePos.Width, spritePos.Height);
                    shake = 0;
                }
                else
                {
                    spritePos = new Rectangle(spritePos.X, spritePos.Y - 3, spritePos.Width, spritePos.Height);
                    shake = 1;
                }
            }

            if(lifetime <= 0)
            {
                player.OnPlatform = false;
                spritePos = new Rectangle(spritePos.X, spritePos.Y + 4, spritePos.Width, spritePos.Height);
            }

            if (spritePos.Right <= 0 || spritePos.Y > 1200)
            {
                decay = false;
                lifetime = 1200;
                active = false;
                spritePos = new Rectangle(1920, 880, spritePos.Width, spritePos.Height);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(platforms[0], spritePos, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
