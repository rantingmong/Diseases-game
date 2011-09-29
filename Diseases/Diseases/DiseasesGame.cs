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
        float                           updateTime      = 0;
        Vector2                         fpsPos          = new Vector2(20, 510);
        Vector2                         mstPos          = new Vector2(20, 480);

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

            //this.Window.Title = "Diseases - DEMO PURPOSES ONLY!";

            this.graphicsManager = new GraphicsDeviceManager(this);

            this.graphicsManager.PreferMultiSampling = true;

            this.graphicsManager.PreferredBackBufferWidth   = 800;
            this.graphicsManager.PreferredBackBufferHeight  = 540;

            this.Activated += (o, s) =>
            {
                if (MediaPlayer.State == MediaState.Paused)
                    MediaPlayer.Resume();
            };
            this.Deactivated += (o, s) =>
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Pause();
            };
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

                this.screenmanager.SwitchScreen(new MenuMain());

                this.Content.RootDirectory = "content";

                base.Initialize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.ToString();

                this.gamecrashed = true;
            }
        }

        protected override void         LoadContent     ()
        {
            try
            {
                this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

                this.debugFont = this.Content.Load<SpriteFont>("fonts/debugfont");

                this.samplerState.Filter = TextureFilter.Point;
                this.GraphicsDevice.SamplerStates[0] = this.samplerState;

                this.crashSprite.LoadContent(this.Content);
                this.crashSong = this.Content.Load<Song>("sounds/music/crash");

                base.LoadContent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.ToString();

                this.gamecrashed = true;
            }
        }
        protected override void         UnloadContent   ()
        {
            try
            {
                this.crashSprite.UnloadContent();
                this.crashSong.Dispose();

                base.UnloadContent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "STOP");
                Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                this.crshmessage = ex.ToString();

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

                        this.updateTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "STOP");
                        Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                        this.crshmessage = ex.ToString();

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

#if DEBUG

                    this.spriteBatch.Begin();

                    this.spriteBatch.DrawString(this.debugFont, string.Format("FPS: {0}", Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds, 0)), this.fpsPos + new Vector2(1), Color.Black);
                    this.spriteBatch.DrawString(this.debugFont, string.Format("FPS: {0}", Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds, 0)), this.fpsPos, Color.White);

                    this.spriteBatch.DrawString(this.debugFont, string.Format("MS: {0}", Math.Round(this.updateTime, 0)), this.mstPos + new Vector2(1), Color.Black);
                    this.spriteBatch.DrawString(this.debugFont, string.Format("MS: {0}", Math.Round(this.updateTime, 0)), this.mstPos, Color.White);

                    this.spriteBatch.End();

#endif
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "STOP");
                    Debug.WriteLine("stack trace\n" + ex.StackTrace, "STOP");

                    this.crshmessage = ex.ToString();

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
