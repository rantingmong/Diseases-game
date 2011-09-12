using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Graphics;
using Diseases.Screen.Level;

namespace Diseases.Screen.Menu
{
    public class MenuMain : DGMenuScreen
    {
        MenuHigh smenuhigh = new MenuHigh();
        MenuSett smenusett = new MenuSett();
        MenuExit smenuexit = new MenuExit();
        LevelGamePlay play = new LevelGamePlay();

        DGMenuEntry menuplay = new DGMenuEntry("play", "entities/menuentries/mainplay", false)
        {
            Location = new Vector2(50, 350)
        };
        DGMenuEntry menuhigh = new DGMenuEntry("play", "entities/menuentries/mainhigh", false)
        {
            Location = new Vector2(215, 350)
        };
        DGMenuEntry menusett = new DGMenuEntry("play", "entities/menuentries/mainsett", false)
        {
            Location = new Vector2(380, 350)
        };
        DGMenuEntry menuexit = new DGMenuEntry("exit", "entities/menuentries/mainexit", false)
        {
            Location = new Vector2(545, 350)
        };

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/main");

        public MenuMain()
            : base("main")
        {
            menuexit.Selected += new EventHandler<EventArgs>(menuexit_Selected);
            menuhigh.Selected += new EventHandler<EventArgs>(menuhigh_Selected);
            menusett.Selected += new EventHandler<EventArgs>(menusett_Selected);
            menuplay.Selected += new EventHandler<EventArgs>(menuplay_Selected);
        }

        void menuplay_Selected(object sender, EventArgs e)
        {
            this.ScreenManager.SwitchScreen(this.play);
        }
        void menuhigh_Selected(object sender, EventArgs e)
        {
            this.ScreenManager.AddScreen(this.smenuhigh);
        }
        void menusett_Selected(object sender, EventArgs e)
        {
            this.ScreenManager.AddScreen(this.smenusett);
        }
        void menuexit_Selected(object sender, EventArgs e)
        {
            this.OnCancel();
        }

        protected override void OnCancel()
        {
            this.ScreenManager.AddScreen(this.smenuexit);
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
