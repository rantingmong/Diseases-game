using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Physics;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;

namespace Diseases.Entity
{
    public class DGTarget : DGEntity
    {
        bool damaged = false;
        float cooldowndamage = 0;
        public int damagecounter = 0;

        public bool isinfected = false;
        public bool isdead = false;

        Random randomizer;

        Vector2 fVector = Vector2.Zero;

        SoundEffect bumpSound;

        DGSpriteAnimat infected = new DGSpriteAnimat("entities/target/inft", 5, 12);

        public DGTarget(Random randomizer)
        {
            this.randomizer = randomizer;
        }
        protected override void Initialize()
        {
            this.restitution = 1f;
            this.speed = 1.5f;
        }

        public override void LoadContent(ContentManager content, World physics)
        {
            this.sprite = new DGSpriteAnimat("entities/target/idle", randomizer.Next(20, 60), 12);

            base.LoadContent(content, physics);

            this.infected.LoadContent(content);
            this.bumpSound = content.Load<SoundEffect>("sounds/bump");

            this.physics.Position = ConvertUnits.ToSimUnits(new Vector2(randomizer.Next(40, 760), randomizer.Next(40, 500)));
            this.physics.ApplyLinearImpulse(new Vector2(this.speed * (float)Math.Cos(randomizer.Next()), this.speed * (float)Math.Sin(randomizer.Next())));

            this.physics.CollisionCategories = Category.Cat2;
            this.physics.CollidesWith = Category.Cat2;

            this.bounds.X = (int)ConvertUnits.ToDisplayUnits(this.physics.Position.X);
            this.bounds.Y = (int)ConvertUnits.ToDisplayUnits(this.physics.Position.Y);
        }
        public override void UnloadContent()
        {
            this.infected.UnloadContent();
            base.UnloadContent();

            this.bumpSound.Dispose();
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            this.cooldowndamage += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (this.cooldowndamage > 2)
            {
                this.damaged = false;

                this.cooldowndamage = 0;
            }

            if (this.damagecounter == 1)
            {
                this.sprite.Tint = Color.Yellow;

                this.restitution = 4;
                this.speed = 1;
            }

            if (this.damagecounter == 3)
            {
                this.sprite.Tint = Color.Green;

                this.speed = 0.5f;
            }

            if (this.damagecounter == 5)
                this.isdead = true;

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

            this.fVector.X = fx;
            this.fVector.Y = fy;

            this.physics.LinearVelocity = this.fVector;           
        }
        
        public void Infected()
        {
            this.isinfected = true;

            this.bumpSound.Play();

            this.infected.Location = this.sprite.Location;
            this.infected.Offset = this.sprite.Offset;

            this.sprite = this.infected;

            this.speed = 3f;
        }
        public void Disinfect()
        {
            this.isinfected = false;
        }

        public void Damage()
        {
            if (!this.damaged)
            {
                damagecounter++;

                this.damaged = true;
            }
        }
    }
}
