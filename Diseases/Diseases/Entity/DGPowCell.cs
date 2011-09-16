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

namespace Diseases.Entity
{
    public class DGPowCell : DGEntity
    {
        Random      randomizer;
        Vector2     forceVector;

        public                      DGPowCell   (Random randomizer)
        {
            this.randomizer = randomizer;
        }
        protected   override void   Initialize  ()
        {
            this.restitution = 1.5f;
            this.speed = 4;

            this.sprite = new DGSpriteAnimat("entities/powerup/idle", 10, 8);
        }

        public      override void   LoadContent (ContentManager content, World physics)
        {
            base.LoadContent(content, physics);

            this.physics.CollisionCategories = Category.Cat4;
            this.physics.CollidesWith = Category.Cat4;

            this.physics.Position = ConvertUnits.ToSimUnits(new Vector2(randomizer.Next(40, 760), randomizer.Next(40, 500)));
            this.physics.ApplyLinearImpulse(new Vector2((float)Math.Cos(randomizer.Next()), (float)Math.Sin(randomizer.Next())) * this.speed);
        }

        public      override void   Update      (GameTime gametime)
        {
            base.Update(gametime);

            float fx = MathHelper.Clamp(this.physics.LinearVelocity.X, -this.speed, this.speed);
            float fy = MathHelper.Clamp(this.physics.LinearVelocity.Y, -this.speed, this.speed);

            if (fx > 0)
                fx = fx + (this.speed - fx);
            else
                fx = fx - (this.speed + fx);

            if (fy > 0)
                fy = fy + (this.speed - fy);
            else
                fy = fy - (this.speed + fy);

            this.forceVector.X = fx;
            this.forceVector.Y = fy;

            this.physics.LinearVelocity = this.forceVector;
        }

        public void Eaten()
        {
            this.dead = true;
        }
    }
}
