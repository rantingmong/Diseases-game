using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Diseases.Level;

namespace Diseases.Entity
{
    public abstract class DGEntity
    {
        private float       angle;

        private Vector2     location;
        private Vector2     direction;
        private Vector2     acceleration;

        private int         life;
        private bool        isdead;

        public int          EntityLife
        {
            get { return this.life; }
            protected set { this.life = value; }
        }
        public bool         EntityDead
        {
            get { return this.isdead; }
            protected set { this.isdead = value; }
        }

        private DGLevel     levelparent;

        public DGLevel      ParentLevel
        {
            get { return this.levelparent; }
        }

        public              DGEntity        (DGLevel parent)
        {

        }

        public virtual void LoadContent     ()
        {
        }
        public virtual void UnloadContent   ()
        {
        }

        public virtual void Update          (GameTime gametime)
        {
        }
        public virtual void Render          (GameTime gametime)
        {
        }
    }
}
