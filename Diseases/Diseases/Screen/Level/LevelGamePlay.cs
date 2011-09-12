using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Physics;
using Diseases.Graphics;
using Diseases.Screen.Menu;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Diseases.Screen.Level
{
    public class LevelGamePlay : DGScreen
    {
        Body                        borderPhysics;

        DGPlayer                    player;

        Matrix                      physicsProj;
        Matrix                      physicsView;
        DebugViewXNA                physicsDebug;

        List<DGEnemy>               enemies;
        List<DGTarget>              targets;
        List<DGNutrient>            powerup;

        List<IDGSprite>             background;

        World                       physics;

        DGInputAction               pause;
        MenuPaus                    menupause;

        public                      LevelGamePlay   ()
        {

        }

        protected   override void   Initialize      ()
        {
            this.enemies = new List<DGEnemy>();
            this.targets = new List<DGTarget>();
            this.powerup = new List<DGNutrient>();

            this.background = new List<IDGSprite>();
            this.background.Add(new DGSpriteStatic("backgrounds/null"));

            this.pause = new DGInputAction(Keys.Escape, true);
        }

        public      override void   LoadContent     ()
        {
            Viewport vport = this.ScreenManager.Game.GraphicsDevice.Viewport;

            Settings.EnableDiagnostics = true;

            this.physics = new World(new Vector2(0, 0));

            this.physicsDebug = new DebugViewXNA(this.physics);
            this.physicsDebug.Flags =   FarseerPhysics.DebugViewFlags.DebugPanel | FarseerPhysics.DebugViewFlags.PerformanceGraph |
                                        DebugViewFlags.Shape | DebugViewFlags.PolygonPoints | DebugViewFlags.ContactPoints;
            this.physicsDebug.LoadContent(this.ScreenManager.Game.GraphicsDevice, this.ScreenManager.Content);

            this.menupause = new MenuPaus();

            foreach (IDGSprite sprite in this.background)
                sprite.LoadContent(this.ScreenManager.Content);

            this.physicsProj = Matrix.CreateOrthographicOffCenter(0, ConvertUnits.ToSimUnits(vport.Width), ConvertUnits.ToSimUnits(vport.Height), 0, 0, 1);
            this.physicsView = Matrix.Identity;

            float w = ConvertUnits.ToSimUnits(vport.Width - 1);
            float h = ConvertUnits.ToSimUnits(vport.Height - 1);

            Vertices verts = new Vertices(4);
            verts.Add(new Vector2(0, 0));
            verts.Add(new Vector2(0, h));
            verts.Add(new Vector2(w, h));
            verts.Add(new Vector2(w, 0));
            
            this.borderPhysics = BodyFactory.CreateLoopShape(this.physics, verts);
            this.borderPhysics.CollisionCategories = Category.All;
            this.borderPhysics.CollidesWith = Category.All;

            base.LoadContent();

            this.player = new DGPlayer();
            this.player.LoadContent(this.ScreenManager.Content, this.physics);

            Random rand = new Random(11043849);

            for (int i = 0; i < 15; i++)
            {
                DGTarget target = new DGTarget(rand);
                target.LoadContent(this.ScreenManager.Content, this.physics);

                this.targets.Add(target);
            }

            for (int i = 0; i < 5; i++)
            {
                DGEnemy enemy = new DGEnemy(rand);
                enemy.LoadContent(this.ScreenManager.Content, this.physics);

                this.enemies.Add(enemy);
            }
        }
        public override void UnloadContent()
        {
            foreach (IDGSprite sprite in this.background)
                sprite.UnloadContent();

            foreach (DGTarget target in this.targets)
                target.UnloadContent();

            foreach (DGEnemy enemy in this.enemies)
                enemy.UnloadContent();

            this.player.UnloadContent();
        }

        public      override void   HandleInput     (GameTime gametime, DGInput input)
        {
            if (this.pause.Evaluate(input))
                this.ScreenManager.AddScreen(this.menupause);

            this.player.HandleInput(gametime, input);
        }

        public      override void   Update          (GameTime gametime)
        {
            this.physics.Step(Math.Min((float)gametime.ElapsedGameTime.TotalMilliseconds * 0.001f, 1 / 30f));

            foreach (IDGSprite sprite in this.background)
                sprite.Update(gametime);

            foreach (DGTarget target in this.targets)
                target.Update(gametime);

            foreach (DGEnemy enemy in this.enemies)
                enemy.Update(gametime);

            this.player.Update(gametime);
        }
        public      override void   Render          (SpriteBatch batch)
        {
            foreach (IDGSprite sprite in this.background)
                sprite.Render(batch);

            foreach (DGTarget target in this.targets)
                target.Render(batch);

            foreach (DGEnemy enemy in this.enemies)
                enemy.Render(batch);

            this.player.Render(batch);

            batch.End();

            //this.physicsDebug.RenderDebugData(ref this.physicsProj, ref this.physicsView);

            batch.Begin();
        }
    }
}
