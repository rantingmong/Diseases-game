using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Diseases.Entity
{
    public abstract class DGEntity
    {
        private     Body            physics;

        protected   int             restitution     = 10;
        protected   float           speed           = 1.5f;

        protected   int             life            = 10;
        public      int             EntityLife
        {
            get { return this.life; }
            set { this.life = value; }
        }

        protected   IDGSprite       sprite;

        public                      DGEntity        ()
        {
            this.Initialize();
        }

        protected   virtual void    Initialize      ()
        {
            
        }

        public      virtual void    LoadContent     (ContentManager content, World physics)
        {
            this.sprite.LoadContent(content);

            this.physics = BodyFactory.CreateCircle(physics, sprite.Texture.Width / 2, 0, Vector2.Zero);
            this.physics.Restitution = restitution;
        }
        public      virtual void    UnloadContent   ()
        {
            this.sprite.UnloadContent();

            this.physics.Dispose();
        }

        public      virtual void    HandleInput     (GameTime gametime, DGInput input)
        {

        }

        public      virtual void    Update          (GameTime gametime)
        {
            this.sprite.Update(gametime);
        }
        public      virtual void    Render          (SpriteBatch batch)
        {
            this.sprite.Render(batch);
        }
    }
}
