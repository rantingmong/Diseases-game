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
    public class MenuExit : DGMenuScreen
    {
        DGMenuEntry buttonok = new DGMenuEntry("OK", "entities/menuentries/ok", false)
        {
            Location = new Vector2(325, 220),
        };
        DGMenuEntry buttonno = new DGMenuEntry("OK", "entities/menuentries/no", false)
        {
            Location = new Vector2(325, 300),
        };

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/exit");

        public MenuExit()
            : base("exit")
        {
            this.PopupScreen = true;

            this.buttonno.Selected += new EventHandler<EventArgs>(buttonno_Selected);
            this.buttonok.Selected += new EventHandler<EventArgs>(buttonok_Selected);
        }

        void buttonok_Selected(object sender, EventArgs e)
        {
            this.ScreenManager.Game.Exit();
        }
        void buttonno_Selected(object sender, EventArgs e)
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

            this.MenuList.Add(buttonok);
            this.MenuList.Add(buttonno);

            this.BackgroundList.Add(background);
        }
    }
}
