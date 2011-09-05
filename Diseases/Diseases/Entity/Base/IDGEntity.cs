using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Screen;

namespace Diseases.Entity
{
    interface IDGEntity
    {
        float       Rotation
        {
            get;
        }
        
        Vector2     Scale
        {
            get;
        }
        Vector2     Location
        {
            get;
        }

        DGScreen    Parent
        {
            get;
        }

        void        LoadContent    ();
        void        UnloadContent  ();

        void        Update         (GameTime gametime);
        void        Render         (SpriteBatch batch);
    }
}
