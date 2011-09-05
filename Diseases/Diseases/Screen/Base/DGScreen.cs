using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diseases.Screen
{
    public abstract class DGScreen
    {
        private string          name;
        public  string          Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        private ContentManager  content;
        public  ContentManager  Content
        {
            get { return this.content; }
        }

        public                      DGScreen        ()
        {
            this.Initialize();
        }

        protected virtual   void    Initialize      ()
        {

        }

        public virtual      void    LoadContent     ()
        {

        }
        public virtual      void    UnloadContent   ()
        {

        }

        public virtual      void    Update          (GameTime gametime)
        {

        }
        public virtual      void    Render          (SpriteBatch batch)
        {

        }
    }
}
