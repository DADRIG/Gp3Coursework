using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WindowsGame1
{
    //This is a struct for the bomb weapon
    struct Bomb
    {
        //variables for the bomb weapon
        public Vector3 Bombposition;
        public Vector3 Bombdirection;
        public float Bombspeed;
        public bool isActive;

        public void Update(float delta)
        {
            //updates the position, direction and speed of the bomb
            Bombposition += Bombdirection * Bombspeed *
                        GameConstants.RocketSpeedAdjustment * delta;
            if (Bombposition.X > GameConstants.PlayfieldSizeX ||
                Bombposition.X < -GameConstants.PlayfieldSizeX ||
                Bombposition.Z > GameConstants.PlayfieldSizeZ ||
                Bombposition.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;
        }
    }
}

