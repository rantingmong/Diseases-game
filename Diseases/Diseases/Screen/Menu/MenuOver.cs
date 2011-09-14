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
        int score = 0;

        DGMenuEntry resart = new DGMenuEntry("retry", "entities/menuentries/restart", false);
        DGMenuEntry quit = new DGMenuEntry("quit", "entities/menuentries/quit", false);

        DGSpriteStatic highScore = new DGSpriteStatic("entities/high");
        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/over");

        public MenuOver(int score)
            : base("game over")
        {

        }

        protected override void OnCancel()
        {
            this.ScreenManager.SwitchScreen(new MenuMain());
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.BackgroundList.Add(this.background);
        }
    }
}
