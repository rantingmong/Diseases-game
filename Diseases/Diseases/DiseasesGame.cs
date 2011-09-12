using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using Diseases.Screen;
using Diseases.Graphics;

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
        MenuMain menumain = new MenuMain();

        private bool                    musicplayed     = false;
        private bool                    gamecrashed     = false;

        private string                  crshmessage     = "";

        private SoundEffect             crashSound;

        private SpriteBatch             spriteBatch;
        private GraphicsDeviceManager   graphicsManager;

        private DGSpriteStatic          crashSprite;

        private DGScreenManager         screenmanager;

        public                          DiseasesGame    ()
        {
            this.graphicsManager = new GraphicsDeviceManager(this);

            this.graphicsManager.PreferMultiSampling = true;

            this.graphicsManager.PreferredBackBufferWidth   = 800;
            this.graphicsManager.PreferredBackBufferHeight  = 540;
        }

        protected override void         Dispose         (bool disposing)
        {
            base.Dispose(disposing);
        }
        protected override void         Initialize      ()
        {
            try
            {
                this.crashSprite = new DGSpriteStatic("backgrounds/errr");
                this.screenmanager = new DGScreenManager(this);

                this.Components.Add(this.screenmanager);

                this.screenmanager.AddScreen(this.menumain);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.Message;

                this.gamecrashed = true;
            }

            base.Initialize();
        }

        protected override void         LoadContent     ()
        {
            try
            {
                this.Content.RootDirectory = "content";

                this.crashSprite.LoadContent(this.Content);
                this.crashSound = this.Content.Load<SoundEffect>("sounds/crashsound");

                this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.Message;

                this.gamecrashed = true;
            }

            base.LoadContent();
        }
        protected override void         UnloadContent   ()
        {
            try
            {
                this.crashSprite.UnloadContent();
                this.crashSound.Dispose();

                this.menumain.UnloadContent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.Message;

                this.gamecrashed = true;
            }

            base.UnloadContent();
        }

        protected override void         Update          (GameTime gameTime)
        {
            if (!this.gamecrashed)
            {
                if (this.IsActive)
                    try
                    {
                        base.Update(gameTime);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "STOP");
                        Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                        this.crshmessage = ex.Message;

                        this.gamecrashed = true;
                    }
            }
            else
            {
                this.crashSprite.Update(gameTime);

                if (!this.musicplayed)
                {
                    this.crashSound.Play();

                    this.musicplayed = true;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    this.Exit();

                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.F1))
                    Debug.Fail(this.crshmessage);
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
                    Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                    this.crshmessage = ex.Message;

                    this.gamecrashed = true;
                }
            }
            else
            {
                this.spriteBatch.Begin();

                this.crashSprite.Render(this.spriteBatch);

                this.spriteBatch.End();
            }
        }
    }
}
