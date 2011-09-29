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
        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/tutt");

        DGSpriteAnimat player = new DGSpriteAnimat("entities/bacteria/idle", 10, 10);
        DGSpriteAnimat target = new DGSpriteAnimat("entities/target/idle", 10, 12);
        DGSpriteAnimat powerp = new DGSpriteAnimat("entities/powerup/idle", 10, 8);

        DGMenuEntry backEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/scor/back"), new DGSpriteStatic("entities/menubuttons/scor/back_selt"))
        {
            Location = new Vector2(20, 490)
        };

        public MenuTutt()
            : base("tutorial")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.backEntry);

            this.backEntry.Selected += (o, s) =>
            {
                this.ScreenManager.RemoveScreen(this);
            };
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.player.Location = new Vector2(135, 110);
            this.target.Location = new Vector2(135, 185);
            this.powerp.Location = new Vector2(142, 335);

            this.player.LoadContent(this.ScreenManager.Content);
            this.target.LoadContent(this.ScreenManager.Content);
            this.powerp.LoadContent(this.ScreenManager.Content);
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            this.player.Update(gametime);
            this.target.Update(gametime);
            this.powerp.Update(gametime);
        }
        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);

            this.player.Render(batch);
            this.target.Render(batch);
            this.powerp.Render(batch);
        }

        protected override void OnCancel()
        {
            this.ScreenManager.RemoveScreen(this);
        }
    }
}
