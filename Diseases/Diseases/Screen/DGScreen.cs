using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;

using Diseases.Input;

namespace Diseases.Screen
{
    public abstract class DGScreen
    {
        #region FIELDS

        bool                        allowupdateinbackground = false;
        public bool                 AllowUpdateInBackground
        {
            get { return this.allowupdateinbackground; }
            set { this.allowupdateinbackground = value; }
        }

        bool                        unloadonremove          = false;
        public bool                 UnloadOnRemove
        {
            get { return this.unloadonremove; }
            set { this.unloadonremove = value; }
        }

        bool                        overrideinput           = false;
        public bool                 OverrideInput
        {
            get { return this.overrideinput; }
            set { this.overrideinput = value; }
        }

        internal bool               loaded                  = false;

        DGScreenManager             screenManager;
        public DGScreenManager      ScreenManager
        {
            get { return this.screenManager; }
            internal set { this.screenManager = value; }
        }

        #endregion 

        #region CTOR

        public                      DGScreen        ()
        {
            Debug.WriteLine("initializing screen " + this.ToString(), "INFO");

            this.Initialize();
        }

        #endregion

        #region METHODS

        protected   virtual void    Initialize      ()
        {

        }
        
        public      virtual void    LoadContent     ()
        {
            this.loaded = true;
            this.screenManager.isloading = false;
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

        #endregion
    }
}
