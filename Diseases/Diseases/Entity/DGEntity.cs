using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Graphics;

namespace Diseases.Entity
{
    public abstract class DGEntity
    {
        protected int   life = 10;
        protected bool  dead = false;

        protected float     rotation    = 0;
        protected Vector2   speed       = Vector2.Zero;

        public DGEntity()
        {

        }

        protected virtual void Initialize()
        {

        }

        public virtual void LoadContent(ContentManager content)
        {

        }
        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gametime)
        {

        }
        public virtual void Render(SpriteBatch batch)
        {

        }
    }
}
