using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

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
        int                         score               = 0;
        float                       gameperiod          = 0;

        float                       targetElapsedTime   = 0;
        float                       enemyElapsedTime    = 0;
        float                       pwerpElapsedTime    = 0;

        Random                      rand;

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

        bool                        showdebugphysics;
        DGInputAction               debugphysics;

        DGInputAction               pause;
        MenuPaus                    menupause;

        SpriteFont                  font;

        protected   override void   Initialize      ()
        {
            this.enemies = new List<DGEnemy>();
            this.targets = new List<DGTarget>();
            this.powerup = new List<DGNutrient>();

            this.background = new List<IDGSprite>();
            this.background.Add(new DGSpriteStatic("backgrounds/null"));

            this.pause          = new DGInputAction(Keys.Escape, true);
            this.debugphysics   = new DGInputAction(Keys.F1, true);
        }

        public      override void   LoadContent     ()
        {
            Viewport vport = this.ScreenManager.Game.GraphicsDevice.Viewport;

            this.physics = new World(new Vector2(0, 0));

            this.menupause = new MenuPaus();

            this.font = this.ScreenManager.Content.Load<SpriteFont>("fonts/gamefont");

            this.physicsDebug = new DebugViewXNA(this.physics);
            this.physicsDebug.Flags =   FarseerPhysics.DebugViewFlags.DebugPanel | FarseerPhysics.DebugViewFlags.PerformanceGraph |
                                        DebugViewFlags.Shape | DebugViewFlags.PolygonPoints | DebugViewFlags.ContactPoints;
            this.physicsDebug.LoadContent(this.ScreenManager.Game.GraphicsDevice, this.ScreenManager.Content);

            foreach (IDGSprite sprite in this.background)
                sprite.LoadContent(this.ScreenManager.Content);

            lock (this.physics)
            {
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

                this.player.Location = new Vector2(vport.Width / 2, vport.Height / 2);

                this.rand = new Random(11043849 / DateTime.Now.Millisecond);
            }
        }
        public      override void   UnloadContent   ()
        {
            foreach (IDGSprite sprite in this.background)
                sprite.UnloadContent();

            foreach (DGTarget target in this.targets)
                target.UnloadContent();

            foreach (DGEnemy enemy in this.enemies)
                enemy.UnloadContent();

            foreach (DGNutrient nutrient in this.powerup)
                nutrient.UnloadContent();

            this.player.UnloadContent();
        }

        public      override void   HandleInput     (GameTime gametime, DGInput input)
        {
            if (this.pause.Evaluate(input))
            {
                this.ScreenManager.AddScreen(this.menupause);

                this.showdebugphysics = false;
            }

            if (this.debugphysics.Evaluate(input))
                this.showdebugphysics = this.showdebugphysics ? false : true;

            this.player.HandleInput(gametime, input);
        }

        public      override void   Update          (GameTime gametime)
        {
            foreach (IDGSprite sprite in this.background)
                sprite.Update(gametime);

            lock (this.physics)
            {
                if (this.player.dead)
                {
                    this.ScreenManager.RemoveScreen(this);
                    this.ScreenManager.AddScreen(new MenuOver((this.score / 2) * (int)this.gameperiod));
                }

                this.physics.Step(Math.Min((float)gametime.ElapsedGameTime.TotalMilliseconds * 0.001f, 1 / 30f));

                this.player.Update(gametime);

                this.targetElapsedTime += (float)gametime.ElapsedGameTime.TotalSeconds;
                this.enemyElapsedTime += (float)gametime.ElapsedGameTime.TotalSeconds;
                this.pwerpElapsedTime += (float)gametime.ElapsedGameTime.TotalSeconds;

                this.gameperiod += (float)gametime.ElapsedGameTime.TotalSeconds;

                for (int i = 0; i < this.targets.Count; i++)
                    if (this.targets[i].isdead)
                    {
                        this.physics.RemoveBody(this.targets[i].physics);
                        this.targets.RemoveAt(i);

                        if (this.targets.Count < 30)
                        {
                            DGTarget newTarget = new DGTarget(this.rand);
                            newTarget.LoadContent(this.ScreenManager.Content, this.physics);

                            this.targets.Add(newTarget);
                        }
                    }

                for (int i = 0; i < this.enemies.Count; i++)
                    if (this.enemies[i].dead)
                    {
                        this.physics.RemoveBody(this.enemies[i].physics);
                        this.enemies.RemoveAt(i);

                        if (this.enemies.Count < 10)
                        {
                            DGEnemy newEnemy = new DGEnemy(this.rand);
                            newEnemy.LoadContent(this.ScreenManager.Content, this.physics);

                            this.enemies.Add(newEnemy);
                        }
                    }

                for (int i = 0; i < this.powerup.Count; i++)
                    if (this.powerup[i].dead)
                    {
                        this.physics.RemoveBody(this.powerup[i].physics);
                        this.powerup.RemoveAt(i);
                    }

                if (this.targetElapsedTime >= (60 / 20) && this.targets.Count <= 20)
                {
                    DGTarget newTarget = new DGTarget(this.rand);
                    newTarget.LoadContent(this.ScreenManager.Content, this.physics);

                    this.targets.Add(newTarget);

                    this.targetElapsedTime = 0;
                }

                if (this.enemyElapsedTime >= (60 / 10) && this.enemies.Count <= 5)
                {
                    DGEnemy newEnemy = new DGEnemy(this.rand);
                    newEnemy.LoadContent(this.ScreenManager.Content, this.physics);

                    this.enemies.Add(newEnemy);

                    this.enemyElapsedTime = 0;
                }

                if (this.pwerpElapsedTime >= 10 && this.powerup.Count <= 2)
                {
                    DGNutrient newNutrient = new DGNutrient(this.rand);
                    newNutrient.LoadContent(this.ScreenManager.Content, this.physics);

                    this.powerup.Add(newNutrient);

                    this.pwerpElapsedTime = 0;
                }

                foreach (DGTarget target in this.targets)
                {
                    target.Update(gametime);

                    if (target.Bounds.Intersects(this.player.Bounds) && !target.isinfected)
                    {
                        target.Infected();

                        this.score += 50;
                    }
                }

                foreach (DGEnemy enemy in this.enemies)
                {
                    enemy.Update(gametime);

                    if (enemy.Bounds.Intersects(this.player.Bounds))
                    {
                        this.player.Damage();
                        enemy.Damage();
                    }

                    foreach (DGTarget target in this.targets)
                    {
                        if (enemy.Bounds.Intersects(target.Bounds) && target.isinfected)
                        {
                            target.Damage();
                            enemy.Damage();
                        }
                    }
                }

                foreach (DGNutrient nutrient in this.powerup)
                {
                    nutrient.Update(gametime);

                    if (nutrient.Bounds.Intersects(this.player.Bounds))
                    {
                        this.player.Repair();
                        nutrient.Kill();
                    }
                }
            }
        }
        public      override void   Render          (SpriteBatch batch)
        {
            foreach (IDGSprite sprite in this.background)
                sprite.Render(batch);

            foreach (DGTarget target in this.targets)
                target.Render(batch);

            foreach (DGEnemy enemy in this.enemies)
                enemy.Render(batch);

            foreach (DGNutrient nutrient in this.powerup)
                nutrient.Render(batch);

            this.player.Render(batch);

            batch.End();

            lock (this.physics)
            {
                if (this.showdebugphysics)
                    this.physicsDebug.RenderDebugData(ref this.physicsProj, ref this.physicsView);
            }

            batch.Begin();

            batch.DrawString(this.font, string.Format("Life: {0}/10", this.player.Life), new Vector2(40, 40), this.player.Life > 2 ? Color.Black : Color.Red);
            batch.DrawString(this.font, string.Format("Score: {0}", (this.score / 2) * (int)this.gameperiod), new Vector2(40, 80), Color.Black);
        }
    }
}
