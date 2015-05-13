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
    // Cheshire Games, Bierre, March 16, 2015
    // Contains all methods and code for enemies and enemy AI

    // Change Log
    // T 3/28/15- fixed constructor, added draw and update code
    // T 3/30/15- added fire method and updated constructor for it
    // T 3/31/15- added fire code to Update
    // T 4/2/15- moved fire method to Character, added death timer and mechanic
    // T 4/7/15- readded fire method to move bullet to gun's position and character's direction on firing
    // T 4/28/15- fixed gun positions
    // T 5/4/15- fixed draw and gun positions for new sprites
    // T 5/13/15- fixed enemies hitting the platform
    public class Enemy:Character
    {
        private int deadTime; // Timer for how long enemy is drawn as dead before disappearing
        HeadsUpDisplay hudref;
        Player player;
        int initialX;
        int initialY;
        int move;
        int leftOrRight = 0;
        int upOrDown = 0;
        int pointX;
        int pointY;
        int blessing;
        Color blessingColor = Color.White;
        int artilleryCDR = 0;
        bool shielded = false;
        int swiftDI = 0;
        bool archangel = false;
        int archMoveCD = 600;
        int oldMD;
        int moveDecision;
        bool pointOverride; // Overrides the point setting so enemies don't glitch out

        public int deathTimer
        {
            get { return deadTime; }
        }
        public bool Shielded
        {
            get { return shielded; }
            set { shielded = value; }
        }

        public Color BlessingColor
        {
            set { blessingColor = value; }
        }

        public int Blessing
        {
            get { return blessing; }
        }
        public bool Archangel
        {
            get { return archangel; }
            set { archangel = value; }
        }

        public Enemy(int X, int Y, int dir, int spd, Texture2D[] loadSprite, Texture2D[] bulletSprite, HeadsUpDisplay hud, Player player, int bless, bool arch) // Sets x,y, direction, and sprite for character
            : base (X, Y, 0, spd, loadSprite)
        {
            archangel = arch;
            blessing = bless;
            initialX = X;
            initialY = Y;
            hudref = hud;
            this.player = player;
            bullets = new Bullet[50]; // Initialize bullet array
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Bullet(0, 0, 0, 10, 1, bulletSprite);
            }

            bulletQueue = new int[50];
            for (int i = 0; i < bullets.Length; i++)
            {
                bulletQueue[i] = i; // Initialize the queue
            }

            cooldown = 2; // Don't let them fire immediately
            direction = dir; // Initial direction
            charHealth = 3; // Enemy health
            if (archangel != true)
            {
                switch (blessing)
                {
                    case 0: // Standard enemy with no blessing
                        blessingColor = Color.White;
                        break;
                    case 1: // Gifted enemy with more health
                        blessingColor = Color.DarkGray;
                        charHealth = 5;
                        break;
                    case 2: // Swift enemy with more speed and strafe range
                        blessingColor = Color.PaleTurquoise;
                        this.objSpeed = 5;
                        swiftDI = 30;
                        charHealth = 2;
                        break;
                    case 3: // Shielded enemy
                        blessingColor = Color.LightCoral;
                        charHealth = 1;
                        shielded = true;
                        break;
                    case 4: // Artillery enemy with less bullet cooldown
                        blessingColor = Color.GreenYellow;
                        artilleryCDR = 40;
                        break;
                }
            }
            if (archangel == true)
            {
                charHealth = 10;
                artilleryCDR = 20;
                this.objSpeed = 4;
                swiftDI = 60;
                blessingColor = Color.Yellow;
                initialX = 600;
                initialY = 300;
            }
        }

        public override void TakeHit(int dmg) // Enemy takes a hit and possibly dies
        {
            if (shielded == false)
            {
                base.TakeHit(dmg);

                if (charHealth <= 0) // Deactivate enemy when killed
                {
                    direction = 8; // Changes state to show death sprite, disappearance is after takehit method is called in calling class
                    hudref.Skyfrequency++;
                    player.score = player.score + 100;
                    if (hudref.Skyfrequency == 0)
                    {
                        hudref.Thought = 1;
                        hudref.SkyeTalk();
                    }
                    if (hudref.Skyfrequency == 3 || hudref.Skyfrequency == 6)
                    {
                        hudref.Thought = 3;
                        hudref.SkyeTalk();
                    }
                    if (hudref.Skyfrequency == 9)
                    {
                        Random rand = new Random();
                        hudref.Thought = 2;
                        hudref.SkyeTalk();
                    }
                    if (hudref.Skyfrequency == 10)
                    {
                        hudref.Skyfrequency = 0;
                    }
                }
            }
        }

        public override void Update()
        {
            base.Update();
            Random rand2 = new Random();
            Random rand1 = new Random(Guid.NewGuid().GetHashCode()); // Different seed for more randomness

            if (direction == 8)
            {
                deadTime++;
                return; // Don't let the enemy move while dead
            }
            if(player.OnPlatform == false && pointOverride == false)
            {
                pointX = initialX;
                pointY = initialY;
            }
            else if (pointOverride == true)
            {
                if ((spritePos.X > pointX - 10 && spritePos.X < pointX + 10) && (spritePos.Y > pointY - 10 && spritePos.Y < pointY + 10))
                {
                    pointOverride = false;
                }
                else if (spritePos.X > pointX)
                {
                    move = 1; // Left
                }
                else if (spritePos.X < pointX)
                {
                    move = 3; // Right
                }
                else if (spritePos.Y > pointY)
                {
                    move = 5; // Up
                }
                else
                {
                    move = 7; // Down
                }
            }
            else
            {
                if (player.direction == 0 || player.direction == 1)
                {
                    pointX = player.spritePos.X + player.spritePos.Width;
                }
                else
                {
                    pointX = player.spritePos.X - player.spritePos.Width;
                }
                if (this.spritePos.Y <= player.spritePos.Y + player.spritePos.Height)
                {
                    pointY = player.spritePos.Y + (player.spritePos.Height * 2);
                }
            }

            if (player.spritePos.X > this.spritePos.X)
            {
                direction = 3; //right
                
                if (player.spritePos.Y > this.spritePos.Y && (player.spritePos.Center.X > this.spritePos.X && player.spritePos.Center.X < this.spritePos.X + player.spritePos.Width))
                {
                    direction = 7; //down
                }
                if (player.spritePos.Y < this.spritePos.Y && (player.spritePos.Center.X > this.spritePos.X && player.spritePos.Center.X < this.spritePos.X + player.spritePos.Width))
                {
                    direction = 5; // up
                }
            }
            else
            {
                direction = 1; // left

                if (player.spritePos.Y > this.spritePos.Y && (player.spritePos.Center.X > this.spritePos.X && player.spritePos.Center.X < this.spritePos.X + player.spritePos.Width))
                {
                    direction = 7; // down
                }
                if (player.spritePos.Y < this.spritePos.Y && (player.spritePos.Center.X > this.spritePos.X && player.spritePos.Center.X < this.spritePos.X + player.spritePos.Width))
                {
                    direction = 5; // up
                }
            }

            // determine movement
            if (archangel == true && archMoveCD <= 0 && pointOverride == false)
            {
                oldMD = moveDecision;
                moveDecision = rand1.Next(1,5);
                if (moveDecision == oldMD)
                {
                    moveDecision = rand1.Next(1,5);
                }
                switch (moveDecision)
                {
                    case 1:
                        initialX = 600;
                        initialY = 100;
                        break;
                    case 2:
                        initialX = 1000;
                        initialY = 750;
                        break;
                    case 3:
                        initialX = 800;
                        initialY = 425;
                        break;
                    case 4:
                        initialX = 600;
                        initialY = 750;
                        break;
                    case 5:
                        initialX = 1000;
                        initialY = 100;
                        break;
                }
                archMoveCD = 480;
            }
            else if (pointOverride == false)
            {
                archMoveCD--;
            }
            if (player.direction == 4 || player.direction == 5 || player.direction == 6 || player.direction == 7 || player.direction == 16 || player.OnPlatform == true) // if player is faced/moving up or down
            {
                if (pointOverride == false)
                {
                    if (this.spritePos.X - pointX < 120 + swiftDI && this.spritePos.X + this.spritePos.Width < Game1.clientWidth && leftOrRight == 0)
                    {
                        move = 3; // right
                    }
                    else
                    {
                        leftOrRight = 1;
                        move = 1; // left
                        if (this.spritePos.X - pointX <= 0)
                        {
                            leftOrRight = 0;
                        }
                    }
                }
            }
            if (player.direction == 0 || player.direction == 1 || player.direction == 2 || player.direction == 3) // if player if faced/moving left or right
            {
                if (pointOverride == false)
                {
                    if (this.spritePos.Y - pointY < 120 + swiftDI && this.spritePos.Y + (this.spritePos.Height * 2) < Game1.clientHeight && upOrDown == 0)
                    {
                        move = 7; // down
                    }
                    else
                    {
                        upOrDown = 1;
                        move = 5; // up
                        if (this.spritePos.Y - pointY <= 0)
                        {
                            upOrDown = 0;
                        }
                    }
                }
            }
            
            switch (move) // Move the sprites
            {
                case 1: // Move left
                    spritePos = new Rectangle(spritePos.X - objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                    break;
                case 3: // Moving right
                    spritePos = new Rectangle(spritePos.X + objSpeed, spritePos.Y, spritePos.Width, spritePos.Height);
                    break;
                case 5: // Moving up
                    spritePos = new Rectangle(spritePos.X, spritePos.Y - objSpeed, spritePos.Width, spritePos.Height);
                    break;
                case 7: // Move down
                    spritePos = new Rectangle(spritePos.X, spritePos.Y + objSpeed, spritePos.Width, spritePos.Height);
                    break;
                case 8: // Dead
                    spritePos = new Rectangle(spritePos.X, spritePos.Y, spritePos.Width, spritePos.Height);
                    break;
            }

            if (cooldown <= 0)
            {
                
                try
                {
                    double difficultyTweak = 3 + (player.score / 1000); // for every 1000 points or 10 enemies defeated, each enemy fires more often
                    difficultyTweak = Math.Floor(difficultyTweak);
                    int dt = (int)difficultyTweak;
                    Random rand = new Random();

                    if (rand.Next(1, dt) != dt - 1) // difficulty factor: Given the opportunity, they will shoot (dt-1)/dt% of the time
                    {
                        Fire();
                    }
                }
                catch (IndexOutOfRangeException noBullets)
                {
                    throw new IndexOutOfRangeException(); // If it tries to fire and there are no bullets, throw up further
                }
                cooldown = 80 - artilleryCDR; // Go into cooldown
            }

            cooldown--; // Countdown to fire again

            if (spritePos.Intersects(player.Platform.spritePos))
            {
                switch (move)
                {
                    case 1:
                        spritePos = new Rectangle(player.Platform.spritePos.Left - spritePos.Width, spritePos.Y, spritePos.Width, spritePos.Height); // Collides with left side
                        pointY = spritePos.Y - 500;
                        break;
                    case 3:
                        spritePos = new Rectangle(player.Platform.spritePos.Right, spritePos.Y, spritePos.Width, spritePos.Height); // Collides with right side
                        pointY = spritePos.Y - 500;
                        break;
                    case 5:
                        spritePos = new Rectangle(spritePos.X, player.Platform.spritePos.Bottom, spritePos.Width, spritePos.Height); // Collides with bottom
                        initialX = spritePos.X + 700;
                        break;
                    default: 
                        spritePos = new Rectangle(spritePos.X, player.Platform.spritePos.Top - spritePos.Height, spritePos.Width, spritePos.Height); // Collides with top
                        initialY = spritePos.Y - 500;
                        break;
                }
                pointOverride = true; // Override original destination
            }
        }

        public override void Draw(SpriteBatch spriteBatch) // Draws the ememies
        {
            switch (direction) // Draw the Enemies
            {
                case 0: case 1:
                    spriteBatch.Draw(spriteArray[0], new Rectangle(spritePos.X, spritePos.Y, spriteArray[0].Width, spriteArray[0].Height), color); // Left
                    break;
                case 2: case 3:
                    spriteBatch.Draw(spriteArray[2], new Rectangle(spritePos.X, spritePos.Y, spriteArray[2].Width, spriteArray[2].Height), color); // Right
                    break;
                case 4: case 5:
                    spriteBatch.Draw(spriteArray[4], new Rectangle(spritePos.X, spritePos.Y, spriteArray[4].Width, spriteArray[4].Height), color); // Up
                    break;
                case 6: case 7:
                    spriteBatch.Draw(spriteArray[6], new Rectangle(spritePos.X, spritePos.Y, spriteArray[6].Width, spriteArray[6].Height), color); // Down
                    break;
                case 8:
                    spriteBatch.Draw(spriteArray[8], new Rectangle(spritePos.X, spritePos.Y, spriteArray[8].Width, spriteArray[8].Height), color);
                    break;
            }
            base.Draw(spriteBatch);
            color = blessingColor; // And reset the color
            // Blessed: Yellow
            // Swift: PaleTurquoise
            // Shield: LightCoral
            // Artillery: GreenYellow
        }

        public override void Fire()
        {
            base.Fire();
            if (direction == 2 || direction == 3) // Move the bullet to the character's direction and gun's position
            {
                bullets[bul].spritePos = new Rectangle(spritePos.Right, spritePos.Y + 26 - (bullets[bul].spritePos.Height / 2), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 0; // Right
            }
            else if (direction == 4 || direction == 5)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 38 - (bullets[bul].spritePos.Width / 2), spritePos.Y - bullets[bul].spritePos.Height, bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 2; // Up
            }
            else if (direction == 6 || direction == 7)
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X + 69 - (bullets[bul].spritePos.Width / 2), spritePos.Y + 83, bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 3; // Down
            }
            else
            {
                bullets[bul].spritePos = new Rectangle(spritePos.X - bullets[bul].spritePos.Width, spritePos.Y + 26 - (bullets[bul].spritePos.Height / 2), bullets[bul].spritePos.Width, bullets[bul].spritePos.Height);
                bullets[bul].direction = 1; // Left
            }
            bullets[bul].isActive = true;
        }
    }
}
