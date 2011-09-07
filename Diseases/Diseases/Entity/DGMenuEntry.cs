using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Diseases.Entity
{
    public class DGMenuEntry
    {
        float pulsate;
        float selectionFade;

        float elapsedtime = 0;

        Vector2 scale;

        bool drawtext;
        public bool DrawText
        {
            get { return this.drawtext; }
            set { this.drawtext = value; }
        }

        string text;
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        Vector2 location;
        public Vector2 Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
                this.sprite.Location = value;
            }
        }

        IDGSprite sprite;
        public IDGSprite Sprite
        {
            get { return this.sprite; }
            set { this.sprite = value; }
        }

        public event EventHandler<EventArgs> Selected;

        protected internal virtual void OnSelected()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }

        public DGMenuEntry(string text, string contentlocation, bool animating)
        {
            this.text = text;
            
            this.scale = new Vector2();

            if (animating)
                this.sprite = new DGSpriteAnimat(contentlocation, 10, 5);
            else
                this.sprite = new DGSpriteStatic(contentlocation);
        }

        public void LoadContent(ContentManager content)
        {
            this.sprite.LoadContent(content);
        }
        public void UnloadContent()
        {
            this.sprite.UnloadContent();
        }

        public void Update(GameTime gametime, bool isselected)
        {
            this.sprite.Update(gametime);

            this.elapsedtime += (float)gametime.ElapsedGameTime.TotalSeconds;

            this.pulsate = (float)Math.Sin(this.elapsedtime * 6) + 1;

            float fadespeed = (float)gametime.ElapsedGameTime.TotalSeconds * 4;

            if (isselected)
                selectionFade = Math.Min(selectionFade + fadespeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadespeed, 0);

            if (selectionFade == 0 && !isselected)
                this.elapsedtime = 0;

            float scalef = 1 + this.pulsate * 0.025f * selectionFade;
            this.scale.X = scalef;
            this.scale.Y = scalef;
        }
        public void Render(SpriteBatch batch, bool isselected)
        {
            this.sprite.Scale = this.scale;
            this.sprite.Render(batch);

            if (this.drawtext)
            {
                // draw text here...
            }
        }
    }
}
