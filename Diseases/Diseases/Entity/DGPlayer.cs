using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Physics;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

namespace Diseases.Entity
{
    public class DGPlayer : DGEntity
    {
        FixedMouseJoint fixture;
        Vector2         fVector = Vector2.Zero;

        protected   override void   Initialize          ()
        {
            this.restitution = 0;
            this.maxLife = 1;
            this.speed = 6;

            this.sprite = new DGSpriteAnimat("entities/bacteria/idle", 12, 10);
        }

        public      override void   LoadContent         (ContentManager content, World physics)
        {
            base.LoadContent(content, physics);

            this.physics.Position = ConvertUnits.ToSimUnits(50, 50);
            this.physics.CollisionCategories = Category.Cat1;
            this.physics.CollidesWith = Category.Cat1;

            fixture = new FixedMouseJoint(this.physics, ConvertUnits.ToSimUnits(new Vector2(50)));
            fixture.MaxForce = float.MaxValue;
                
            physics.AddJoint(fixture);
        }

        public      override void   HandleInput         (GameTime gametime, DGInput input)
        {
            fixture.WorldAnchorB = ConvertUnits.ToSimUnits(new Vector2(input.CurMouseState.X, input.CurMouseState.Y));
        }
        public      override void   Update              (GameTime gametime)
        {
            base.Update(gametime);

            if (this.wastedLife == 6)
            {
                this.sprite.Tint = Color.White;
                this.speed = 3;
            }

            if (this.wastedLife == 7)
            {
                this.sprite.Tint = Color.Orange;
                this.speed = 6;
            }

            this.wastedLife = (int)MathHelper.Clamp(this.wastedLife, 0, 10);

            float fx = this.physics.LinearVelocity.X;
            float fy = this.physics.LinearVelocity.Y;

            fx = MathHelper.Clamp(fx, -this.speed, this.speed);
            fy = MathHelper.Clamp(fy, -this.speed, this.speed);

            this.fVector.X = fx;
            this.fVector.Y = fy;

            this.physics.LinearVelocity = this.fVector;
        }

        public      bool            Dead                ()
        {
            return this.wastedLife == this.maxLife;
        }

        public      void            Damage              ()
        {
            if (!this.cooldownActive)
            {
                this.cooldownElapsed = 0;
                this.cooldownActive = true;

                this.wastedLife++;
            }
        }
        public      void            Healed              ()
        {
            this.cooldownActive = false;
            this.wastedLife--;
        }
    }
}
