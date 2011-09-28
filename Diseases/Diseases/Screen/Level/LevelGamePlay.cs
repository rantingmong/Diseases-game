using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        #region FIELDS

        float                       readyTime = 0;
        bool                        isPlaying;

        bool                        gameLost;
        Random                      randomizer;

        Body                        gameBorder;
        World                       gamePhysic;

        Matrix                      projMatrix;
        DebugViewXNA                viewnDebug;

        DGPlayer                    player;

        float                       totElapsed          = 0;

        List<DGRedCell>             redCells;
        List<DGWhtCell>             whtCells;
        List<DGPowCell>             powCells;

        float                       redElapsed          = 0;
        float                       whtElapsed          = 0;
        float                       powElapsed          = 0;

        SpriteFont                  debugFont;
        DGSpriteStatic              gameBackground;

        bool                        physicsDebugShown   = false;
        DGInputAction               physicsInput;

        Song                        overSong;
        MenuOver                    overMenu;

        MenuPaus                    pausMenu;
        DGInputAction               pausInput;

        #endregion

        #region OVERRIDES

        protected   override void   Initialize              ()
        {
            this.gameBackground = new DGSpriteStatic("backgrounds/menu/mainBack");

            this.redCells = new List<DGRedCell>();
            this.whtCells = new List<DGWhtCell>();
            this.powCells = new List<DGPowCell>();

            this.pausInput = new DGInputAction(Keys.Escape, true);
            this.physicsInput = new DGInputAction(Keys.F1, true);

            this.pausMenu = new MenuPaus();
            this.overMenu = new MenuOver();
        }

        public      override void   LoadContent             ()
        {
            this.debugFont = this.ScreenManager.Content.Load<SpriteFont>("fonts/debugfont");
            this.gameBackground.LoadContent(this.ScreenManager.Content);

            this.overSong = this.ScreenManager.Content.Load<Song>("sounds/music/failed");

            this.gamePhysic = new World(Vector2.Zero);
            this.viewnDebug  = new DebugViewXNA(this.gamePhysic)
            {
                Flags = DebugViewFlags.DebugPanel | DebugViewFlags.PerformanceGraph | DebugViewFlags.Shape | DebugViewFlags.Joint
            };
            this.viewnDebug.LoadContent(this.ScreenManager.GraphicsDevice, this.ScreenManager.Content);

            this.randomizer = new Random(11043849 / DateTime.Now.Millisecond);

            lock (this.gamePhysic)
            {
                Viewport gameView = this.ScreenManager.GraphicsDevice.Viewport;

                this.projMatrix = Matrix.CreateOrthographicOffCenter(0,
                                                                     ConvertUnits.ToSimUnits(gameView.Width),
                                                                     ConvertUnits.ToSimUnits(gameView.Height),
                                                                     0,
                                                                     0,
                                                                     1);

                float w = ConvertUnits.ToSimUnits(gameView.Width - 1);
                float h = ConvertUnits.ToSimUnits(gameView.Height - 1);

                Vertices borderVerts = new Vertices(4);
                borderVerts.Add(new Vector2(0, 0));
                borderVerts.Add(new Vector2(0, h));
                borderVerts.Add(new Vector2(w, h));
                borderVerts.Add(new Vector2(w, 0));

                this.gameBorder = BodyFactory.CreateLoopShape(this.gamePhysic, borderVerts);
                this.gameBorder.CollisionCategories = Category.All;
                this.gameBorder.CollidesWith = Category.All;

                this.player = new DGPlayer();
                this.player.LoadContent(this.ScreenManager.Content, this.gamePhysic);

                MouseState state = Mouse.GetState();

                this.player.EntityLocation = new Vector2(state.X, state.Y);
            }

            Thread.Sleep(3000);

            base.LoadContent();
        }
        public      override void   UnloadContent           ()
        {
            this.player.UnloadContent();

            this.overMenu.UnloadContent();
            this.pausMenu.UnloadContent();

            this.viewnDebug.Dispose();
        }

        public      override void   HandleInput             (GameTime gametime, DGInput input)
        {
            if (!this.player.Dead())
            {
                if (this.pausInput.Evaluate(input))
                {
                    this.ScreenManager.AddScreen(this.pausMenu);
                    this.physicsDebugShown = false;
                }

#if DEBUG

                if (this.physicsInput.Evaluate(input))
                    this.physicsDebugShown = this.physicsDebugShown ? false : true;

#endif

                this.player.HandleInput(gametime, input);
            }
        }

        public      override void   Update                  (GameTime gametime)
        {
            //if (!this.isPlaying)
            //{
            //    this.readyTime += (float)gametime.ElapsedGameTime.TotalSeconds;

            //    if (this.readyTime > 3)
            //        this.isPlaying = true;

            //    return;
            //}

            this.gameBackground.Update(gametime);

            if (this.player.Dead() && !this.gameLost)
            {
                this.gameLost = true;
                this.OverrideInput = true;

                MediaPlayer.Play(this.overSong);

                this.ScreenManager.AddScreen(this.overMenu);
            }

            lock (this.gamePhysic)
            {
                this.gamePhysic.Step(Math.Min((float)gametime.ElapsedGameTime.TotalSeconds, 1 / 30f));

                this.UpdateEntityLifecycle(gametime);
                this.CleanupEntities(gametime);

                this.CreateEntities(gametime);
                this.UpdateEntities(gametime);

                this.player.Update(gametime);
            }
        }
        public      override void   Render                  (SpriteBatch batch)
        {
            this.gameBackground.Render(batch);

            foreach (DGRedCell cell in this.redCells)
                cell.Render(batch);

            foreach (DGPowCell cell in this.powCells)
                cell.Render(batch);

            foreach (DGWhtCell cell in this.whtCells)
                cell.Render(batch);

            if (!this.player.Dead())
                this.player.Render(batch);

#if DEBUG

            batch.End();

            lock (this.gamePhysic)
            {
                if (this.physicsDebugShown)
                    this.viewnDebug.RenderDebugData(ref this.projMatrix);
            }

            batch.Begin();

            batch.DrawString(this.debugFont, string.Format("Life: {0}", this.player.MaxLife - this.player.WastedLife), new Vector2(201, 401), Color.Black);
            batch.DrawString(this.debugFont, string.Format("Life: {0}", this.player.MaxLife - this.player.WastedLife), new Vector2(200, 400), Color.White);

            batch.DrawString(this.debugFont, string.Format("RED stack size: {0}", this.redCells.Count), new Vector2(51, 401), Color.Black);
            batch.DrawString(this.debugFont, string.Format("WHT stack size: {0}", this.whtCells.Count), new Vector2(51, 431), Color.Black);
            batch.DrawString(this.debugFont, string.Format("POW stack size: {0}", this.powCells.Count), new Vector2(51, 461), Color.Black);

            batch.DrawString(this.debugFont, string.Format("RED stack size: {0}", this.redCells.Count), new Vector2(50, 400), Color.White);
            batch.DrawString(this.debugFont, string.Format("WHT stack size: {0}", this.whtCells.Count), new Vector2(50, 430), Color.White);
            batch.DrawString(this.debugFont, string.Format("POW stack size: {0}", this.powCells.Count), new Vector2(50, 460), Color.White);

#endif
        }

        #endregion 

        #region METHODS

        private     void            UpdateEntityLifecycle   (GameTime gametime)
        {
            this.redElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            this.whtElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            this.powElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            this.totElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
        }
        private     void            CleanupEntities         (GameTime gametime)
        {
            for (int i = 0; i < this.redCells.Count; i++)
            {
                DGRedCell redCell = this.redCells[i];

                if (redCell.EntityDead)
                {
                    this.gamePhysic.RemoveBody(redCell.EntityPhysics);
                    this.redCells.RemoveAt(i);

                    if (this.redCells.Count < 30)
                    {
                        redCell = new DGRedCell(this.randomizer);
                        redCell.LoadContent(this.ScreenManager.Content, this.gamePhysic);

                        this.redCells.Add(redCell);
                    }
                }
            }

            for (int i = 0; i < this.whtCells.Count; i++)
            {
                DGWhtCell whtCell = this.whtCells[i];

                if (whtCell.EntityDead)
                {
                    this.gamePhysic.RemoveBody(whtCell.EntityPhysics);
                    this.whtCells.RemoveAt(i);

                    if (this.redCells.Count < 5)
                    {
                        whtCell = new DGWhtCell(this.randomizer);
                        whtCell.LoadContent(this.ScreenManager.Content, this.gamePhysic);

                        this.whtCells.Add(whtCell);
                    }
                }
            }

            for (int i = 0; i < this.powCells.Count; i++)
            {
                DGPowCell powCell = this.powCells[i];

                if (powCell.EntityDead)
                {
                    this.gamePhysic.RemoveBody(powCell.EntityPhysics);
                    this.powCells.RemoveAt(i);
                }
            }
        }
                                    
        private     void            CreateEntities          (GameTime gametime)
        {
            if (this.redElapsed >= (60 / 30) && this.redCells.Count < 15)
            {
                DGRedCell cell = new DGRedCell(this.randomizer);
                cell.LoadContent(this.ScreenManager.Content, this.gamePhysic);

                this.redCells.Add(cell);

                this.redElapsed = 0;
            }

            if (this.whtElapsed >= (60 / 10) && this.whtCells.Count < 3)
            {
                DGWhtCell cell = new DGWhtCell(this.randomizer);
                cell.LoadContent(this.ScreenManager.Content, this.gamePhysic);

                this.whtCells.Add(cell);

                this.whtElapsed = 0;
            }

            if (this.powElapsed >= (60 / 4) && this.powCells.Count < 2 && this.player.WastedLife > 1)
            {
                DGPowCell cell = new DGPowCell(this.randomizer);
                cell.LoadContent(this.ScreenManager.Content, this.gamePhysic);

                this.powCells.Add(cell);

                this.powElapsed = 0;
            }
        }
        private     void            UpdateEntities          (GameTime gametime)
        {            
            foreach (DGWhtCell cell in this.whtCells)
            {
                cell.Update(gametime);

                if (cell.EntityBounds.Intersects(this.player.EntityBounds) && !this.player.Dead())
                {
                    this.player.Damage();
                    cell.Damage();
                }
            }

            foreach (DGRedCell cell in this.redCells)
            {
                cell.Update(gametime);

                if (cell.EntityBounds.Intersects(this.player.EntityBounds) && !cell.CellInfected && !this.player.Dead())
                {
                    cell.Infect();
                }

                foreach (DGWhtCell wcell in this.whtCells)
                {
                    if (cell.EntityBounds.Intersects(wcell.EntityBounds) && cell.CellInfected)
                    {
                        cell.Damage();
                        wcell.Damage();
                    }
                }
            }

            foreach (DGPowCell cell in this.powCells)
            {
                cell.Update(gametime);

                if (cell.EntityBounds.Intersects(this.player.EntityBounds) && !this.player.Dead())
                {
                    cell.Eaten();
                    this.player.Healed();
                }
            }
        }

        #endregion
    }
}
