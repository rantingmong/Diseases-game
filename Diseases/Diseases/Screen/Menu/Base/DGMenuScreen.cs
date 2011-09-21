using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Graphics;

namespace Diseases.Screen.Menu
{
    public class DGMenuScreen : DGScreen
    {
        int                         selectedentry   = 0;
 
        SoundEffect                 buttonMove;

        DGInputAction               menuUp;
        DGInputAction               menuLf;
        DGInputAction               menuRt;
        DGInputAction               menuBt;
        
        DGInputAction               menuEx;
        DGInputSequence             menuSt;

        internal bool               loading         = false;

        internal string             menuname;

        List<DGMenuEntry>           tempmenu;

        List<DGMenuEntry>           menus           = new List<DGMenuEntry>();
        public List<DGMenuEntry>    MenuList
        {
            get { return this.menus; }
        }

        List<IDGSprite>             background      = new List<IDGSprite>();
        public List<IDGSprite>      BackgroundList
        {
            get { return this.background; }
        }

        public event EventHandler   MenuCanceled;

        public                      DGMenuScreen    (string menuname)
        {
            this.menuname = menuname;
            this.tempmenu = new List<DGMenuEntry>();

            this.menuUp = new DGInputAction(Keys.Up, 
                true)
            {
                inputname = "up key",
            };
            this.menuLf = new DGInputAction(Keys.Left, 
                true)
            {
                inputname = "left key",
            };
            this.menuRt = new DGInputAction(Keys.Right, 
                true)
            {
                inputname = "right key",
            };
            this.menuBt = new DGInputAction(Keys.Down, 
                true)
            {
                inputname = "down key",
            };

            this.menuSt = new DGInputSequence(new Keys[] { Keys.Space, Keys.Enter },
                true,
                false)
            {
                inputname = "space key",
            };
            this.menuEx = new DGInputAction(Keys.Escape, 
                true)
            {
                inputname = "escape key",
            };
        }

        protected virtual void      OnSelect        (int entryIndex)
        {
            if (this.menus.Count >= 1)
                this.menus[entryIndex].OnSelected();            
        }
        protected virtual void      OnCancel        ()
        {
            throw new NotImplementedException("Please specify a cancel event for this menu.");
        }

        public override void        LoadContent     ()
        {
            this.buttonMove = this.ScreenManager.Content.Load<SoundEffect>("sounds/buttonpress");

            foreach (DGMenuEntry menu in this.menus)
                menu.LoadContent(this.ScreenManager.Content);

            foreach (IDGSprite background in this.background)
                background.LoadContent(this.ScreenManager.Content);

            base.LoadContent();
        }
        public override void        UnloadContent   ()
        {
            foreach (DGMenuEntry menu in this.menus)
                menu.UnloadContent();

            foreach (IDGSprite background in this.background)
                background.UnloadContent();

            this.buttonMove.Dispose();
        }

        public override void        HandleInput     (GameTime gametime, DGInput input)
        {
            foreach (DGMenuEntry entry in this.menus)
            {
                if (input.TestLocation(entry.Bounds))
                {
                    this.selectedentry = this.menus.IndexOf(entry);

                    break;
                }
            }

            if (this.menus.Count > 1)
            {
                bool navikeypress = false;

                if (menuUp.Evaluate(input))
                {
                    DGMenuEntry selectedentry = this.menus[this.selectedentry];
                    foreach (DGMenuEntry entry in this.menus)
                    {
                        if (entry != selectedentry && entry.Location.Y < selectedentry.Location.Y)
                            this.tempmenu.Add(entry);
                    }

                    navikeypress = true;
                }

                if (menuLf.Evaluate(input))
                {
                    DGMenuEntry selectedentry = this.menus[this.selectedentry];
                    foreach (DGMenuEntry entry in this.menus)
                    {
                        if (entry != selectedentry && entry.Location.X < selectedentry.Location.X)
                            this.tempmenu.Add(entry);
                    }

                    navikeypress = true;
                }

                if (menuRt.Evaluate(input))
                {
                    DGMenuEntry selectedentry = this.menus[this.selectedentry];
                    foreach (DGMenuEntry entry in this.menus)
                    {
                        if (entry != selectedentry && entry.Location.X > selectedentry.Location.X)
                            this.tempmenu.Add(entry);
                    }

                    navikeypress = true;
                }

                if (menuBt.Evaluate(input))
                {
                    DGMenuEntry selectedentry = this.menus[this.selectedentry];
                    foreach (DGMenuEntry entry in this.menus)
                    {
                        if (entry != selectedentry && entry.Location.Y > selectedentry.Location.Y)
                            this.tempmenu.Add(entry);
                    }

                    navikeypress = true;
                }

                if (this.tempmenu.Count >= 1 && navikeypress)
                {
                    int i = 0;
                    double[] heuristic = new double[this.tempmenu.Count];

                    DGMenuEntry selectedentry = this.menus[this.selectedentry];
                    foreach (DGMenuEntry entry in this.tempmenu)
                    {
                        heuristic[i++] = Math.Sqrt(Math.Pow(selectedentry.Location.X - entry.Location.X, 2) + Math.Pow(selectedentry.Location.Y - entry.Location.Y, 2));
                    }

                    int nxtslct = this.selectedentry;
                    double nearest = double.MaxValue;
                    for (int d = 0; d < heuristic.Length; d++)
                    {
                        if (nearest > heuristic[d])
                        {
                            nearest = heuristic[d];
                            nxtslct = d;
                        }
                    }

                    this.selectedentry = this.menus.IndexOf(this.tempmenu[nxtslct]);

                    this.tempmenu.Clear();
                }
            }

            if (menuSt.Evaluate(input) && this.menus.Count > 1 || input.TestLeftButton())
            {
                this.buttonMove.Play();
                OnSelect(this.selectedentry);
            }
            
            if (menuEx.Evaluate(input))
            {
                OnCancel();

                if (this.MenuCanceled != null)
                    this.MenuCanceled(this, EventArgs.Empty);
            }
        }

        public override void        Update          (GameTime gametime)
        {
            for (int i = 0; i < this.background.Count; i++)
                this.background[i].Update(gametime);

            for (int i = 0; i < this.menus.Count; i++)
                this.menus[i].Update(gametime, i == this.selectedentry);
        }
        public override void        Render          (SpriteBatch batch)
        {
            for (int i = 0; i < this.background.Count; i++)
                this.background[i].Render(batch);

            for (int i = 0; i < this.menus.Count; i++)
                this.menus[i].Render(batch, i == this.selectedentry);
        }
    }
}
