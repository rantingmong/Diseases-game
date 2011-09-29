using System;
using System.Collections.Generic;

using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Graphics;

namespace Diseases.Screen.Menu
{
    public class MenuHigh : DGMenuScreen
    {
        List<int> scores = new List<int>();

        SpriteFont font;

        DGSpriteStatic background = new DGSpriteStatic("backgrounds/menu/high");

        DGMenuEntry backEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/scor/back"), new DGSpriteStatic("entities/menubuttons/scor/back_selt"))
        {
            Location = new Vector2(20, 490)
        };
        DGMenuEntry rsetEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/scor/rset"), new DGSpriteStatic("entities/menubuttons/scor/rset_selt"))
        {
            Location = new Vector2(220, 490)
        };

        public MenuHigh()
            : base("high scores")
        {

        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.backEntry);

            this.backEntry.Selected += (o, s) => //x
            {
                this.ScreenManager.RemoveScreen(this);
            };

            if (!File.Exists("scores.txt"))
                File.Create("scores.txt");

            using (TextReader reader = new StreamReader(File.OpenRead("scores.txt")))
            {
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    this.scores.Add(Convert.ToInt32(line));
                }
            }

            this.scores.Sort((m1, m2) =>
            {
                if (m2 > m1)
                    return 1;
                else if (m2 < m1)
                    return -1;
                else
                    return 0;
            });
        }
        protected override void OnCancel()
        {
            this.ScreenManager.RemoveScreen(this);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.font = this.ScreenManager.Content.Load<SpriteFont>("fonts/gamefont");
        }

        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);

            Vector2 position = new Vector2(60, 100);

            for (int i = 0; i < Math.Min(6, this.scores.Count); i++)
            {
                batch.DrawString(this.font, this.scores[i].ToString(), position, Color.Black);
                position.Y += 50;
            }
        }
    }
}
