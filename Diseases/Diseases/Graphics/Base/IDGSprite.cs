using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diseases.Graphics
{
    public interface IDGSprite
    {
        Texture2D   Texture
        {
            get;
        }

        Vector2     Scale
        {
            get;
            set;
        }
        Vector2     Location
        {
            get;
            set;
        }

        float       Rotation
        {
            get;
            set;
        }

        void        LoadContent     (ContentManager content);
        void        UnloadContent   ();

        void        Update          (GameTime gametime);
        void        Render          (SpriteBatch batch);
    }
}
