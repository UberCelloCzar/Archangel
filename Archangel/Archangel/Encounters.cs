﻿using System;
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
        private List<Enemy> enemyList;
        public List<Enemy> enemies
        {
            get { return enemyList; }
            set { enemyList = value; }
        }

        SkyPlayer player;
        bool skirmishOver;

        // constructor
        public Encounters(SkyPlayer player)
        {
            enemies = new List<Enemy>();
            this.player = player;
        }

        // create enemies
        private void CreateEnemy(string enemyinfo, Texture2D [] enemysprites, Texture2D [] bulletsprites, HeadsUpDisplay hud)
        {
            int y = 0;
            int[] elem = new int[4];
            foreach (string enemyStr in enemyinfo.Split(','))
            {
                try
                {
                    elem[y] = int.Parse(enemyStr);
                    int enemyXpos = elem[0];
                    int enemyYpos = elem[1];
                    int enemyState = elem[2];
                    int enemySpeed = elem[3];
                    if (y == 2)
                    {
                        enemies.Add(new Enemy(enemyXpos, enemyYpos, enemyState, enemySpeed, enemysprites, bulletsprites));
                    }
                    y = y + 1;
                }
                catch (IndexOutOfRangeException ioe)
                {
                    hud.Skyesays = "Their formation is off...";
                }
                catch (FormatException foe)
                {
                    hud.Skyesays = "...Huh, no enemies, I guess. Sweet!";
                }
            }
        }

        // read encounter layout
        public void ReadEncounter(Texture2D[] enemysprites, Texture2D[] bulletsprites, HeadsUpDisplay hud)
        {
            try
            {
                // create Streamreader and read in random encounter file
                Random rand = new Random();
                string file = "encounter" + rand.Next(1, 2) + ".txt"; // increase upper bound as more encounters are made
                StreamReader input = new StreamReader(file);

                string freqline = input.ReadLine(); // used to determine how often platforms appear. The lower the number, the more frequent. Lowest = 2

                freqline = input.ReadLine();
                // pass that into the platform spawn method here

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
    }
}
