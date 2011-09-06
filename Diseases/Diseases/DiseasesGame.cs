using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using Diseases.Screen;
using Diseases.Graphics;

using Diseases.Screen.Menu;

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
        private bool                    musicplayed     = false;
        private bool                    gamecrashed     = false;

        private SoundEffect             crashSound;

        private SpriteBatch             spriteBatch;
        private GraphicsDeviceManager   graphicsManager;

        private DGSpriteStatic          crashSprite;

        private DGScreenManager         screenmanager;

        public                          DiseasesGame    ()
        {
            this.graphicsManager = new GraphicsDeviceManager(this);

            this.graphicsManager.PreferredBackBufferWidth = 800;
            this.graphicsManager.PreferredBackBufferHeight = 540;
        }

        protected override void         Dispose         (bool disposing)
        {
            base.Dispose(disposing);
        }
        protected override void         Initialize      ()
        {
            try
            {
                this.IsMouseVisible = true;

                this.crashSprite = new DGSpriteStatic("backgrounds/errr");
                this.screenmanager = new DGScreenManager(this);

                this.Components.Add(this.screenmanager);

                this.screenmanager.AddScreen(new MenuMain(this.screenmanager));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine(ex.StackTrace, "STOP");

                this.gamecrashed = true;
            }

            base.Initialize();
        }

        protected override void         LoadContent     ()
        {
            try
            {
                this.Content.RootDirectory = "Content/Assets";

                this.crashSprite.LoadContent(this.Content);
                this.crashSound = this.Content.Load<SoundEffect>("sounds/126 pokemon whistle");

                this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine(ex.StackTrace, "STOP");

                this.gamecrashed = true;
            }

            base.LoadContent();
        }
        protected override void         UnloadContent   ()
        {
            this.crashSprite.UnloadContent();
            this.crashSound.Dispose();

            base.UnloadContent();
        }

        protected override void         Update          (GameTime gameTime)
        {
            if (!this.gamecrashed)
                try
                {
                    base.Update(gameTime);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "STOP");
                    Debug.WriteLine(ex.StackTrace, "STOP");

                    this.gamecrashed = true;
                }
            else
            {
                this.crashSprite.Update(gameTime);

                if (!this.musicplayed)
                {
                    this.crashSound.Play();

                    this.musicplayed = true;
                }
            }
        }
        protected override void         Draw            (GameTime gameTime)
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
                    Debug.WriteLine(ex.StackTrace, "STOP");

                    this.gamecrashed = true;
                }
            }
            else
            {
                try
                {
                    this.spriteBatch.Begin();

                    this.crashSprite.Render(this.spriteBatch);

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
