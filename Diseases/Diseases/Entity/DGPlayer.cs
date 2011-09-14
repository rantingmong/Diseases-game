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
using Microsoft.Xna.Framework.Audio;

namespace Diseases.Entity
{
    public class DGPlayer : DGEntity
    {
        float           pulsate;
        float           selectionFade;
        float           elapsedtime     = 0;
        Vector2         scale           = new Vector2(1, 1);

        public bool     dead            = false;
        bool            damaged         = false;
        float           cooldowndamage  = 0;
        public int      damagecounter   = 0;

        public int      Life
        {
            get { return 10 - this.damagecounter; }
        }

        bool            ypressed        = false;
        bool            xpressed        = false;
        float           kpelapsed       = 0;

        Vector2         fvector         = Vector2.Zero;

        SoundEffect     ouch;
        SoundEffect     health;

        DGInputAction   keyup           = new DGInputAction(Keys.Up,    false);
        DGInputAction   keydown         = new DGInputAction(Keys.Down,  false);
        DGInputAction   keyleft         = new DGInputAction(Keys.Left,  false);
        DGInputAction   keyrigt         = new DGInputAction(Keys.Right, false);

        protected   override void   Initialize  ()
        {
            this.restitution = 1.5f;
            this.speed = 3;

            this.sprite = new DGSpriteAnimat("entities/bacteria/idle", 8, 10);
        }

        public      override void   LoadContent (ContentManager content, World physics)
        {
            base.LoadContent(content, physics);

            this.physics.Position = ConvertUnits.ToSimUnits(50, 50);
            this.physics.CollisionCategories = Category.Cat1;
            this.physics.CollidesWith = Category.Cat1;

            this.bounds.X = (int)ConvertUnits.ToDisplayUnits(this.physics.Position.X);
            this.bounds.Y = (int)ConvertUnits.ToDisplayUnits(this.physics.Position.Y);

            this.ouch = content.Load<SoundEffect>("sounds/ouch");
            this.health = content.Load<SoundEffect>("sounds/health");
        }

        public      override void   HandleInput (GameTime gametime, DGInput input)
        {
            kpelapsed += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (kpelapsed > 50)
            {
                if (keyup.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(0, -0.8f));

                    this.ypressed = true;
                }
                else
                {
                    this.ypressed = false;
                }

                if (keydown.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(0, 0.8f));

                    this.ypressed = true;
                }
                else
                {
                    this.ypressed = false;
                }

                if (keyleft.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(-0.8f, 0));
                    this.physics.ApplyTorque(-1000);

                    this.xpressed = true;
                }
                else
                {
                    this.xpressed = false;
                }

                if (keyrigt.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(0.8f, 0));
                    this.physics.ApplyTorque(1000);

                    this.xpressed = true;
                }
                else
                {
                    this.xpressed = false;
                }

                kpelapsed = 0;
            }

            float fx = 0;
            float fy = 0;

            if (this.xpressed)
            {
                fx = MathHelper.Clamp(this.physics.LinearVelocity.X, -this.speed, this.speed);
            }
            else
            {
                if (this.physics.LinearVelocity.X > 0)
                    fx = MathHelper.Clamp(this.physics.LinearVelocity.X - 0.1f, 0, this.speed);
                else
                    fx = MathHelper.Clamp(this.physics.LinearVelocity.X + 0.1f, -this.speed, 0);
            }

            if (this.ypressed)
            {
                fy = MathHelper.Clamp(this.physics.LinearVelocity.Y, -this.speed, this.speed);
            }
            else
            {
                if (this.physics.LinearVelocity.Y > 0)
                    fy = MathHelper.Clamp(this.physics.LinearVelocity.Y - 0.1f, 0, this.speed);
                else
                    fy = MathHelper.Clamp(this.physics.LinearVelocity.Y + 0.1f, -this.speed, 0);
            }

            fvector.X = fx;
            fvector.Y = fy;

            this.physics.LinearVelocity = fvector;
        }
        public      override void   Update      (GameTime gametime)
        {
            base.Update(gametime);

            this.elapsedtime += (float)gametime.ElapsedGameTime.TotalSeconds;

            this.pulsate = (float)Math.Sin(this.elapsedtime * 5) + 1;

            float fadespeed = (float)gametime.ElapsedGameTime.TotalSeconds * 4;
            selectionFade = Math.Min(selectionFade + fadespeed, 1);
           
            float pfactr = this.pulsate * 0.25f * selectionFade;

            float scalef = pfactr + 1;
            float ofactr = pfactr / 2;

            this.scale.X = scalef;
            this.scale.Y = scalef;

            this.sprite.Scale = this.scale;

            this.cooldowndamage += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (this.cooldowndamage > 2)
            {
                this.damaged = false;

                this.cooldowndamage = 0;
            }

            if (this.damagecounter == 7)
            {
                this.sprite.Tint = Color.White;
                this.speed = 3;
            }

            if (this.damagecounter == 8)
            {
                this.sprite.Tint = Color.Purple;
                this.speed = 2;
            }

            if (this.damagecounter == 10)
            {
                this.dead = true;
            }

            this.damagecounter = (int)MathHelper.Clamp(this.damagecounter, 0, 10);
        }

        public      void            Damage      ()
        {
            if (!this.damaged)
            {
                this.damagecounter++;
                this.damaged = true;

                this.ouch.Play();
            }
        }
        public      void            Repair      ()
        {
            this.damagecounter--;
            this.damaged = false;

            this.health.Play();
        }
    }
}
