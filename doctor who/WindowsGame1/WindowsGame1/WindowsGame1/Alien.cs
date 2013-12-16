using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WindowsGame1
{
    //This is a struct for the alien enemy.
    struct Alien
    {
        // These are the variables for the alien
        public Vector3 Alienposition;
        public Vector3 Aliendirection;
        public float Alienspeed;
        public bool isActive;


        public void Update(float delta)
        {
            //Updates the alien position,direction and speed
            Alienposition += Aliendirection * Alienspeed *
                        GameConstants.AlienSpeedAdjustment * delta;
            if (Alienposition.X > GameConstants.PlayfieldSizeX)
                Alienposition.X -= 2 * GameConstants.PlayfieldSizeX;
            if (Alienposition.X < -GameConstants.PlayfieldSizeX)
                Alienposition.X += 2 * GameConstants.PlayfieldSizeX;
            if (Alienposition.Z > GameConstants.PlayfieldSizeZ)
                Alienposition.Z -= 2 * GameConstants.PlayfieldSizeZ;
            if (Alienposition.Z < -GameConstants.PlayfieldSizeZ)
                Alienposition.Z += 2 * GameConstants.PlayfieldSizeZ;
        }
    }
}

