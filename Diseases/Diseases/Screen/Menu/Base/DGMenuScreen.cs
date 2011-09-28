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

        DGInputAction               menuEx;
        DGInputSequence             menuSt;

        internal string             menuname;

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

            this.menuSt = new DGInputSequence(new Keys[] { Keys.Space, Keys.Enter }, true, false)
            {
                inputname = "menu select",
            };
            this.menuEx = new DGInputAction(Keys.Escape, true)
            {
                inputname = "menu cancel",
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
            foreach (DGMenuEntry menu in this.menus)
                menu.LoadContent(this.ScreenManager.Content);

            foreach (IDGSprite background in this.background)
                background.LoadContent(this.ScreenManager.Content);

            base.LoadContent();
        }
        public override void        UnloadContent   ()
        {
            foreach (DGMenuEntry menu in this.menus)
                if (menu != null)
                    menu.UnloadContent();

            foreach (IDGSprite background in this.background)
                if (background != null)
                    background.UnloadContent();
        }

        public override void        HandleInput     (GameTime gametime, DGInput input)
        {
            bool selectionFound = false;

            foreach (DGMenuEntry entry in this.menus)
            {
                if (input.TestLocation(entry.Bounds))
                {
                    selectionFound = true;
                    this.selectedentry = this.menus.IndexOf(entry);

                    break;
                }
            }

            if (!selectionFound)
                this.selectedentry = -1;

            if (menuSt.Evaluate(input) && this.menus.Count > 1 || (input.TestLeftButton() && selectionFound))
            {
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
