using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diseases.Graphics
{
    public class DGSpriteAnimate : IDGSprite
    {
        bool    pause       = false;

        int     width       = 0;
        int     curframe    = 0;

        int     totalfram   = 0;
        int     frampersc   = 0;

        float   timeperfram = 0;
        float   elapsedtime = 0;

        Rectangle clipLoc;

        string contentloc;
        public string       ContentLocation
        {
            get { return this.contentloc; }
        }

        Texture2D texture;
        public Texture2D    Texture
        {
            get { return this.texture; }
        }

        Color tint = Color.White;
        public Color        Tint
        {
            get { return this.tint; }
            set { this.tint = value; }
        }

        float rotation = 0;
        public float        Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        Vector2 scale = new Vector2(1, 1);
        public Vector2      Scale
        {
            get { return this.scale; }
            set { this.scale = value; }
        }

        Vector2 location = Vector2.Zero;
        public Vector2      Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public              DGSpriteAnimate (string contentlocation, int fps, int totalframes)
        {
            this.contentloc = contentlocation;

            this.frampersc = fps;
            this.totalfram = totalframes;
        }

        public void         LoadContent     (ContentManager content)
        {
            this.texture = content.Load<Texture2D>(this.contentloc);

            this.width = this.texture.Width / this.totalfram;
            this.timeperfram = 1000 / this.frampersc;

            this.clipLoc = new Rectangle()
            {
                Width   = this.width,
                Height  = this.texture.Height
            };
            
            Debug.WriteLine(string.Format("asset created! ({0})", this.contentloc), "INFO");
        }
        public void         UnloadContent   ()
        {
            this.texture.Dispose();
        }

        public void         Update          (GameTime gametime)
        {
            this.elapsedtime += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (this.elapsedtime > this.timeperfram && !this.pause)
            {
                this.elapsedtime = 0;

                this.curframe = (this.curframe++ % this.totalfram);
            }
        }
        public void         Render          (SpriteBatch batch)
        {
            this.clipLoc.X = this.curframe * this.width;

            batch.Draw(this.texture, this.location, this.clipLoc, this.tint, this.rotation, Vector2.Zero, this.scale, SpriteEffects.None, 0);
        }
    }
}
