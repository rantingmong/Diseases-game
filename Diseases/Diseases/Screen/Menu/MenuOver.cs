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
    public class MenuOver : DGMenuScreen
    {
        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/over");

        public MenuOver()
            : base("game over")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);
        }
        protected override void OnCancel()
        {
            this.ScreenManager.SwitchScreen(new MenuMain());
        }
    }
}
