using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

//This is the class for the particle system.
namespace WindowsGame1
{
    class ParticleGenerator
    {
        //variables for the particle system
        Texture2D texture;

        float spawnWidth;
        float density;

        List<Fire> fires = new List<Fire>();

        float timer;

        Random rand1, rand2;

        // method for generating the particle effect.

        public ParticleGenerator(Texture2D newTexture, float newSpawnWidth, float newDensity)
        {
            texture = newTexture;
            spawnWidth = newSpawnWidth;
            density = newDensity;

            rand1 = new Random();
            rand2 = new Random();

        }
        // method for creating the particle
        public void createParticle()
        {

            //double anything  = rand1.Next();

            //random position for the particle effect

            fires.Add(new Fire(texture,new Vector2(
                -50 +(float)rand1.NextDouble() * spawnWidth, 0),
                  new Vector2(1, rand2.Next(5, 13))));
        
        }

        public void Update(GameTime gameTime, GraphicsDevice graphics)
        {

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // creates the effect when the game is running
            while (timer > 0)
            {
                timer -= 1f / density;
                createParticle();
            }

            // when the particle leaves the screen it is removed from the array.
            for (int i = 0; i < fires.Count; i++)
            {
                fires[i].Update();
                if (fires[i].Position.Y > graphics.Viewport.Height)
                {
                    fires.RemoveAt(i);
                    i--;

                }
            }
        }

        // allows the particle to be drawn on screen

        public void Draw(SpriteBatch spriteBatch)

        {
            foreach (Fire firedrop in fires)
                firedrop.Draw(spriteBatch);

            }

    }
}
