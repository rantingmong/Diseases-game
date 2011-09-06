using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Diseases.Input;

namespace Diseases.Screen
{
    public abstract class DGScreen
    {
        bool                        overrideinput = false;
        public bool                 OverrideInput
        {
            get { return this.overrideinput; }
            set { this.overrideinput = value; }
        }

        DGScreenManager             screenManager;
        public DGScreenManager      ScreenManager
        {
            get { return this.screenManager; }
        }

        public                      DGScreen        (DGScreenManager screenManager)
        {
            this.screenManager = screenManager;

            this.Initialize();
        }

        protected   virtual void    Initialize      ()
        {

        }
        
        public      virtual void    LoadContent     ()
        {

        }
        public      virtual void    UnloadContent   ()
        {

        }

        public      virtual void    HandleInput     (GameTime gametime, DGInput input)
        {

        }

        public      virtual void    Update          (GameTime gametime)
        {

        }
        public      virtual void    Render          (SpriteBatch batch)
        {

        }
    }
}
