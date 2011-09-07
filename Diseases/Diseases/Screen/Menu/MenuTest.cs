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
    public class MenuMain : MenuScreen
    {
        DGMenuEntry menuplay = new DGMenuEntry("play", "entities/menuentries/mainplay", false)
        {
            Location = new Vector2(55, 96)
        };
        DGMenuEntry menuhigh = new DGMenuEntry("play", "entities/menuentries/mainhigh", false)
        {
            Location = new Vector2(55, 367)
        };
        DGMenuEntry menusett = new DGMenuEntry("play", "entities/menuentries/mainsett", false)
        {
            Location = new Vector2(486, 95)
        };
        DGMenuEntry menuexit = new DGMenuEntry("exit", "entities/menuentries/mainexit", false)
        {
            Location = new Vector2(486, 367)
        };

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/main");

        public MenuMain()
            : base("menu test")
        {
            menuexit.Selected += new EventHandler<EventArgs>(menuexit_Selected);
        }

        void menuexit_Selected(object sender, EventArgs e)
        {
            this.ScreenManager.Game.Exit();
        }

        protected override void OnCancel()
        {
            base.OnCancel();

            this.ScreenManager.Game.Exit();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.MenuList.Add(menuplay);
            this.MenuList.Add(menuexit);
            this.MenuList.Add(menuhigh);
            this.MenuList.Add(menusett);

            this.BackgroundList.Add(background);
        }
    }
}
