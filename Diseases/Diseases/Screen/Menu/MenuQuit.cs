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
    public class MenuQuit : DGMenuScreen
    {
        DGMenuEntry cnfmEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/quit/cnfm"), new DGSpriteStatic("entities/menubuttons/quit/cnfm_selt"))
        {
            Location = new Vector2(200, 240)
        };
        DGMenuEntry backEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/quit/back"), new DGSpriteStatic("entities/menubuttons/quit/back_selt"))
        {
            Location = new Vector2(200, 290)
        };

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/quit");

        public MenuQuit()
            : base("quit")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.cnfmEntry);
            this.MenuList.Add(this.backEntry);

            this.cnfmEntry.Selected += (s, o) =>
            {
                this.ScreenManager.Game.Exit();
            };
            this.backEntry.Selected += (s, o) =>
            {
                this.ScreenManager.RemoveScreen(this);
            };
        }
        protected override void OnCancel()
        {
            this.ScreenManager.RemoveScreen(this);
        }
    }
}
