using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;

namespace Archangel
{
    // Cheshire Games, Bierre, March 31, 2015
    // Contains code to read in maps and creates enemies

    // Change Log
    // T 3/31/15- changed enemyList into a private with properties
    // T 4/2/15- changed parameters and calls to use the Game1 hud object
    class Encounters
    {
        // attributes
        private List<Enemy> enemyList = new List<Enemy>();
        Player player;
        int platFrequency;
        double blessedNum = 1; // lowest = 1
        public List<Enemy> enemies
        {
            get { return enemyList; }
            set { enemyList = value; }
        }

        bool skirmishOver;

        // constructor
        public Encounters(Player player)
        {
            enemies = new List<Enemy>();
            this.player = player;
        }

        // create enemies
        private void CreateEnemy(string enemyinfo, Texture2D [] enemysprites, Texture2D [] bulletsprites, HeadsUpDisplay hud)
        {
            int y = 0;
            string[] info = new string[4];
            int[] elem = new int[4];
            info = enemyinfo.Split(',');
            for (int i = 0; i < 4; i++)
            {
                int.TryParse(info[i], out elem[i]);
            }
            int bn = (int)blessedNum;
            int blessType = 0;
            if (bn >= 1)
            {
                Random rand = new Random();
                blessType = rand.Next(0, 5);
                blessedNum--;
            }
            enemies.Add(new Enemy(elem[0], elem[1], elem[2], elem[3], enemysprites, bulletsprites, hud, player, blessType));
        }

        // read encounter layout
        public void ReadEncounter(Texture2D[] enemysprites, Texture2D[] bulletsprites, HeadsUpDisplay hud)
        {
            try
            {
                // create Streamreader and read in random encounter file
                Random rand = new Random();
                string file = "encounter" + rand.Next(1, 6) + ".txt"; // increase upper bound as more encounters are made
                StreamReader input = new StreamReader(file);
                string freqline = input.ReadLine(); // used to determine how often platforms appear. The lower the number, the more frequent. Lowest = 2
                platFrequency = int.Parse(freqline);

                // check score before making enemies
                blessedNum = player.score / 800;
                blessedNum = Math.Round(blessedNum);

                string line = "";
                while ((line = input.ReadLine()) != null) // read enemy data
                {
                    CreateEnemy(line, enemysprites, bulletsprites, hud);
                }
            }
            catch (IOException ioe)
            {
                hud.Skyesays = "...Huh, no enemies, I guess. Awesome!";
            }
        }

        // manage platform spawning
        public Platform PlatformSpawn(Texture2D platformsprite) // this seemes unnecessary
        {
            Texture2D[] platforms = new Texture2D[1]; // if more platforms are made, add them to this array
            platforms[0] = platformsprite;
            Platform platform1 = new Platform(1920, 880, 0, 3, platforms, platFrequency, player);
            return platform1;
        }
    }
}
