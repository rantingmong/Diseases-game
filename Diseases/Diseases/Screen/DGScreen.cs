using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Diseases.Input;
using System.Diagnostics;

namespace Diseases.Screen
{
    public abstract class DGScreen
    {
        bool                        popup           = false;
        public bool                 PopupScreen
        {
            get { return this.popup; }
            set { this.popup = value; }
        }

        bool                        unloadonremove  = false;
        public bool                 UnloadOnRemove
        {
            get { return this.unloadonremove; }
            set { this.unloadonremove = value; }
        }

        bool                        overrideinput   = false;
        public bool                 OverrideInput
        {
            get { return this.overrideinput; }
            set { this.overrideinput = value; }
        }

        DGScreenManager             screenManager;
        public DGScreenManager      ScreenManager
        {
            get { return this.screenManager; }
            internal set { this.screenManager = value; }
        }

        public                      DGScreen        ()
        {
            Debug.WriteLine("initializing screen " + this.ToString(), "INFO");

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

        public      virtual void    Exit            ()
        {
            this.screenManager.RemoveScreen(this);
        }
    }
}
