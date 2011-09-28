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
        DGMenuEntry playEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/paus/play"), new DGSpriteStatic("entities/menubuttons/paus/play_selt"))
        {
            Location = new Vector2(400, 140)
        };
        DGMenuEntry scorEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/paus/high"), new DGSpriteStatic("entities/menubuttons/paus/high_selt"))
        {
            Location = new Vector2(400, 190)
        };
        DGMenuEntry quitEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/paus/quit"), new DGSpriteStatic("entities/menubuttons/paus/quit_selt"))
        {
            Location = new Vector2(400, 240)
        };

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/paus");

        public MenuPaus()
            : base("pause")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.playEntry);
            this.MenuList.Add(this.scorEntry);
            this.MenuList.Add(this.quitEntry);

            this.playEntry.Selected += (o, s) =>
            {
                this.ScreenManager.RemoveScreen(this);
            };
            this.scorEntry.Selected += (o, s) =>
            {

            };
            this.quitEntry.Selected += (o, s) =>
            {
                this.ScreenManager.SwitchScreen(new MenuMain(true));
            };
        }
        protected override void OnCancel()
        {
            this.ScreenManager.RemoveScreen(this);
        }
    }
}
