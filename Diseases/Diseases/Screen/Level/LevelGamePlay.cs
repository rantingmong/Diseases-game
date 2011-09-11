using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;

namespace Diseases.Screen.Level
{
    public class LevelGamePlay : DGScreen
    {
        DGPlayer                    player;

        List<DGEnemy>               enemies;
        List<DGTarget>              targets;
        List<DGNutrient>            powerup;

        List<IDGSprite>             background;

        World                       physics;

        public                      LevelGamePlay   ()
        {

        }

        protected   override void   Initialize      ()
        {
            this.player = new DGPlayer();

            this.enemies = new List<DGEnemy>(5);
            this.targets = new List<DGTarget>(25);
            this.powerup = new List<DGNutrient>(2);

            this.background = new List<IDGSprite>();
            this.background.Add(new DGSpriteStatic("backgrounds/null"));
        }

        public      override void   LoadContent     ()
        {
            Thread.Sleep(1000);

            this.physics = new World(new Vector2(0, 0));

            this.player.Location = new Vector2(this.ScreenManager.Game.GraphicsDevice.Viewport.Width / 2, this.ScreenManager.Game.GraphicsDevice.Viewport.Height / 2);
            this.player.LoadContent(this.ScreenManager.Content, this.physics);

            foreach (IDGSprite sprite in this.background)
                sprite.LoadContent(this.ScreenManager.Content);

            Thread.Sleep(3000);

            base.LoadContent();
        }
        public      override void   UnloadContent   ()
        {
            foreach (IDGSprite sprite in this.background)
                sprite.UnloadContent();
        }

        public      override void   HandleInput     (GameTime gametime, DGInput input)
        {
            this.player.HandleInput(gametime, input);   
        }

        public      override void   Update          (GameTime gametime)
        {
            foreach (IDGSprite sprite in this.background)
                sprite.Update(gametime);

            this.player.Update(gametime);
        }
        public      override void   Render          (SpriteBatch batch)
        {
            foreach (IDGSprite sprite in this.background)
                sprite.Render(batch);

            this.player.Render(batch);
        }
    }
}
