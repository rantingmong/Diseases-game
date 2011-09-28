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
        float               pulsate;
        float               selectionFade;

        float               elapsedtime     = 0;

        Vector2             scale;
        Vector2             offset          = Vector2.Zero;

        Vector2             location;
        public Vector2      Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
                this.normalSprite.Location = value;
                this.selectedSprite.Location = value;

                this.bounds.X = (int)value.X;
                this.bounds.Y = (int)value.Y;
            }
        }

        IDGSprite           normalSprite;
        public IDGSprite    NormalSprite
        {
            get { return this.normalSprite; }
            set { this.normalSprite = value; }
        }

        IDGSprite           selectedSprite;
        public IDGSprite    SelectedSprite
        {
            get { return this.selectedSprite; }
            set { this.selectedSprite = value; }
        }

        Rectangle           bounds;
        public Rectangle    Bounds
        {
            get { return this.bounds; }
        }

        public  event EventHandler<EventArgs>   Selected;

        public                                  DGMenuEntry     (IDGSprite normSprite, IDGSprite seltSprite)
        {
            this.scale = new Vector2();

            this.normalSprite = normSprite;
            this.selectedSprite = seltSprite;
        }

        public  virtual void                    OnSelected      ()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }

        public  void                            LoadContent     (ContentManager content)
        {
            this.normalSprite.LoadContent(content);

            if(this.selectedSprite != null)
                this.selectedSprite.LoadContent(content);

            this.bounds = new Rectangle((int)this.location.X, (int)this.location.Y, this.normalSprite.Texture.Width, this.normalSprite.Texture.Height);
        }
        public  void                            UnloadContent   ()
        {
            this.normalSprite.UnloadContent();
        }

        public  void                            Update          (GameTime gametime, bool isselected)
        {
            this.normalSprite.Update(gametime);

            if (this.selectedSprite == null)
            {
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

                this.offset.X = this.normalSprite.Texture.Width * ofactr;
                this.offset.Y = this.normalSprite.Texture.Height * ofactr;

                this.normalSprite.Scale = this.scale;
                this.normalSprite.Offset = this.offset;
            }
            else
            {
                this.selectedSprite.Update(gametime);
            }
        }
        public  void                            Render          (SpriteBatch batch, bool isselected)
        {
            if (isselected && this.selectedSprite != null)
                this.selectedSprite.Render(batch);
            else
                this.normalSprite.Render(batch);
        }
    }
}
