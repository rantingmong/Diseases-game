using System;
using System.Diagnostics;
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
                Debug.WriteLine("game initialized", "INFO");

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
            this.IsMouseVisible = true;

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
            this.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
