using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Graphics;

using Diseases.Screen.Other;

namespace Diseases.Screen
{
    public class DGScreenManager : DrawableGameComponent
    {
        internal bool               isloading       = false;

        bool                        inputhandled    = false;
        bool                        isinitialized   = false;

        DGScreenLoad                loadScreen;
        DGInputSequence             crashinput;

        SpriteBatch                 spritebatch;

        ContentManager              content;
        public ContentManager       Content
        {
            get { return this.content; }
        }

        DGInput                     input;
        public DGInput              Input
        {
            get { return this.input; }
        }

        List<DGScreen>              screens         = new List<DGScreen>();
        public List<DGScreen>       Screens
        {
            get { return this.screens; }
        }

        List<DGScreen>              tempscreens     = new List<DGScreen>();

        public                      DGScreenManager (DiseasesGame game)
            : base(game)
        {

        }

        public      override void   Initialize      ()
        {
            Debug.WriteLine("initializing screen manager...", "INFO");

            this.content = new ContentManager(this.Game.Services);
            this.content.RootDirectory = "content";

            this.crashinput = new DGInputSequence(new Keys[] { Keys.LeftControl, Keys.F12 }, false, true);
            this.crashinput.inputname = "crashinput";

            this.loadScreen = new DGScreenLoad(new DGSpriteAnimat("backgrounds/load", 4, 4));
            this.loadScreen.ScreenManager = this;

            base.Initialize();

            this.isinitialized = true;
        }

        protected   override void   LoadContent     ()
        {
            this.input = new DGInput();
            this.spritebatch = new SpriteBatch(this.Game.GraphicsDevice);

            this.loadScreen.LoadContent();

            foreach (DGScreen screen in this.screens)
            {
                Debug.WriteLine("loading screen " + screen.ToString(), "INFO");

                screen.LoadContent();
            }
        }
        protected   override void   UnloadContent   ()
        {
            foreach (DGScreen screen in this.screens)
                screen.UnloadContent();

            this.loadScreen.UnloadContent();
        }

        public      override void   Update          (GameTime gameTime)
        {
            this.input.Update(gameTime);
            
            if (!this.isloading)
            {
                if (this.crashinput.Evaluate(this.input))
                    throw new NullReferenceException("test crash input!");

                this.tempscreens.Clear();

                foreach (DGScreen screen in this.screens)
                    this.tempscreens.Add(screen);

                while (this.tempscreens.Count > 0)
                {
                    DGScreen screen = this.tempscreens[this.tempscreens.Count - 1];

                    this.tempscreens.RemoveAt(this.tempscreens.Count - 1);

                    if (!this.inputhandled || screen.OverrideInput)
                    {
                        screen.Update(gameTime);
                        screen.HandleInput(gameTime, input);

                        this.inputhandled = true;
                    }
                }

                this.inputhandled = false;
            }
            else
            {
                this.loadScreen.Update(gameTime);
            }
        }
        public      override void   Draw            (GameTime gameTime)
        {
            if (!this.isloading)
            {
                this.tempscreens.Clear();

                foreach (DGScreen screen in this.screens)
                    this.tempscreens.Add(screen);

                for (int i = 0; i < this.tempscreens.Count; i++)
                {
                    DGScreen screen = this.tempscreens[i];

                    this.spritebatch.Begin();

                    screen.Render(this.spritebatch);

                    this.spritebatch.End();
                }
            }
            else
            {
                this.spritebatch.Begin();

                this.loadScreen.Render(this.spritebatch);

                this.spritebatch.End();
            }
        }

        public      void            AddScreen       (DGScreen screen)
        {
            if (!this.screens.Contains(screen))
            {
                screen.ScreenManager = this;

                if (this.isinitialized && !screen.loaded)
                    screen.LoadContent();

                this.screens.Add(screen);
            }
        }
        public      void            RemoveScreen    (DGScreen screen)
        {
            if (this.screens.Contains(screen))
            {
                if (screen.UnloadOnRemove)
                    screen.UnloadContent();

                this.screens.Remove(screen);
            }
        }

        public      void            SwitchScreen    (DGScreen screen)
        {
            this.isloading = true;

            DGScreen[] templist = this.screens.ToArray();

            foreach (DGScreen s in templist)
                this.RemoveScreen(s);

            screen.ScreenManager = this;

            if (this.isinitialized)
            {
                Debug.WriteLine("loading screen " + screen.ToString(), "INFO");

                Thread loadThread = new Thread(screen.LoadContent)
                {
                    Name            = "load thread",
                    IsBackground    = false
                };
                loadThread.Start();
            }

            this.screens.Add(screen);
        }
    }
}
