using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diseases.Graphics
{
    public class DGSpriteStatic : IDGSprite
    {
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

        public              DGSpriteStatic  (string contentlocation)
        {
            this.contentloc = contentlocation;
        }

        public void         LoadContent     (ContentManager content)
        {
            this.texture = content.Load<Texture2D>(this.contentloc);

            Debug.WriteLine(string.Format("asset created! ({0})", this.contentloc), "INFO");
        }
        public void         UnloadContent   ()
        {
            this.texture.Dispose();
        }

        public void         Update          (GameTime gametime)
        {
            return;
        }
        public void         Render          (SpriteBatch batch)
        {
            batch.Draw(this.texture, this.location, null, this.tint, this.rotation, Vector2.Zero, this.scale, SpriteEffects.None, 0);
        }
    }
}
