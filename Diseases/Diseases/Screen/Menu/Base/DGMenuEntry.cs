using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Graphics;

namespace Diseases.Entity
{
    public class DGMenuEntry
    {
        float pulsate;
        float selectionFade;

        float elapsedtime = 0;

        Vector2 scale;
        Vector2 offset = Vector2.Zero;

        Vector2 location;
        public Vector2 Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
                this.sprite.Location = value;

                this.bounds.X = (int)value.X;
                this.bounds.Y = (int)value.Y;
            }
        }

        IDGSprite sprite;
        public IDGSprite Sprite
        {
            get { return this.sprite; }
            set { this.sprite = value; }
        }

        Rectangle bounds;
        public Rectangle Bounds
        {
            get { return this.bounds; }
        }

        public event EventHandler<EventArgs> Selected;

        protected internal virtual void OnSelected()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }

        public DGMenuEntry(string text, string contentlocation, bool animating)
        {
            this.scale = new Vector2();

            if (animating)
                this.sprite = new DGSpriteAnimat(contentlocation, 10, 5);
            else
                this.sprite = new DGSpriteStatic(contentlocation);
        }

        public void LoadContent(ContentManager content)
        {
            this.sprite.LoadContent(content);

            this.bounds = new Rectangle((int)this.location.X, (int)this.location.Y, this.sprite.Texture.Width, this.sprite.Texture.Height);
        }
        public void UnloadContent()
        {
            this.sprite.UnloadContent();
        }

        public void Update(GameTime gametime, bool isselected)
        {
            this.sprite.Update(gametime);

            this.elapsedtime += (float)gametime.ElapsedGameTime.TotalSeconds;

            this.pulsate = (float)Math.Sin(this.elapsedtime * 10) + 1;

            float fadespeed = (float)gametime.ElapsedGameTime.TotalSeconds * 4;

            if (isselected)
                selectionFade = Math.Min(selectionFade + fadespeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadespeed, 0);

            if (selectionFade == 0 && !isselected)
                this.elapsedtime = 0;

            float pfactr = this.pulsate * 0.03f * selectionFade;

            float scalef = pfactr + 1;
            float ofactr = pfactr / 2;

            this.scale.X = scalef;
            this.scale.Y = scalef;

            this.offset.X = this.sprite.Texture.Width * ofactr;
            this.offset.Y = this.sprite.Texture.Height * ofactr;
        }
        public void Render(SpriteBatch batch, bool isselected)
        {
            this.sprite.Scale = this.scale;
            this.sprite.Offset = this.offset;
            this.sprite.Render(batch);
        }
    }
}
