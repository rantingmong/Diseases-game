using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Graphics;

namespace Diseases.Screen.Menu
{
    public class MenuTutt : DGMenuScreen
    {
        int screenIndex = 0;

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/tutt");

        DGSpriteAnimat bactra = new DGSpriteAnimat("entities/bacteria/idle", 10, 10)
        {
            Location = new Vector2(350, 145)
        };
        DGSpriteAnimat target = new DGSpriteAnimat("entities/target/idle", 10, 12)
        {
            Location = new Vector2(510, 345)
        };
        DGSpriteStatic enemys = new DGSpriteStatic("entities/enemy/idle")
        {
            Location = new Vector2(540, 145)
        };
        DGSpriteAnimat pwerup = new DGSpriteAnimat("entities/powerup/idle", 10, 8)
        {
            Location = new Vector2(270, 150)
        };

        DGSpriteStatic slide1 = new DGSpriteStatic("entities/tutt/slide1")
        {
            Location = new Vector2(50, 100)
        };
        DGSpriteStatic slide2 = new DGSpriteStatic("entities/tutt/slide2")
        {
            Location = new Vector2(50, 100)
        };
        DGSpriteStatic slide3 = new DGSpriteStatic("entities/tutt/slide3")
        {
            Location = new Vector2(50, 100)
        };

        DGMenuEntry backEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/tutt/back"), new DGSpriteStatic("entities/menubuttons/tutt/back_selt"))
        {
            Location = new Vector2(555, 450)
        };
        DGMenuEntry nextEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/tutt/next"), new DGSpriteStatic("entities/menubuttons/tutt/next_selt"))
        {
            Location = new Vector2(650, 450)
        };

        public MenuTutt()
            : base("tutorial")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.backEntry);
            this.MenuList.Add(this.nextEntry);

            this.backEntry.Selected += (o, s) =>
            {
                this.screenIndex--;
            };
            this.nextEntry.Selected += (o, s) =>
            {
                this.screenIndex++;
            };
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.slide1.LoadContent(this.ScreenManager.Content);
            this.slide2.LoadContent(this.ScreenManager.Content);
            this.slide3.LoadContent(this.ScreenManager.Content);

            this.target.LoadContent(this.ScreenManager.Content);
            this.bactra.LoadContent(this.ScreenManager.Content);
            this.enemys.LoadContent(this.ScreenManager.Content);
            this.pwerup.LoadContent(this.ScreenManager.Content);
        }

        public override void Update(GameTime gametime)
        {
            switch (this.screenIndex)
            {
                case -1:
                case 3:
                    {
                        this.ScreenManager.RemoveScreen(this);
                    }
                    break;
                case 0:
                    {
                        this.slide1.Update(gametime);

                        this.bactra.Update(gametime);
                        this.target.Update(gametime);
                    }
                    break;
                case 1:
                    {
                        this.slide2.Update(gametime);

                        this.enemys.Update(gametime);
                    }
                    break;
                case 2:
                    {
                        this.slide3.Update(gametime);

                        this.pwerup.Update(gametime);
                    }
                    break;
            }

            base.Update(gametime);
        }
        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);

            switch (this.screenIndex)
            {
                case 0:
                    {
                        this.slide1.Render(batch);

                        this.bactra.Render(batch);
                        this.target.Render(batch);
                    }
                    break;
                case 1:
                    {
                        this.slide2.Render(batch);

                        this.enemys.Render(batch);
                    }
                    break;
                case 2:
                    {
                        this.slide3.Render(batch);

                        this.pwerup.Render(batch);
                    }
                    break;
            }
        }

        protected override void OnCancel()
        {
            this.ScreenManager.RemoveScreen(this);
        }
    }
}
