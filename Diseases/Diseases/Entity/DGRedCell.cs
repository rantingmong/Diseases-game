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
    public class DGRedCell : DGEntity
    {
        float           cellLife        = 0;

        bool            cellInfected    = false;
        public  bool    CellInfected
        {
            get { return this.cellInfected; }
        }

        Random          randomizer;
        Vector2         forceVector;

        DGSpriteAnimat  infectedSprite;

        public                      DGRedCell       (Random randomizer)
        {
            this.randomizer = randomizer;
        }
        protected   override void   Initialize      ()
        {
            this.restitution = 2;
            this.maxLife = 3;
            this.speed = 2;
        }

        public      override void   LoadContent     (ContentManager content, World physics)
        {
            int rotationSpeed = randomizer.Next(10, 30);

            this.sprite         = new DGSpriteAnimat("entities/target/idle", rotationSpeed, 12);
            this.infectedSprite = new DGSpriteAnimat("entities/target/inft", rotationSpeed, 12);

            this.infectedSprite.LoadContent(content);
            base.LoadContent(content, physics);

            this.physics.CollisionCategories = Category.Cat2;
            this.physics.CollidesWith = Category.Cat2;

            this.physics.Position = ConvertUnits.ToSimUnits(new Vector2(randomizer.Next(40, 760), randomizer.Next(40, 500)));
            this.physics.ApplyLinearImpulse(new Vector2((float)Math.Cos(randomizer.Next()), (float)Math.Sin(randomizer.Next())) * this.speed);
        }
        public      override void   UnloadContent   ()
        {
            base.UnloadContent();

            this.infectedSprite.UnloadContent();
        }

        public      override void   Update          (GameTime gametime)
        {
            base.Update(gametime);

            if (this.cellInfected && this.wastedLife >= 1)
                this.cellLife += (float)gametime.ElapsedGameTime.TotalSeconds;

            this.ConstrainPhysics();

            if (this.wastedLife == 1 || this.cellLife >= 10)
            {
                this.sprite.Tint = Color.Yellow;

                this.restitution = 4;
                this.speed = 1;
            }

            if (this.wastedLife == 2 || this.cellLife >= 15)
                this.sprite.Tint = Color.LightGreen;

            if (this.cellLife >= 20)
                this.dead = true;
        }

        public      void            Infect          ()
        {
            this.cellInfected = true;

            this.infectedSprite.Location    = this.sprite.Location;
            this.infectedSprite.Offset      = this.sprite.Offset;

            this.sprite = this.infectedSprite;

            this.speed = 3;
        }
        public      void            Damage          ()
        {
            if (!this.cooldownActive)
            {
                this.wastedLife++;
                this.cooldownActive = true;
            }
        }

        void ConstrainPhysics()
        {
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
    }
}
