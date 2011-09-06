using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Screen;
using Diseases.Screen.Menu;
using Diseases.Screen.Level;

namespace Diseases
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (DiseasesGame game = new DiseasesGame())
            {
                Debug.WriteLine("game initialized", "INFO");

                game.Run();
            }
        }
    }

    public class DiseasesGame : Game
    {
        private bool                    gamecrashed = false;

        private SpriteBatch             spriteBatch;
        private GraphicsDeviceManager   graphicsManager;

        private Texture2D       crashTexture;

        public                  DiseasesGame    ()
        {
            this.graphicsManager = new GraphicsDeviceManager(this);

            this.graphicsManager.PreferredBackBufferWidth = 800;
            this.graphicsManager.PreferredBackBufferHeight = 540;
        }

        protected override void Dispose         (bool disposing)
        {
            base.Dispose(disposing);
        }
        protected override void Initialize      ()
        {
            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent     ()
        {
            this.Content.RootDirectory = "Content/Assets";

            this.crashTexture = this.Content.Load<Texture2D>("backgrounds/errr");

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            base.LoadContent();
        }
        protected override void UnloadContent   ()
        {
            base.UnloadContent();
        }

        protected override void Update          (GameTime gameTime)
        {
            if (!this.gamecrashed)
                try
                {
                    base.Update(gameTime);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "STOP");

                    this.gamecrashed = true;
                }
            else
            {

            }
        }
        protected override void Draw            (GameTime gameTime)
        {
            if (!this.gamecrashed)
            {
                try
                {
                    this.GraphicsDevice.Clear(Color.Black);

                    base.Draw(gameTime);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "STOP");

                    this.gamecrashed = true;
                }
            }
            else
            {
                try
                {
                    this.spriteBatch.Begin();

                    this.spriteBatch.Draw(this.crashTexture, Vector2.Zero, Color.White);

                    this.spriteBatch.End();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
