﻿using System;
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
    // GamePiece defines necessary objects for child classes Platform and MoveableGamePiece

    // Change Log
    // T 3/26/15- added spritebatch variable for drawing
    public abstract class GameObject
    {
        protected SpriteBatch spriteBatch; // For drawing

        private Rectangle objPos;  // Rectangle position of object's hitbox and sprite (x,y,width,height)
        public Rectangle spritePos
        {
            get { return objPos; }
            set { objPos = value; }
        }

        private Texture2D sprite;
        public Texture2D spriteImg
        {
            get { return sprite; }
        }


        public GameObject(int X, int Y, Texture2D loadSprite) // Constructor runs to set up new game object
        {
            sprite = loadSprite; // Load image into texture
            spritePos = new Rectangle(X, Y, sprite.Width, sprite.Height); // Load image/hitbox position
        }

        public abstract void Draw(){} // Requires on screen objects to have a render method
    }
}
