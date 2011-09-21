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
    public class DGPlayer : DGEntity
    {
        float           forceImpulse    = 0.8f;

        bool            ypressed        = false;
        bool            xpressed        = false;

        Vector2         fvector         = Vector2.Zero;

        SoundEffect     ouch;
        SoundEffect     hlth;

        DGInputAction   keyup           = new DGInputAction(Keys.Up,    false);
        DGInputAction   keydown         = new DGInputAction(Keys.Down,  false);
        DGInputAction   keyleft         = new DGInputAction(Keys.Left,  false);
        DGInputAction   keyrigt         = new DGInputAction(Keys.Right, false);

        protected   override void   Initialize  ()
        {
            this.restitution = 1.5f;
            this.maxLife = 10;
            this.speed = 3;

            this.sprite = new DGSpriteAnimat("entities/bacteria/idle", 8, 10);
        }

        public      override void   LoadContent (ContentManager content, World physics)
        {
            base.LoadContent(content, physics);

            this.physics.Position = ConvertUnits.ToSimUnits(50, 50);
            this.physics.CollisionCategories = Category.Cat1;
            this.physics.CollidesWith = Category.Cat1;

            this.ouch = content.Load<SoundEffect>("sounds/ouch");
            this.hlth = content.Load<SoundEffect>("sounds/health");
        }

        public      override void   HandleInput (GameTime gametime, DGInput input)
        {
            if (keyup.Evaluate(input))
            {
                this.physics.ApplyLinearImpulse(new Vector2(0, -this.forceImpulse));

                this.ypressed = true;
            }
            else this.ypressed = false;

            if (keydown.Evaluate(input))
            {
                this.physics.ApplyLinearImpulse(new Vector2(0, this.forceImpulse));

                this.ypressed = true;
            }
            else this.ypressed = false;

            if (keyleft.Evaluate(input))
            {
                this.physics.ApplyLinearImpulse(new Vector2(-this.forceImpulse, 0));

                this.xpressed = true;
            }
            else this.xpressed = false;

            if (keyrigt.Evaluate(input))
            {
                this.physics.ApplyLinearImpulse(new Vector2(this.forceImpulse, 0));

                this.xpressed = true;
            }
            else this.xpressed = false;
        }
        public      override void   Update      (GameTime gametime)
        {
            base.Update(gametime);
            
            if (this.wastedLife == 6)
            {
                this.sprite.Tint = Color.White;
                this.speed = 3;
            }

            if (this.wastedLife == 7)
            {
                this.sprite.Tint = Color.LightGreen;
                this.speed = 2;
            }

            this.wastedLife = (int)MathHelper.Clamp(this.wastedLife, 0, 10);

            this.ConstrainPhysics();
        }

        public      void            Damage      ()
        {
            if (!this.cooldownActive)
            {
                this.cooldownElapsed = 0;
                this.cooldownActive = true;

                this.wastedLife++;
            }
        }
        public      void            Healed      ()
        {
            this.cooldownActive = false;
            this.wastedLife--;
        }

        void ConstrainPhysics()
        {
            float fx = this.physics.LinearVelocity.X;
            float fy = this.physics.LinearVelocity.Y;

            if (this.xpressed)
                fx = MathHelper.Clamp(fx, -this.speed, this.speed);
            else
            {
                if (this.physics.LinearVelocity.X > 0)
                    fx = MathHelper.Clamp(fx - 0.1f, 0, this.speed);
                else
                    fx = MathHelper.Clamp(fx + 0.1f, -this.speed, 0);
            }

            if (this.ypressed)
                fy = MathHelper.Clamp(fy, -this.speed, this.speed);
            else
            {
                if (this.physics.LinearVelocity.Y > 0)
                    fy = MathHelper.Clamp(fy - 0.1f, 0, this.speed);
                else
                    fy = MathHelper.Clamp(fy + 0.1f, -this.speed, 0);
            }

            fvector.X = fx;
            fvector.Y = fy;

            this.physics.LinearVelocity = fvector;
        }
    }
}
