using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    //class for the fire particle
    class Fire
    {
        //variables for the fire particle
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;

        //returns the position of the fire particle
        public Vector2 Position
        {
            get { return position; }
        }

        // method for creating the fire particle with texture position and velocity.
        public Fire(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity)
        {
            texture = newTexture;
            position = newPosition;
            velocity = newVelocity;
        }


        public void Update()
        {
            //adding the position and velocity together.
            position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //drawing a spritebatch for the fire to be drawn on.
            spriteBatch.Draw(texture, position, Color.White);
        }



    }
}
