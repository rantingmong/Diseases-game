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
        MenuMain                        menumain        = new MenuMain();

        SamplerState                    samplerState    = new SamplerState();

        private bool                    musicplayed     = false;
        private bool                    gamecrashed     = false;

        private string                  crshmessage     = "";

        private Song                    crashSong;
        private SpriteFont              debugFont;

        private SpriteBatch             spriteBatch;
        private GraphicsDeviceManager   graphicsManager;

        private DGSpriteStatic          crashSprite;

        private DGScreenManager         screenmanager;

        public                          DiseasesGame    ()
        {
            this.IsMouseVisible = true;

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
                this.crashSprite = new DGSpriteStatic("backgrounds/stop");
                this.screenmanager = new DGScreenManager(this);

                this.Components.Add(this.screenmanager);

                this.screenmanager.AddScreen(this.menumain);

                base.Initialize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.Message;

                this.gamecrashed = true;
            }
        }

        protected override void         LoadContent     ()
        {
            try
            {
                this.Content.RootDirectory = "content";

                this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

                this.samplerState.Filter = TextureFilter.Linear;

                this.crashSprite.LoadContent(this.Content);
                this.crashSong  = this.Content.Load<Song>("sounds/music/crash");

                this.debugFont = this.Content.Load<SpriteFont>("fonts/debugfont");

                base.LoadContent();

                this.GraphicsDevice.SamplerStates[0] = this.samplerState;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.Message;

                this.gamecrashed = true;
            }
        }
        protected override void         UnloadContent   ()
        {
            try
            {
                this.crashSprite.UnloadContent();
                this.crashSong.Dispose();

                this.menumain.UnloadContent();

                base.UnloadContent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.Message;

                this.gamecrashed = true;
            }
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
                    MediaPlayer.Play(this.crashSong);

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

                    this.spriteBatch.Begin();

                    this.spriteBatch.DrawString(this.debugFont, string.Format("FPS: {0}", Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds, 0)), new Vector2(20, 510), Color.Black);

                    this.spriteBatch.End();
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
