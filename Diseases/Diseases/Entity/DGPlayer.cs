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
    public class DGPlayer : DGEntity
    {
        bool            ypressed    = false;
        bool            xpressed    = false;
        float           kpelapsed   = 0;

        Vector2         fvector     = Vector2.Zero;

        DGInputAction   keyup       = new DGInputAction(Keys.Up,    false);
        DGInputAction   keydown     = new DGInputAction(Keys.Down,  false);
        DGInputAction   keyleft     = new DGInputAction(Keys.Left,  false);
        DGInputAction   keyrigt     = new DGInputAction(Keys.Right, false);

        protected override void Initialize()
        {
            this.restitution = 1.5f;
            this.sprite = new DGSpriteStatic("entities/bacteria/idle");
        }

        public override void LoadContent(ContentManager content, World physics)
        {
            base.LoadContent(content, physics);

            this.physics.Position = ConvertUnits.ToSimUnits(50, 50);
            this.physics.CollisionCategories = Category.Cat1;
            this.physics.CollidesWith = Category.Cat1;
        }

        public override void HandleInput(GameTime gametime, DGInput input)
        {
            kpelapsed += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (kpelapsed > 50)
            {
                if (keyup.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(0, -0.5f));

                    this.ypressed = true;
                }
                else
                {
                    this.ypressed = false;
                }

                if (keydown.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(0, 0.5f));

                    this.ypressed = true;
                }
                else
                {
                    this.ypressed = false;
                }

                if (keyleft.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(-0.5f, 0));

                    this.xpressed = true;
                }
                else
                {
                    this.xpressed = false;
                }

                if (keyrigt.Evaluate(input))
                {
                    this.physics.ApplyLinearImpulse(new Vector2(0.5f, 0));

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
                fx = MathHelper.Clamp(this.physics.LinearVelocity.X, -2, 2);
            }
            else
            {
                if (this.physics.LinearVelocity.X > 0)
                    fx = MathHelper.Clamp(this.physics.LinearVelocity.X - 0.05f, 0, 2);
                else
                    fx = MathHelper.Clamp(this.physics.LinearVelocity.X + 0.05f, -2, 0);
            }

            if (this.ypressed)
            {
                fy = MathHelper.Clamp(this.physics.LinearVelocity.Y, -2, 2);
            }
            else
            {
                if (this.physics.LinearVelocity.Y > 0)
                    fy = MathHelper.Clamp(this.physics.LinearVelocity.Y - 0.05f, 0, 2);
                else
                    fy = MathHelper.Clamp(this.physics.LinearVelocity.Y + 0.05f, -2, 0);
            }

            fvector.X = fx;
            fvector.Y = fy;

            this.physics.LinearVelocity = fvector;
        }
    }
}
