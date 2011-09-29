using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using Diseases.Input;
using Diseases.Entity;
using Diseases.Graphics;
using Diseases.Screen.Level;
using System.IO;

namespace Diseases.Screen.Menu
{
    public class MenuOver : DGMenuScreen
    {
        int scoreVal = 0;
        bool ishighscore = false;

        List<int> scores = new List<int>();

        SpriteFont gameFont;

        DGSpriteStatic background   = new DGSpriteStatic("backgrounds/menu/over");

        DGMenuEntry rtryEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/over/rtry"), new DGSpriteStatic("entities/menubuttons/over/rtry_selt"))
        {
            Location = new Vector2(200, 290)
        };
        DGMenuEntry mainEntry = new DGMenuEntry(new DGSpriteStatic("entities/menubuttons/over/main"), new DGSpriteStatic("entities/menubuttons/over/main_selt"))
        {
            Location = new Vector2(200, 350)
        };

        public MenuOver(int score)
            : base("game over")
        {
            this.scoreVal = score;

            if (File.Exists("scores.txt"))
                using (TextReader reader = new StreamReader(File.Open("scores.txt", FileMode.Open)))
                {
                    string line = "";

                    while ((line = reader.ReadLine()) != null)
                    {
                        this.scores.Add(Convert.ToInt32(line));
                    }
                }

            this.scores.Add(this.scoreVal);
            this.scores.Sort((m1, m2) =>
            {
                if (m2 > m1)
                    return 1;
                else if (m2 < m1)
                    return -1;
                else
                    return 0;
            });

            for (int i = 0; i < Math.Min(6, this.scores.Count); i++)
            {
                if (this.scores[i] == score)
                    this.ishighscore = true;
            }

            using (TextWriter writer = new StreamWriter(File.Open("scores.txt", FileMode.OpenOrCreate)))
            {
                foreach (int value in this.scores)
                {
                    writer.WriteLine(value);
                }
            }
        }

        protected override void Initialize()
        {
            this.BackgroundList.Add(this.background);

            this.MenuList.Add(this.rtryEntry);
            this.MenuList.Add(this.mainEntry);

            this.rtryEntry.Selected += (o, s) =>
            {
                this.ScreenManager.SwitchScreen(new LevelGamePlay());
            };
            this.mainEntry.Selected += (o, s) =>
            {
                this.ScreenManager.SwitchScreen(new MenuMain(true));
            };
        }

        public override void LoadContent()
        {
            this.gameFont = this.ScreenManager.Content.Load<SpriteFont>("fonts/gamefont");

            base.LoadContent();
        }

        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);

            Vector2 size = this.gameFont.MeasureString("YOUR SCORE: " + this.scoreVal + (this.ishighscore ? " (NEW HIGH SCORE!)" : ""));
            batch.DrawString(this.gameFont, "YOUR SCORE: " + this.scoreVal + (this.ishighscore ? " (NEW HIGH SCORE!)" : ""), new Vector2(400 - (size.X / 2), 230), Color.White);
        }

        protected override void OnCancel()
        {
            this.ScreenManager.SwitchScreen(new MenuMain());
        }
    }
}
