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
    // Cheshire Games, Bierre, March 31, 2015
    // Contains the logic for top level collisions, draw, and update functions

    // Change Log
    class GameLogic
    {
        public void Update(SkyPlayer skyPlayer, List<Enemy> enemies, HeadsUpDisplay hud) // Inputs and movement calls
        {

            Collisions(skyPlayer, enemies); // Go-go collision detect!
        }

        public void Collisions(SkyPlayer skyPlayer, List<Enemy> enemies) // Handles all collision detection
        {

        }

        public void Draw(SpriteBatch spriteBatch, SkyPlayer skyPlayer, List<Enemy> enemies, HeadsUpDisplay hud) // Draw calls and logic
        {

        }
    }
}
