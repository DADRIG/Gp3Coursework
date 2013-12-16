using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    struct Rocket
    {
        //variables for the rocket gun type
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public bool isActive;

        public void Update(float delta)
        {
            //the position, direction and speed for the rocket.
            position += direction * speed *
                        GameConstants.RocketSpeedAdjustment * delta;
            if (position.X > GameConstants.PlayfieldSizeX ||
                position.X < -GameConstants.PlayfieldSizeX ||
                position.Z > GameConstants.PlayfieldSizeZ ||
                position.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;
        }
    }
}
