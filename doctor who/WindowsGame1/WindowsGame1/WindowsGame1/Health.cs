using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace WindowsGame1
{
    // struct for the health pickup
    struct Health
    {
        //variables for the health pickup
        public Vector3 Healthposition;
        public Vector3 Healthdirection;
        public float speed;
        public bool isActive;
        public int health;

        public void Update(float delta)
        {
            // the position, direction and speed of the health pickup.
            Healthposition += Healthdirection * speed *
                        GameConstants.HealthSpeed* delta;
            if (Healthposition.X > GameConstants.PlayfieldSizeX)
                Healthposition.X -= 2 * GameConstants.PlayfieldSizeX;
            if (Healthposition.X < -GameConstants.PlayfieldSizeX)
                Healthposition.X += 2 * GameConstants.PlayfieldSizeX;
            if (Healthposition.Z > GameConstants.PlayfieldSizeZ)
                Healthposition.Z -= 2 * GameConstants.PlayfieldSizeZ;
            if (Healthposition.Z < -GameConstants.PlayfieldSizeZ)
                Healthposition.Z += 2 * GameConstants.PlayfieldSizeZ;
        }
    }
}
