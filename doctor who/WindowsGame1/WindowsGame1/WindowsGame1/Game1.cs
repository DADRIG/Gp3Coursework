using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ParticleGenerator rain;

        //Textures for each screen.

        Texture2D startScreen;
        Texture2D mainBackground;
        Texture2D GameOverScreen;
        Texture2D successScreen;

        // Parallaxing Layers
        ScrollingBackground bgLayer1;
        ScrollingBackground bgLayer2;

     
        #region User Defined Variables
        //------------------------------------------
        // Added for use with fonts
        //------------------------------------------
        SpriteFont Font;
        //--------------------------------------------------
        // Added for use with playing Audio via Media player
        //--------------------------------------------------
        private Song bkgMusic;
     
        //enum for switching between game screens

        enum GameState
        {
            Titlescreen,
            Playing,
            success

        }

        //current screen that is being displayed.
        GameState CurrentGameState = GameState.Titlescreen;
        //--------------------------------------------------
        //Set the sound effects to use
        //--------------------------------------------------
        private SoundEffectInstance tardisSoundInstance;
        private SoundEffect tardisSound;
        private SoundEffect explosionSound;
        private SoundEffect firingSound;
        private SoundEffect DeathSound;
        private SoundEffect BoostSound;

       
        //variables for the reload times for the weapons.

        int counter = 1;
        float countDuration = 1f;
        float currentTime = 0f;
        float finishTime = 0f;
        int reloadTime = 0;

        //float for creating the timer.
        float timer= 0;

        //  int for retrieving and setting the score.
        public int Score { get; set; }
        
        //starting health value
        public int health = 100;

        // boundingsphere for the player.
        public BoundingSphere playerBox;

        // Set the 3D model to draw.
        private Model mdlTardis;
        private Matrix[] mdlTardisTransforms;
        private Matrix tardisTransform;

        // The aspect ratio determines how to scale 3d to 2d projection.
        private float aspectRatio;

        // Set the position of the model in world space, and set the rotation.
        private Vector3 mdlPosition = Vector3.Zero;
        private float mdlRotation = 0.0f;
        private Vector3 mdlVelocity = Vector3.Zero;

        private Vector3 mdlendposition = new Vector3(450.0f, 220.0f, 220f);
        
        //variables for setting the width and height of the game screen
        int screenWidth;
        int screenHeight;
        

        // create an array of enemy satellites.
        private Model mdlSatellite;
        private Matrix[] mdsatelliteransforms;
        private Satellites[] satelliteList = new Satellites[GameConstants.NumSatellites];

        // create an array of rockets
        private Model mdlrocket;
        private Matrix[] mdlrocketTransforms;
        private float rocketrotation;
        private Rocket[] rocketList = new Rocket[GameConstants.NumRockets];

        //create an array of aliens
        private Model mdlAlien;
        private Matrix[] mdlAlienTransforms;
        private Alien[] AlienList = new Alien[GameConstants.NumAliens];
        
        //create an array of bombs.
        private Model mdlBomb;
        private Matrix[] mdlBombTransforms;
        private Bomb[] BombList = new Bomb[GameConstants.NumBombs];

        //create a health item
        private Model mdlHealth;
        private Matrix[] mdlHealthTransforms;
        private Health Health;

        //create a random variable
        private Random random = new Random();

        //variable for different camera
        private Camera FreeRoam;

        //boolean for switching cameras
        private bool MyCam = true;

       
        //setting the keyboard state
        private KeyboardState lastState;
        private KeyboardState keyboardState;

        //integer for increasing the hit count
        private int hitCount;

        // Set the position of the camera in world space, for our view matrix.
        private Vector3 cameraPosition1 = new Vector3(0.0f, 155.0f, 50.0f);
        private Matrix viewMatrix1;
        private Matrix projectionMatrix1;

        //set the postion of the different camera in world space.
        private Vector3 Position = new Vector3(0.0f, 155.0f, 50.0f);
        private Matrix View;
        private Matrix Projection;


        // method for the camera transform of the initial camera
        private void InitializeTransform()
        {
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            viewMatrix1 = Matrix.CreateLookAt(cameraPosition1, Vector3.Zero, Vector3.Up);

            projectionMatrix1 = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(90), aspectRatio, 1.0f, 350.0f);

        }

        //method for the camera transform of the different camera.
        private void InitializeFreeRoam()
        {
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up);

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), aspectRatio, 1.0f, 350.0f);

            FreeRoam = new Camera(this, new Vector3(0.0f, 155.0f, 50.0f), Vector3.Zero, 5.0f);
            Components.Add(FreeRoam);

        }

        //method for allowing the model to move.
        private void MoveModel()
        {
            //gets the state of the keyboard.
             keyboardState = Keyboard.GetState();

             // pressing enter allows the change of game screens.

             if (keyboardState.IsKeyDown(Keys.Enter))
             {

                 CurrentGameState = GameState.Playing;
             }


            // Create some velocity if the right trigger is down.
            Vector3 mdlVelocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            mdlVelocityAdd.X = -(float)Math.Sin(mdlRotation);
            mdlVelocityAdd.Z = -(float)Math.Cos(mdlRotation);

            if (keyboardState.IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)//allows the xbox controller input
            {
                // Rotate left.
                mdlRotation -= -1.0f * 0.10f;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)//allows the xbox controller input.
            {
                // Rotate right.
                mdlRotation -= 1.0f * 0.10f;
            }

            if (keyboardState.IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)//allows the xbox controller input.
            {
                // Rotate left.
                // Create some velocity if the right trigger is down.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityAdd *= 0.05f;
                mdlVelocity += mdlVelocityAdd;
            }

            if (keyboardState.IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)//allows the xbox controller input.
            {
                // Rotate left.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityAdd *= -0.05f;
                mdlVelocity += mdlVelocityAdd;
            }


            


             // if statement which allows switching between cameras with a button press. 
            if (keyboardState.IsKeyDown(Keys.C) && lastState.IsKeyUp(Keys.C) || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)//allows the xbox controller input.
            {
                if (MyCam)
                {
                    MyCam = false;
                }

                else
                {
                    MyCam = true;
                }
            }


            

            
          

            //are we shooting?
            if (keyboardState.IsKeyDown(Keys.Space) || lastState.IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)//allows the xbox controller input.
            {
                //add another bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumRockets; i++)
                {
                    if (!rocketList[i].isActive && reloadTime == 0)
                    {
                        rocketrotation = mdlRotation;
                        tardisTransform = Matrix.CreateRotationY(rocketrotation);
                        rocketList[i].direction = tardisTransform.Forward;
                        rocketList[i].speed = GameConstants.RocketSpeedAdjustment;
                        rocketList[i].position = mdlPosition + rocketList[i].direction;
                        rocketList[i].isActive = true;
                        
                        //plays sound when rocket is fired
                        firingSound.Play();
                        // reload time after rocket is fired.
                        reloadTime = 1;
                        break; //exit the loop     
                    }
                }
            }


            if (keyboardState.IsKeyDown(Keys.S) || lastState.IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)//allows the xbox controller input.
            {
                //add another different bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumBombs; i++)
                {
                    if (!BombList[i].isActive && reloadTime == 0)
                    {
                        rocketrotation = mdlRotation;
                        tardisTransform = Matrix.CreateRotationY(rocketrotation);
                        BombList[i].Bombdirection = tardisTransform.Forward;
                        BombList[i].Bombspeed = GameConstants.BombSpeedAdjustment;
                        BombList[i].Bombposition = mdlPosition +BombList[i].Bombdirection;
                        BombList[i].isActive = true;

                        //plays the sound when the bomb is fired.
                        firingSound.Play();
                        //time taken before another bomb can be fired.
                        reloadTime = 3;
                        break; //exit the loop     
                    }
                }
            }

            {

                // allows the muting of audio and sound effects with a key press
                if (keyboardState.IsKeyDown(Keys.M) || GamePad.GetState(PlayerIndex.One).Buttons.X== ButtonState.Pressed)//allows the xbox controller input.
                {
                    //mutes audio
                    MediaPlayer.IsMuted = true;
                    //mutes sound effects
                    SoundEffect.MasterVolume = 0;
                }



                // allows the sound to be played with a key press.
                if (keyboardState.IsKeyDown(Keys.N) || lastState.IsKeyDown(Keys.N))
                {
                    //plays audio
                    MediaPlayer.IsMuted = false;
                    //plays sound effects
                    SoundEffect.MasterVolume = 1;
                }

            }

            lastState = keyboardState;

        }

        // method for intially spawning the satellites in the game. They have a random x and y position in relation to the playfield size.
        private void ResetSatellites()
        {
            float xStart;
            float zStart;
            for (int i = 0; i < GameConstants.NumSatellites; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                zStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeZ;
                satelliteList[i].position = new Vector3(xStart, 0.0f, zStart);
                double angle = random.NextDouble() * 2 * Math.PI;
                satelliteList[i].direction.X = -(float)Math.Sin(angle);
                satelliteList[i].direction.Z = (float)Math.Cos(angle);
                satelliteList[i].speed = GameConstants.SatelliteMinSpeed +
                   (float)random.NextDouble() * GameConstants.SatelliteMaxSpeed;
                satelliteList[i].isActive = true;

            }

        }

        // method for intially spawning the aliens in the game. They have a random x and y position in relation to the playfield size.
        public void SpawnAliens()
        {

            float xStart;
            float zStart;
            for (int i = 0; i < GameConstants.NumAliens; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                zStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeZ;
                AlienList[i].Alienposition = new Vector3(xStart, 0.0f,zStart);
                double angle = random.NextDouble() * 2 * Math.PI;
                AlienList[i].Aliendirection.X = -(float)Math.Sin(angle);
                AlienList[i].Aliendirection.Z = (float)Math.Cos(angle);
                AlienList[i].Alienspeed = GameConstants.AlienMinSpeed +
                   (float)random.NextDouble() * GameConstants.AlienMaxSpeed;
                AlienList[i].isActive = true;
            }
        }

        // method for intially spawning the health pickup in the game. It has a random spawn point in relation to the x and y position in the play field size.
        private void SpawnHealth()
        {
            float xStart;
            float zStart;
            for (int i = 0; i < GameConstants.NumHealth; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                zStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeZ;
                Health.Healthposition = new Vector3(xStart, 0.0f, zStart);
                double angle = random.NextDouble() * 2 * Math.PI;
                Health.Healthdirection.X = -(float)Math.Sin(angle);
                Health.Healthdirection.Z = (float)Math.Cos(angle);
                Health.isActive = true;

            }

        }
        //matrix for setting up the default effect transformations.
        
        private Matrix[] SetupEffectTransformDefaults(Model myModel)
        {
            //creates a new matrix in relation to the transforms.
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    //changes the transformation defaults in relation to which camera is active.
                    if (MyCam == false)
                    {
                    effect.Projection = projectionMatrix1;
                    effect.View = viewMatrix1;

                    }
                    else

                        if (MyCam == true)
                        {
                            effect.Projection = Projection;
                            effect.View = View;

                        }



                }
            }
            return absoluteTransforms;
        }

        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;

                    // draws the model in relation to which camera is active.
                    if (MyCam == false)
                    {
                        effect.Projection = projectionMatrix1;
                        effect.View = viewMatrix1;

                    }
                    else

                        if (MyCam == true)
                        {
                            effect.Projection = Projection;
                            effect.View = View;

                        }
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

        // method for writing text.
        private void writeText(string msg, Vector2 msgPos, Color msgColour)
        {
            spriteBatch.Begin();
            string output = msg;
            // Find the center of the string
            Vector2 FontOrigin = Font.MeasureString(output) / 2;
            Vector2 FontPos = msgPos;
            // Draw the string
            spriteBatch.DrawString(Font, output, FontPos, msgColour);
            spriteBatch.End();
        }

        #endregion
        //method for the game.
        public Game1()
        {
            //creates a new graphics device manager
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //allows the mouse to be visible on screen.
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // methods that are called when the game is initialized.
            this.IsMouseVisible = false;
            Window.Title = "Spoderman In Space";
            //graphics.ApplyChanges();
            hitCount = 0;
            InitializeTransform();
            InitializeFreeRoam();
            bgLayer1 = new ScrollingBackground();
            bgLayer2 = new ScrollingBackground();
            ResetSatellites();
            SpawnAliens();
            SpawnHealth();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            IsMouseVisible = true;

            //-------------------------------------------------------------
            // added to load font
            //-------------------------------------------------------------
            Font = Content.Load<SpriteFont>(".\\Fonts\\Font");
            //-------------------------------------------------------------
            // added to load Song
            //-------------------------------------------------------------
            bkgMusic = Content.Load<Song>(".\\Audio\\Obsidia");
            MediaPlayer.Play(bkgMusic);
            MediaPlayer.IsRepeating = true;
            //-------------------------------------------------------------
            // added to load Model
            //-------------------------------------------------------------
            mdlTardis = Content.Load<Model>(".\\Models\\wasphunter");
            mdlTardisTransforms = SetupEffectTransformDefaults(mdlTardis);
            mdlSatellite = Content.Load<Model>(".\\Models\\uhfsat");
            mdsatelliteransforms = SetupEffectTransformDefaults(mdlSatellite);
            mdlrocket = Content.Load<Model>(".\\Models\\retro_rocket");
            mdlrocketTransforms = SetupEffectTransformDefaults(mdlrocket);
            mdlAlien = Content.Load<Model>(".\\Models\\Alien Character");
            mdlAlienTransforms = SetupEffectTransformDefaults(mdlAlien);
            mdlHealth = Content.Load<Model>(".\\Models\\Battery");
            mdlHealthTransforms = SetupEffectTransformDefaults(mdlHealth);
            mdlBomb = Content.Load<Model>(".\\Models\\Grenade");
            mdlBombTransforms = SetupEffectTransformDefaults(mdlBomb);

            //-------------------------------------------------------------
            // added to load SoundFX's
            //-------------------------------------------------------------
            tardisSound = Content.Load<SoundEffect>("Audio\\tardisEdit");
            explosionSound = Content.Load<SoundEffect>("Audio\\explosion2");
            firingSound = Content.Load<SoundEffect>("Audio\\shot007");
            DeathSound = Content.Load<SoundEffect>("Audio\\Scream");
            BoostSound = Content.Load<SoundEffect>("Audio\\Thrust");
            tardisSoundInstance = tardisSound.CreateInstance();
            tardisSoundInstance.Play();

            // Load the parallaxing background
            bgLayer1.Initialize(Content, "bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "bgLayer2", GraphicsDevice.Viewport.Width, -2);

            //Set the screen width and height in relation to the texture.
            screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            //loading different screen textures.
            startScreen = Content.Load<Texture2D>(".\\Textures\\startscreen");
            mainBackground = Content.Load<Texture2D>(".\\Textures\\farback");
            GameOverScreen = Content.Load<Texture2D>(".\\Textures\\spoderman");
            successScreen = Content.Load<Texture2D>(".\\Textures\\win");

            //Allows the particle to be loaded.
            rain= new ParticleGenerator(Content.Load<Texture2D>(".\\Textures\\fire"), graphics.GraphicsDevice.Viewport.Width, 50);

           // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || lastState.IsKeyDown(Keys.Escape))
                this.Exit();

            MoveModel();

            

            //what happens when the current game state is on.
            if (CurrentGameState == GameState.Titlescreen)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(startScreen, Vector2.Zero, Color.White);
               
                spriteBatch.End();


                if (keyboardState.IsKeyDown(Keys.Enter))
                {

                    CurrentGameState = GameState.Playing;
                   

                   
                }


            }


            //what happens when the gamestate switches to a new state.

            
if (CurrentGameState == GameState.Playing)
            {
               
                //updates the particle generator.
                rain.Update(gameTime, graphics.GraphicsDevice);

                //updates both of the background layers.
                bgLayer1.Update();
                bgLayer2.Update();

                //allows the current time and finish time to be used.
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                finishTime += (float)gameTime.TotalGameTime.TotalSeconds;


                // if function for the reload time if its greater than 0 it makes a delay.
                if (reloadTime > 0)
                {
                    if (currentTime >= countDuration)
                    {
                        counter++;
                        currentTime -= countDuration;
                    }

                    if (counter >= reloadTime)
                    {
                        counter = 0;
                        reloadTime = 0;
                    }
                }

                //allows the timer to be updated.
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;



                // TODO: Add your update logic here
                MoveModel();

                // Add velocity to the current position.
                mdlPosition += mdlVelocity;

                // Bleed off velocity over time.

                mdlVelocity *= 0.95f;

                //updates time delta
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //allows the array for satellites to be updated
                for (int i = 0; i < GameConstants.NumSatellites; i++)
                {
                    satelliteList[i].Update(timeDelta);
                }

                //allows the array for aliens to be updated
                for (int i = 0; i < GameConstants.NumAliens; i++)
                {
                    AlienList[i].Update(timeDelta);
                }

                //allows the array for rockets to be updated
                for (int i = 0; i < GameConstants.NumRockets; i++)
                {
                    if (rocketList[i].isActive)
                    {
                        rocketList[i].Update(timeDelta);
                    }
                }

                //allows the array for bombs to be updated
                for (int h = 0; h < GameConstants.NumBombs; h++)
                {
                    if (BombList[h].isActive)
                    {
                        BombList[h].Update(timeDelta);
                    }
                }


                //allows the health pickup to be updated.
                if (Health.isActive)
                {
                    Health.Update(timeDelta);
                }


                // if health reaches zero .
                if (health <= 0)
                {
                    health = 0;

                    MediaPlayer.IsMuted = true;
                    SoundEffect.MasterVolume = 0;

                }


           

                    //creates a new boundingsphere for the ship.
                    BoundingSphere ShipSphere =
                      new BoundingSphere(mdlPosition,
                               mdlTardis.Meshes[0].BoundingSphere.Radius *
                                     GameConstants.ShipBoundingSphereScale);




                    //Check for collisions involved with the satellite
                    for (int i = 0; i < satelliteList.Length; i++)
                    {
                        if (satelliteList[i].isActive)
                        {
                            BoundingSphere satelliteSphereA =
                              new BoundingSphere(satelliteList[i].position, mdlSatellite.Meshes[0].BoundingSphere.Radius *
                                             GameConstants.SatelliteBoundingSphereScale);


                            for (int k = 0; k < rocketList.Length; k++)
                            {
                                if (rocketList[k].isActive)
                                {
                                    BoundingSphere rocketSphere = new BoundingSphere(
                                      rocketList[k].position, mdlrocket.Meshes[0].BoundingSphere.Radius *
                                             GameConstants.RocketBoundingSphereScale);
                                    if (satelliteSphereA.Intersects(rocketSphere))
                                    {
                                        explosionSound.Play();
                                        satelliteList[i].isActive = false;
                                        rocketList[k].isActive = false;

                                        hitCount++;
                                        Score += 10;
                                        break; //no need to check other bullets
                                    }

                                }
                            }

                            for (int l = 0; l < BombList.Length; l++)
                            {
                                if (BombList[l].isActive)
                                {
                                    BoundingSphere BombSphere = new BoundingSphere(
                                      BombList[l].Bombposition, mdlBomb.Meshes[0].BoundingSphere.Radius *
                                             GameConstants.BombBoundingSphereScale);
                                    if (satelliteSphereA.Intersects(BombSphere))
                                    {
                                        explosionSound.Play();
                                        satelliteList[i].isActive = false;
                                        BombList[l].isActive = false;

                                        hitCount++;
                                        Score += 20;
                                        break; //no need to check other bullets
                                    }


                                }

                            }


                            if (satelliteSphereA.Intersects(ShipSphere)) //Check collision between rocket and ship.
                            {
                                explosionSound.Play();


                                if (health > 0)
                                {
                                    health -= 10;
                                }

                                satelliteList[i].isActive = false;
                                satelliteList[i].direction *= -1.0f;
                                rocketList[i].isActive = false;
                            }


                            //check for collisions involved with the aliens
                            for (int j = 0; j < AlienList.Length; j++)
                            {
                                if (AlienList[j].isActive)
                                {
                                    BoundingSphere AlienSphereA =
                                      new BoundingSphere(AlienList[j].Alienposition, mdlAlien.Meshes[0].BoundingSphere.Radius *
                                                     GameConstants.AlienBoundingSphereScale);

                                    for (int r = 0; r < rocketList.Length; r++)
                                    {
                                        if (rocketList[r].isActive)
                                        {
                                            BoundingSphere laserSphere = new BoundingSphere(
                                              rocketList[r].position, mdlrocket.Meshes[0].BoundingSphere.Radius *
                                                     GameConstants.RocketBoundingSphereScale);
                                            if (AlienSphereA.Intersects(laserSphere))
                                            {
                                                DeathSound.Play();
                                                AlienList[j].isActive = false;
                                                rocketList[r].isActive = false;

                                                hitCount++;
                                                Score += 20;
                                                break; //no need to check other bullets
                                            }


                                        }
                                    }

                                    for (int s = 0; s < BombList.Length; s++)
                                    {
                                        if (BombList[s].isActive)
                                        {
                                            BoundingSphere BombSphere = new BoundingSphere(
                                              BombList[s].Bombposition, mdlBomb.Meshes[0].BoundingSphere.Radius *
                                                     GameConstants.BombBoundingSphereScale);
                                            if (AlienSphereA.Intersects(BombSphere))
                                            {
                                                DeathSound.Play();
                                                AlienList[j].isActive = false;
                                                BombList[s].isActive = false;

                                                hitCount++;
                                                Score += 20;
                                                break; //no need to check other bullets
                                            }


                                        }



                                    }
                                    if (AlienSphereA.Intersects(ShipSphere)) //Check collision between alien and ship.
                                    {
                                        explosionSound.Play();
                                        if (health > 0)
                                        {
                                            health -= 20;
                                        }


                                        AlienList[j].isActive = false;
                                        AlienList[j].Aliendirection *= -1.0f;
                                        rocketList[i].isActive = false;

                                        break; //no need to check other bullets

                                    }

                                    //check for collsions involving the health pick up.
                                    if (Health.isActive)
                                    {


                                        BoundingSphere HealthSphere = new BoundingSphere(
                                          Health.Healthposition, mdlHealth.Meshes[0].BoundingSphere.Radius *
                                                 GameConstants.HealthSphereScale);


                                        if (ShipSphere.Intersects(HealthSphere))
                                        {


                                            Health.isActive = false;

                                            if (Health.isActive == false)
                                            {

                                                Health.Healthposition.X = random.Next(7) * GameConstants.PlayfieldSizeX;
                                                Health.Healthposition.Z = random.Next(7) * GameConstants.PlayfieldSizeZ;

                                            }
                                            if (health < 100)
                                            {
                                                health += 10;
                                            }

                                            break; //no need to check other bullets
                                        }

                                        
                                    }

                                  

                                }

                               
                            }
                          
                        }

                        
                    }
                   
                }

            base.Update(gameTime); 
           
            }
        
       
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            


            //what is drawn when the current screen is in use.
            if (CurrentGameState == GameState.Titlescreen)
            {
               spriteBatch.Begin();
               spriteBatch.Draw(startScreen, Vector2.Zero, Color.White);
               spriteBatch.End();


            }


            // what is drawn when the game is in a playing state.
            if (CurrentGameState == GameState.Playing)
            {
                spriteBatch.Begin();

                //draw the main background on screen.
                spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
                // Draw the moving background
                bgLayer1.Draw(spriteBatch);
                bgLayer2.Draw(spriteBatch);
                //draw the particle.
                rain.Draw(spriteBatch);

                //draw the required strings on screen.
                spriteBatch.DrawString(Font, "Time: " + timer.ToString("0.00"), new Vector2(0, 0), Color.Red);
                spriteBatch.DrawString(Font, "Score: " + Score.ToString("0"), new Vector2(310, 0), Color.BlanchedAlmond);
                spriteBatch.DrawString(Font, "Health: " + health + "%", new Vector2(575, 0), Color.Yellow);


                spriteBatch.End();
 
                //draws the different models on screen with their required variables and constants.
                for (int i = 0; i < GameConstants.NumSatellites; i++)
                {
                    if (satelliteList[i].isActive)
                    {
                        Matrix dalekTransform = Matrix.CreateScale(GameConstants.SatelliteScalar) * Matrix.CreateTranslation(satelliteList[i].position);
                        DrawModel(mdlSatellite, dalekTransform, mdsatelliteransforms);
                    }
                }
                for (int i = 0; i < GameConstants.NumRockets; i++)
                {
                    if (rocketList[i].isActive)
                    {
                        Matrix laserTransform = Matrix.CreateScale(GameConstants.RocketScalar) * Matrix.CreateRotationY(rocketrotation) * Matrix.CreateTranslation(rocketList[i].position);
                        DrawModel(mdlrocket, laserTransform, mdlrocketTransforms);
                    }
                }


                for (int i = 0; i < GameConstants.NumBombs; i++)
                {
                    if (BombList[i].isActive)
                    {
                        Matrix BombTransform = Matrix.CreateScale(GameConstants.BombScalar) * Matrix.CreateRotationY(rocketrotation) * Matrix.CreateTranslation(BombList[i].Bombposition);
                        DrawModel(mdlBomb, BombTransform, mdlBombTransforms);
                    }
                }


                for (int i = 0; i < GameConstants.NumAliens; i++)
                {
                    if (AlienList[i].isActive)
                    {
                        Matrix AlienTransform = Matrix.CreateScale(GameConstants.AlienScalar) * Matrix.CreateTranslation(AlienList[i].Alienposition);
                        DrawModel(mdlAlien, AlienTransform, mdlAlienTransforms);
                    }
                }
                if (Health.isActive)
                {
                    Matrix HealthTransform = Matrix.CreateScale(GameConstants.HealthScalar) * Matrix.CreateTranslation(Health.Healthposition);
                    DrawModel(mdlHealth, HealthTransform, mdlHealthTransforms);
                }




                Matrix modelTransform = Matrix.CreateScale(GameConstants.ShipScalar) * Matrix.CreateRotationY(mdlRotation) * Matrix.CreateTranslation(mdlPosition);
                DrawModel(mdlTardis, modelTransform, mdlTardisTransforms);

            }
            

            //draws the end screen if health runs out.
            if (health <= 0)
            {
                spriteBatch.Begin();
                DrawScenery();
                spriteBatch.DrawString(Font, "GAME OVER! ",new Vector2(125, 225), Color.Blue);
                spriteBatch.DrawString(Font, "Score: " + Score.ToString("0"), new Vector2(125, 275), Color.Blue);
                spriteBatch.End();
                

                }




            //draws the end screen if health runs out.
            if (Score >= 250)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font, "Score: " + Score.ToString("0"), new Vector2(125, 275), Color.Blue);
                spriteBatch.Draw(successScreen, Vector2.Zero, Color.White);
                spriteBatch.End();


            }
            
            base.Draw(gameTime);

        }

        //method for drawing the game over screen.
        private void DrawScenery()
        {
           
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(GameOverScreen, screenRectangle, Color.White);
           
        }

      
    }






}


 