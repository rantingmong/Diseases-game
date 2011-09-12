using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Physics;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Diseases.Entity
{
    public class DGEnemy : DGEntity
    {
        Random randomizer;

        Vector2 fVector = Vector2.Zero;

        public DGEnemy(Random randomizer)
        {
            this.randomizer = randomizer;
        }

        protected override void Initialize()
        {
            this.restitution = 1.5f;
            this.speed = 2;
            this.sprite = new DGSpriteStatic("entities/enemy/idle");
        }

        public override void LoadContent(ContentManager content, World physics)
        {
            base.LoadContent(content, physics);

            this.physics.Position = ConvertUnits.ToSimUnits(new Vector2(randomizer.Next(40, 760), randomizer.Next(40, 500)));
            this.physics.ApplyLinearImpulse(new Vector2(this.speed * (float)Math.Cos(randomizer.Next()), this.speed * (float)Math.Sin(randomizer.Next())));

            this.physics.CollisionCategories = Category.Cat3;
            this.physics.CollidesWith = Category.Cat3;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            float fx = MathHelper.Clamp(this.physics.LinearVelocity.X, -this.speed, this.speed);
            float fy = MathHelper.Clamp(this.physics.LinearVelocity.Y, -this.speed, this.speed);

            this.fVector.X = fx;
            this.fVector.Y = fy;

            this.physics.LinearVelocity = this.fVector;
        }
    }
}
