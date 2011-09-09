using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Graphics;

namespace Diseases.Screen.Util
{
    public class LoadScreen : DGScreen
    {
        public IDGSprite background;

        public LoadScreen()
        {
            background = new DGSpriteStatic("backgrounds/load");
        }

        public override void LoadContent()
        {
            background.LoadContent(this.ScreenManager.Content);
        }

        public override void Render(SpriteBatch batch)
        {
            background.Render(batch);
        }
    }
}
