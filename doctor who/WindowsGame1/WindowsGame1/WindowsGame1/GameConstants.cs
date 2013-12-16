using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsGame1
{
    static class GameConstants
    {
        //camera constants
        public const float CameraHeight = 25000.0f;
        public const float PlayfieldSizeX = 200f;
        public const float PlayfieldSizeZ = 200f;
        //Dalek constants
        public const int NumSatellites = 15;
        public const float SatelliteMinSpeed = 3.0f;
        public const float SatelliteMaxSpeed = 10.0f;
        public const float SatelliteSpeedAdjustment = 2.5f;
        public const float SatelliteScalar = 2.0f;
        //collision constants
        public const float SatelliteBoundingSphereScale = 0.30f;  //50% size
        public const float ShipBoundingSphereScale = 0.48f;  //50% size
        public const float RocketBoundingSphereScale = 0.20f;  //50% size
        public const float AlienBoundingSphereScale = 0.090f;
        public const float BombBoundingSphereScale = 0.5f;
        //bullet constants
        public const int NumRockets = 60;
        public const float RocketSpeedAdjustment = 10.0f;
        public const float RocketScalar = 0.025f;

        //bomb constants
        public const int NumBombs = 30;
        public const float BombSpeedAdjustment = 10.0f;
        public const float BombScalar = 0.3f;

        

        //health constants
        public const int NumHealth = 5;
        public const float HealthSphereScale = 0.025f;  //50% size
        public const float HealthScalar = 2.0f;
        public const float HealthSpeed =5.0f;

        //Alien Constants
        public const int NumAliens = 15;
        public const float AlienMinSpeed = 3.0f;
        public const float AlienMaxSpeed = 6.0f;
        public const float AlienSpeedAdjustment = 2.5f;
        public const float AlienScalar = 3.0f;

        //ship consants
        public const float ShipScalar = 0.2f; 
  

    }
}
