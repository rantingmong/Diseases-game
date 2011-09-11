using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Diseases.Entity
{
    public class DGPlayer : DGEntity
    {
        float           elapsed     = 0;

        bool            usemouse    = false;
        Point           mpoint      = Point.Zero;

        Vector2         location    = Vector2.Zero;
        public Vector2  Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        DGInputAction   keyup       = new DGInputAction(Keys.Up,    false);
        DGInputAction   keydown     = new DGInputAction(Keys.Down,  false);
        DGInputAction   keyleft     = new DGInputAction(Keys.Left,  false);
        DGInputAction   keyrigt     = new DGInputAction(Keys.Right, false);

        public DGPlayer()
        {
            this.sprite = new DGSpriteStatic("entities/null");

            this.speed = 0;
            this.restitution = 0;

            this.life = 5;
        }

        public override void HandleInput(GameTime gametime, DGInput input)
        {
            if (input.CurMouseState.X != this.mpoint.X && input.CurMouseState.Y != this.mpoint.Y)
                this.usemouse = true;

            this.elapsed += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (this.elapsed > 5)
            {
                if (keyup.Evaluate(input))
                {
                    location.Y -= 4;
                    this.usemouse = false;
                }

                if (keydown.Evaluate(input))
                {
                    location.Y += 4;
                    this.usemouse = false;
                }

                if (keyleft.Evaluate(input))
                {
                    location.X -= 4;
                    this.usemouse = false;
                }

                if (keyrigt.Evaluate(input))
                {
                    location.X += 4;
                    this.usemouse = false;
                }

                this.elapsed = 0;
            }

            if (this.usemouse)
            {
                location.X = input.CurMouseState.X - (this.sprite.Texture.Width / 2);
                location.Y = input.CurMouseState.Y - (this.sprite.Texture.Height / 2);
            }

            location.X = (int)MathHelper.Clamp(location.X, 0, 800 - this.sprite.Texture.Width);
            location.Y = (int)MathHelper.Clamp(location.Y, 0, 540 - this.sprite.Texture.Height);

            this.mpoint.X = input.CurMouseState.X;
            this.mpoint.Y = input.CurMouseState.Y;
        }

        public override void Render(SpriteBatch batch)
        {
            this.sprite.Location = this.location;
            base.Render(batch);
        }
    }
}
