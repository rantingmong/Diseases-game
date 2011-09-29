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
    public class MenuOver : DGMenuScreen
    {
        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/over");

        DGMenuEntry rtryEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/over/rtry"), new DGSpriteStatic("entities/menubuttons/over/rtry_selt"))
        {
            Location = new Vector2(200, 290)
        };
        DGMenuEntry mainEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/over/main"), new DGSpriteStatic("entities/menubuttons/over/main_selt"))
        {
            Location = new Vector2(200, 350)
        };

        public MenuOver()
            : base("game over")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.rtryEntry);
            this.MenuList.Add(this.mainEntry);

            this.rtryEntry.Selected += (o, s) =>
            {
                this.ScreenManager.SwitchScreen(new LevelGamePlay());
            };
            this.mainEntry.Selected += (o, s) =>
            {
                this.ScreenManager.SwitchScreen(new MenuMain(true));
            };
        }
        protected override void OnCancel()
        {
            this.ScreenManager.SwitchScreen(new MenuMain());
        }
    }
}
