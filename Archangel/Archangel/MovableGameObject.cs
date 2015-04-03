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
    // Cheshire Games, Bierre, March 13, 2015
    // MoveableGameObject defines properties for children such as Bullet, Character, and Sword

    // Change Log
    // T 3/26/15- added sprite array and properties, changed constructor to reflect this, moved direction and properties to here, added speed to constructor
                  // NOTE: changed all constructors in child classes to reflect changes to this one
    // B 4/1/15 - made a slight change to the constructor
    // T 4/2/15- added color variable and used it in the draw for a red flash when characters are hit
    // B 4/2/15 - changed the Draw method to include scaling
    public abstract class MovableGameObject:GameObject
    { 
        private int speed; // Variable for object speed and properties
        protected int objSpeed
        {
            get { return speed; }
            set { speed = value; }
        }

        protected Color color = Color.White; // Will turn red momentarily when hit

        private Texture2D[] spriteImages; // Holds all the sprites for a given object
        protected Texture2D[] spriteArray
        {
            get { return spriteImages; }
            set { spriteImages = value; }
        }

        /// <Logic>
        /// for bullet: right=0,left=1,up=2,down=3
        /// for sky player: faceRight=0,moveRight=1,faceLeft=2,moveLeft=3,faceUp=4,moveUp=5,faceDown=6,moveDown=7,dead=8
        /// for ground player: faceUp=0, moveRight=1,moveLeft=2,dead=3
        /// for enemies: faceLeft=0,moveLeft=1,faceRight=2,moveRight=3,faceUp=4,moveUp=5,faceDown=6,moveDown=7,dead=8
        /// </Logic>
        private int facedDirection; 
        public int direction // Direction object is facing and/or moving in
        {
            get { return facedDirection; }
            set { facedDirection = value; }
        }
        
        // NOTE: This does not compile due to a null reference exception - B 4/1/15
        public MovableGameObject(int X, int Y, int dir, int spd, Texture2D[] loadSprite) // Sets x,y,and sprite for object
            : base(X, Y, loadSprite[dir])
        {
            spriteImages = loadSprite; // Initializes array of sprites
            facedDirection = dir; // Initialize direction
            speed = spd; // Initialize movement speed
        }

        public abstract void Update(); // Requires a movement method for all children

        public override void Draw(SpriteBatch spriteBatch) // Draw the sprites
        {
            spriteBatch.Draw(spriteArray[direction], new Vector2(spritePos.X, spritePos.Y), spritePos, color, 0, new Vector2(), this.FloatScale, SpriteEffects.None, 0);
            color = Color.White; // Reset the colo
        }
    }
}
