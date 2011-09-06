using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Graphics;

namespace Diseases.Screen.Menu
{
    public class MenuMain : DGScreen
    {
        DGSpriteStatic background;

        public MenuMain (DGScreenManager manager)
            : base(manager)
        {

        }

        protected override void Initialize()
        {
            this.background = new DGSpriteStatic("backgrounds/null");
        }

        public override void LoadContent()
        {
            this.background.LoadContent(this.ScreenManager.Content);
        }

        public override void Update(GameTime gametime)
        {
            
        }

        public override void Render(SpriteBatch batch)
        {
            this.background.Render(batch);
        }
    }
}
