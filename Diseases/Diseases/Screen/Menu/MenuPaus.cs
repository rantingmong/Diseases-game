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
    public class MenuPaus : DGMenuScreen
    {
        MenuSett settings = new MenuSett();

        DGMenuEntry buttonresm = new DGMenuEntry("resume", "entities/menuentries/resm", false)
        {
            Location = new Vector2(325, 220)
        };
        DGMenuEntry buttonoptn = new DGMenuEntry("option", "entities/menuentries/mainsett", false)
        {
            Location = new Vector2(325, 300)
        };
        DGMenuEntry buttonquit = new DGMenuEntry("quit", "entities/menuentries/quit", false)
        {
            Location = new Vector2(325, 380)
        };

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/paus");

        public MenuPaus()
            : base("menu")
        {
            this.PopupScreen = true;

            this.buttonresm.Selected += new EventHandler<EventArgs>(buttonresm_Selected);
            this.buttonoptn.Selected += new EventHandler<EventArgs>(buttonoptn_Selected);
            this.buttonquit.Selected += new EventHandler<EventArgs>(buttonquit_Selected);
        }

        void buttonquit_Selected(object sender, EventArgs e)
        {
            this.ScreenManager.SwitchScreen(new MenuMain());
        }
        void buttonoptn_Selected(object sender, EventArgs e)
        {
            this.ScreenManager.AddScreen(this.settings);
        }
        void buttonresm_Selected(object sender, EventArgs e)
        {
            this.OnCancel();
        }

        protected override void OnCancel()
        {
            this.ScreenManager.RemoveScreen(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.MenuList.Add(buttonresm);
            this.MenuList.Add(buttonoptn);
            this.MenuList.Add(buttonquit);

            this.BackgroundList.Add(background);
        }
    }
}
