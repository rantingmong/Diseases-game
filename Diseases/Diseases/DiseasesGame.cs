using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diseases
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (DiseasesGame game = new DiseasesGame())
            {
                game.Run();
            }
        }
    }

    public class DiseasesGame : Game
    {
        private GraphicsDeviceManager graphicsManager;

        public                  DiseasesGame    ()
        {
            this.graphicsManager = new GraphicsDeviceManager(this);

        }

        protected override void Dispose         (bool disposing)
        {
            base.Dispose(disposing);
        }
        protected override void Initialize      ()
        {
            base.Initialize();
        }

        protected override void LoadContent     ()
        {
            base.LoadContent();
        }
        protected override void UnloadContent   ()
        {
            base.UnloadContent();
        }

        protected override void Update          (GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void Draw            (GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
