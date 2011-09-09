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
    public class MenuSett : MenuScreen
    {
        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/sett");

        public MenuSett()
            : base("settings")
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
