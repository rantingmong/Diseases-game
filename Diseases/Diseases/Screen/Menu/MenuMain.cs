using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Physics;
using Diseases.Graphics;
using Diseases.Screen.Level;
using System.Threading;
using Microsoft.Xna.Framework.Media;

namespace Diseases.Screen.Menu
{
    public class MenuMain : DGMenuScreen
    {
        #region FIELDS

        bool                        longload            = false;

        bool                        backPlayed          = false;
        Song                        backSong;

        World                       physics             = new World(Vector2.Zero);
        DGRedCell[]                 redCells            = new DGRedCell[20];
        
        DGMenuEntry                 playEntry           = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/main/play"), new DGSpriteStatic("entities/menubuttons/main/play_selt"))
        {
            Location = new Vector2(400, 140)
        };
        DGMenuEntry                 scorEntry           = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/main/high"), new DGSpriteStatic("entities/menubuttons/main/high_selt"))
        {
            Location = new Vector2(400, 190)
        };
        DGMenuEntry                 tuttEntry           = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/main/tutt"), new DGSpriteStatic("entities/menubuttons/main/tutt_selt"))
        {
            Location = new Vector2(400, 240)
        };

        DGSpriteStatic              gameBackground      = new DGSpriteStatic("backgrounds/menu/mainBack");
        DGSpriteStatic              backgroundHolder    = new DGSpriteStatic("backgrounds/menu/main");

        #endregion

        #region CTOR

        public                      MenuMain            ()
            : base("main")
        {

        }
        public                      MenuMain            (bool loading)
            : base("main")
        {
            this.longload = loading;
        }

        #endregion

        #region OVERRIDES

        protected   override void   Initialize          ()
        {
            this.BackgroundList.Add(backgroundHolder);

            this.MenuList.Add(playEntry);
            this.MenuList.Add(scorEntry);
            this.MenuList.Add(tuttEntry);

            this.playEntry.Selected += (s, o) =>
            {
                this.ScreenManager.SwitchScreen(new LevelGamePlay());
            };
            this.scorEntry.Selected += (s, o) =>
            {
                this.ScreenManager.AddScreen(new MenuHigh());
            };
            this.tuttEntry.Selected += (s, o) =>
            {
                this.ScreenManager.AddScreen(new MenuTutt());
            };
        }

        public      override void   LoadContent         ()
        {
            this.backSong = this.ScreenManager.Content.Load<Song>("sounds/music/main");

            Viewport gameView = this.ScreenManager.GraphicsDevice.Viewport;

            float w = ConvertUnits.ToSimUnits(gameView.Width - 1);
            float h = ConvertUnits.ToSimUnits(gameView.Height - 1);

            Vertices borderVerts = new Vertices(4);
            borderVerts.Add(new Vector2(0, 0));
            borderVerts.Add(new Vector2(0, h));
            borderVerts.Add(new Vector2(w, h));
            borderVerts.Add(new Vector2(w, 0));

            Body body = BodyFactory.CreateLoopShape(this.physics, borderVerts);
            body.CollisionCategories = Category.All;
            body.CollidesWith = Category.All;

            Random randomizer = new Random(11041846 / DateTime.Now.Millisecond);

            for (int i = 0; i < this.redCells.Length; i++)
            {
                this.redCells[i] = new DGRedCell(randomizer);
                this.redCells[i].LoadContent(this.ScreenManager.Content, this.physics);
            }

            this.gameBackground.LoadContent(this.ScreenManager.Content);

            if (this.longload)
                Thread.Sleep(2000);

            MediaPlayer.Volume = 0.5f;

            base.LoadContent();
        }
        public      override void   UnloadContent       ()
        {
            for (int i = 0; i < this.redCells.Length; i++)
            {
                this.redCells[i].UnloadContent();
                this.redCells[i] = null;
            }

            base.UnloadContent();
        }

        public      override void   Update              (GameTime gametime)
        {
            base.Update(gametime);

            if (!this.backPlayed)
            {
                this.backPlayed = true;

                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(this.backSong);
            }

            this.physics.Step(Math.Min((float)gametime.ElapsedGameTime.TotalSeconds, 1 / 30f));

            foreach (DGRedCell cell in this.redCells)
                cell.Update(gametime);

            this.gameBackground.Update(gametime);
        }
        public      override void   Render              (SpriteBatch batch)
        {
            this.gameBackground.Render(batch);

            foreach (DGRedCell cell in this.redCells)
                cell.Render(batch);

            base.Render(batch);
        }

        protected   override void   OnCancel            ()
        {
            this.ScreenManager.AddScreen(new MenuQuit());
        }
        protected   override void   OnSelect            (int entryIndex)
        {
            try
            {
                base.OnSelect(entryIndex);
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
