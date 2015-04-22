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
    // Contains all the code for the heads up display

    // Change Log
    // T 4/2/15- Moved around parameters and player.health so health updates with game
    // B 4/14/15 - Changed code to allow the stamina to decrease on the screen

    public class HeadsUpDisplay
    {
        // attributes
        SpriteBatch spriteBatch;
        SpriteFont mainfont;
        Vector2 vect2;
        int health = 100; //(pass in player object) health = player.charHealth
        double stamina = 100; // player.charStamina
        string skyesays;
        int thought = 4;
        int linenum;
        Player player;
        int skyefrequency = 0; // frequency of how often she talks

        // properties
        public int Thought
        {
            get { return thought; }
            set
            {
                thought = value;
                SkyeThink();
            }
        }

        public string Skyesays
        {
            set
            {
                skyesays = value;
            }
        }
        public int Skyfrequency
        {
            get
            {
                return skyefrequency;
            }
            set { skyefrequency = value; }
        }

        public HeadsUpDisplay()
        {
            SkyeThink();
        }


        public void DrawHUD(SpriteBatch sb, SpriteFont sf, Player skyPlayer)
        {
            player = skyPlayer;
            health = player.charHealth; // I moved these here so player health changes on the HUD when it changes in the game
            stamina = player.Stamina;
            stamina = Math.Truncate(stamina);

            spriteBatch = sb;
            mainfont = sf;
            spriteBatch.DrawString(mainfont, "HP    " + health, new Vector2(10, 9), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Score: " + skyPlayer.score, new Vector2(725, 9), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "END " + stamina, new Vector2(10, 31), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Lives: " + skyPlayer.lives, new Vector2(10, 53), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(10, 73), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(290, 453), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0); // bottom of screen example
            // left, neutral, plus .01f scale
            spriteBatch.DrawString(mainfont, "HP    " + health, new Vector2(9, 10), Color.Black, 0, vect2, .79f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Score: " + player.score, new Vector2(724, 10), Color.Black, 0, vect2, .79f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "END " + stamina, new Vector2(9, 30), Color.Black, 0, vect2, .79f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Lives: " + skyPlayer.lives, new Vector2(10, 53), Color.Black, 0, vect2, .76f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(9, 72), Color.Black, 0, vect2, .755f, SpriteEffects.None, 0);
            // 2 left, neutral
            spriteBatch.DrawString(mainfont, "HP    " + health, new Vector2(8, 10), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Score: " + player.score, new Vector2(723, 10), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "END " + stamina, new Vector2(8, 30), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Lives: " + skyPlayer.lives, new Vector2(8, 53), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(8, 72), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            // 2 left, 2 up
            spriteBatch.DrawString(mainfont, "HP    " + health, new Vector2(8, 8), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Score: " + player.score, new Vector2(723, 8), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "END " + stamina, new Vector2(8, 28), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Lives: " + skyPlayer.lives, new Vector2(8, 51), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(8, 74), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            // base, 2 up
            spriteBatch.DrawString(mainfont, "HP    " + health, new Vector2(10, 8), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Score: " + player.score, new Vector2(725, 8), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "END " + stamina, new Vector2(10, 28), Color.Black, 0, vect2, .78f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Lives: " + skyPlayer.lives, new Vector2(10, 51), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(10, 70), Color.Black, 0, vect2, .75f, SpriteEffects.None, 0);

            // white inline
            spriteBatch.DrawString(mainfont, "HP    " + health, new Vector2(9, 10), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Score: " + player.score, new Vector2(724, 10), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "END " + stamina, new Vector2(9, 30), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Lives: " + skyPlayer.lives, new Vector2(9, 52), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "HP    " + health, new Vector2(10, 10), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Score: " + player.score, new Vector2(725, 10), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "END " + stamina, new Vector2(10, 30), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Lives: " + skyPlayer.lives, new Vector2(10, 52), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(10, 72), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mainfont, "Skye: " + SkyeTalk(), new Vector2(9, 73), Color.White, 0, vect2, .75f, SpriteEffects.None, 0);
        }

        // HOW IT ALL WORKS:
        /* In order to change what Skye says, we must first change her mindset. Any time something happens that could change her thoughts,
         * change the thought variable to reflect what she should think. Change it to 1 for tips, 2 for story, 3 for taunts, and 4 for 
         * funny/functional lines.
        */
        // Skye Talk method to get a phrase from the SkyeLines file
        public string SkyeTalk()
        {
            // Text file Table of Contents:
            // Tips: 0 to 9
            // Story bits: 10 to 19
            // Enemy taunts: 20 to 29
            // Funny lines: 30 to 39
            try
            {
                StreamReader input = new StreamReader("SkyeLines.txt");
                string line = "";
                for (int lineread = 0; lineread < 40; lineread++)
                {
                    line = input.ReadLine();
                    if (lineread == this.linenum)
                    {
                        skyesays = line;
                        if (line == "")
                        {
                            skyesays = "...I've got nothing.";
                            input.Close();
                            return skyesays;
                        }
                        input.Close();
                        return skyesays;
                    }
                }
                skyesays = "I was going to say something...";
                input.Close();
                return skyesays;
            }
            catch (FileNotFoundException fne)
            {
                skyesays = "I'm drawing a blank here...";
                return skyesays;
            }
            catch (IOException ioe)
            {
                skyesays = "Is it weird that I forgot how to read?";
                return skyesays;
            }
        }

        // SkyeThink method to determine what she should say
        public void SkyeThink()
        {
            Random rand = new Random();
            switch (thought)
            {
                case 1://tips
                    {
                        this.linenum = rand.Next(0, 10);
                        break;
                    }
                case 2://story
                    {
                        linenum = rand.Next(10, 20);
                        break;
                    }
                case 3://taunts
                    {
                        linenum = rand.Next(20, 30);
                        break;
                    }
                case 4://funny/functional line
                    {
                        linenum = rand.Next(30, 40);
                        break;
                    }
                default: linenum = 9999; break;
            }
        }
    }
}
