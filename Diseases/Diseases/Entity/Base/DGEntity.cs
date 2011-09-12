using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Physics;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Diseases.Entity
{
    public abstract class DGEntity
    {
        protected   Body            physics;

        protected   float           restitution     = 0.8f;
        protected   float           speed           = 1.5f;

        protected   int             life            = 10;
        public      int             EntityLife
        {
            get { return this.life; }
            set { this.life = value; }
        }

        protected   IDGSprite       sprite;

        Vector2 offset = Vector2.Zero;

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

            this.physics = BodyFactory.CreateCircle(physics, ConvertUnits.ToSimUnits(sprite.Texture.Width / 2), 0);
            this.physics.BodyType = BodyType.Dynamic;
            this.physics.Restitution = this.restitution;

            offset = new Vector2(sprite.Texture.Width / 2);
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
            this.sprite.Location = ConvertUnits.ToDisplayUnits(this.physics.Position);
            this.sprite.Rotation = this.physics.Rotation;
            this.sprite.Offset = offset;

            this.sprite.Render(batch);
        }
    }
}
