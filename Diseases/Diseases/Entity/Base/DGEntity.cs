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
        public      Body            physics;

        protected   float           restitution     = 0.8f;
        protected   float           speed           = 1.5f;

        protected   int             life            = 10;
        public      int             EntityLife
        {
            get { return this.life; }
            set { this.life = value; }
        }

        protected   IDGSprite       sprite;

        protected   Vector2         offset          = Vector2.Zero;

        Vector2                     location        = Vector2.Zero;
        public      Vector2         Location
        {
            get { return ConvertUnits.ToDisplayUnits(this.physics.Position); }
            set { this.physics.Position = ConvertUnits.ToSimUnits(value); }
        }

        protected   Rectangle       bounds;
        public      Rectangle       Bounds
        {
            get
            {
                this.bounds.X = (int)Math.Round(ConvertUnits.ToDisplayUnits(this.physics.Position.X), 0);
                this.bounds.Y = (int)Math.Round(ConvertUnits.ToDisplayUnits(this.physics.Position.Y), 0);
             
                return this.bounds;
            }
            set { this.bounds = value; }
        }

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

            this.physics = BodyFactory.CreateCircle(physics, ConvertUnits.ToSimUnits(sprite.Texture.Height / 2), 0);
            this.physics.BodyType = BodyType.Dynamic;
            this.physics.Restitution = this.restitution;

            this.offset = new Vector2(sprite.Texture.Height / 2);

            this.bounds = new Rectangle(0, 0, (int)this.sprite.Texture.Height, (int)this.sprite.Texture.Height);
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

            this.sprite.Location = ConvertUnits.ToDisplayUnits(this.physics.Position);
            this.sprite.Rotation = this.physics.Rotation;
            this.sprite.Offset = offset;
        }
        public      virtual void    Render          (SpriteBatch batch)
        {
            this.sprite.Render(batch);
        }
    }
}
