using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Graphics;

namespace Diseases.Screen.Menu
{
    public class MenuScreen : DGScreen
    {
        int selectedentry = 0;

        internal string menuname;

        List<DGMenuEntry> tempmenu;

        List<DGMenuEntry> menus = new List<DGMenuEntry>();
        public List<DGMenuEntry>    MenuList
        {
            get { return this.menus; }
        }

        List<IDGSprite> background = new List<IDGSprite>();
        public List<IDGSprite>      BackgroundList
        {
            get { return this.background; }
        }

        DGInputAction menuUp;
        DGInputAction menuLf;
        DGInputAction menuRt;
        DGInputAction menuBt;
        
        DGInputAction menuSt;
        DGInputAction menuEx;

        public MenuScreen(string menuname)
        {
            this.menuname = menuname;

            this.menuUp = new DGInputAction(Keys.Up, false)
            {
                inputname = "up key",
            };
            this.menuLf = new DGInputAction(Keys.Left, false)
            {
                inputname = "left key",
            };
            this.menuRt = new DGInputAction(Keys.Right, false)
            {
                inputname = "right key",
            };
            this.menuBt = new DGInputAction(Keys.Down, false)
            {
                inputname = "down key",
            };

            this.menuSt = new DGInputAction(Keys.Space, false)
            {
                inputname = "space key",
            };
            this.menuEx = new DGInputAction(Keys.Escape, false)
            {
                inputname = "escape key",
            };
        }

        protected virtual void OnSelect(int entryIndex)
        {
            this.menus[entryIndex].OnSelected();
        }
        protected virtual void OnCancel()
        {
            this.Exit();
        }

        public override void LoadContent()
        {
            foreach (DGMenuEntry menu in this.menus)
                menu.LoadContent(this.ScreenManager.Content);

            foreach (IDGSprite background in this.background)
                background.LoadContent(this.ScreenManager.Content);
        }
        public override void UnloadContent()
        {
            foreach (DGMenuEntry menu in this.menus)
                menu.UnloadContent();

            foreach (IDGSprite background in this.background)
                background.UnloadContent();
        }

        public override void HandleInput(GameTime gametime, DGInput input)
        {
            tempmenu = this.menus;

            if (tempmenu != null)
            {
                if (menuUp.Evaluate(input))
                {
                    // sort the menu list from smallest to biggest y-location
                    tempmenu.Sort((DGMenuEntry m1, DGMenuEntry m2) =>
                    {
                        if (m1.Location.Y == m2.Location.Y)
                            return (int)(m1.Location.X - m2.Location.X);

                        return (int)(m1.Location.Y - m2.Location.Y);
                    });

                    this.selectedentry = (int)MathHelper.Clamp(tempmenu.IndexOf(this.menus[selectedentry]) - 1, 0, this.menus.Count - 1);
                }

                if (menuLf.Evaluate(input))
                {
                    // sort the menu list from smallest to biggest x-location
                    tempmenu.Sort((DGMenuEntry m1, DGMenuEntry m2) =>
                    {
                        if (m1.Location.X == m2.Location.X)
                            return (int)(m1.Location.Y - m2.Location.Y);

                        return (int)(m1.Location.X - m2.Location.X);
                    });

                    this.selectedentry = (int)MathHelper.Clamp(tempmenu.IndexOf(this.menus[selectedentry]) - 1, 0, this.menus.Count - 1);
                }

                if (menuRt.Evaluate(input))
                {
                    // sort the menu list from biggest to smallest x-location
                    tempmenu.Sort((DGMenuEntry m1, DGMenuEntry m2) =>
                    {
                        if (m1.Location.X == m2.Location.X)
                            return (int)(m1.Location.Y - m2.Location.Y);

                        return (int)(m2.Location.X - m1.Location.X);
                    });

                    this.selectedentry = (int)MathHelper.Clamp(tempmenu.IndexOf(this.menus[selectedentry]) - 1, 0, this.menus.Count - 1);
                }

                if (menuBt.Evaluate(input))
                {
                    // sort the menu list from biggest to smallest y-location
                    tempmenu.Sort((DGMenuEntry m1, DGMenuEntry m2) =>
                    {
                        if (m1.Location.Y == m2.Location.Y)
                            return (int)(m1.Location.X - m2.Location.X);

                        return (int)(m2.Location.Y - m1.Location.Y);
                    });

                    this.selectedentry = (int)MathHelper.Clamp(tempmenu.IndexOf(this.menus[selectedentry]) - 1, 0, this.menus.Count - 1);
                }
            }

            if (menuSt.Evaluate(input))
            {
                OnSelect(this.selectedentry);
            }

            if (menuEx.Evaluate(input))
            {
                OnCancel();
            }
        }

        public override void Update(GameTime gametime)
        {
            for (int i = 0; i < this.background.Count; i++)
                this.background[i].Update(gametime);

            for (int i = 0; i < this.menus.Count; i++)
                this.menus[i].Update(gametime, i == this.selectedentry);
        }
        public override void Render(SpriteBatch batch)
        {
            for (int i = 0; i < this.background.Count; i++)
                this.background[i].Render(batch);

            for (int i = 0; i < this.menus.Count; i++)
                this.menus[i].Render(batch, i == this.selectedentry);
        }
    }
}
