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
    public class MenuHigh : MenuScreen
    {
        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/high");

        public MenuHigh()
            : base("high scores")
        {

        }

        protected override void OnCancel()
        {
            this.ScreenManager.RemoveScreen(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.BackgroundList.Add(background);
        }
    }
}
