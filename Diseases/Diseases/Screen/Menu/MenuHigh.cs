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
    public class MenuHigh : DGMenuScreen
    {
        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/high");

        DGMenuEntry backEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/scor/back"), new DGSpriteStatic("entities/menubuttons/scor/back_selt"))
        {
            Location = new Vector2(20, 490)
        };
        DGMenuEntry rsetEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/scor/rset"), new DGSpriteStatic("entities/menubuttons/scor/rset_selt"))
        {
            Location = new Vector2(220, 490)
        };
        public MenuHigh()
            : base("high scores")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.backEntry);

            this.backEntry.Selected += (o, s) => //x
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
