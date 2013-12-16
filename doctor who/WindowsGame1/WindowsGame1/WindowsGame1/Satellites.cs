using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    // this is the struct for the enemy satellites.
    struct Satellites
    {
        //variables needed
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public bool isActive;

        public void Update(float delta)
        {
            //updates the position, direction and speed of the satellites.
            position += direction * speed *
                        GameConstants.SatelliteSpeedAdjustment * delta;
            if (position.X > GameConstants.PlayfieldSizeX)
                position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (position.X < -GameConstants.PlayfieldSizeX)
                position.X += 2 * GameConstants.PlayfieldSizeX;
            if (position.Z > GameConstants.PlayfieldSizeZ)
                position.Z -= 2 * GameConstants.PlayfieldSizeZ;
            if (position.Z < -GameConstants.PlayfieldSizeZ)
                position.Z += 2 * GameConstants.PlayfieldSizeZ;
        }
    }
}
