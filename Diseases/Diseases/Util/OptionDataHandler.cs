using System;
using System.IO;
using System.Collections.Generic;

namespace Diseases.Util
{
    public enum GameMode
    {
        Endless,
        Arcade,
        Normal,
        Extreme,
    }

    public class OptionDataHandler
    {
        GameMode gamemode;
        public GameMode     GameMode
        {
            get { return this.gamemode; }
            set { this.gamemode = value; }
        }

        bool sounds;
        public bool         Sounds
        {
            get { return this.sounds; }
            set { this.sounds = value; }
        }

        bool musics;
        public bool         Musics
        {
            get { return this.sounds; }
            set { this.sounds = value; }
        }


        public static void  WriteInfo   (Stream stream, ref OptionDataHandler info)
        {
            using (TextWriter writ = new StreamWriter(stream))
            {
                writ.WriteLine(string.Format("gamemode={0}", Enum.GetName(typeof(GameMode), info.gamemode)));
                writ.WriteLine(string.Format("music={0}", info.musics));
                writ.WriteLine(string.Format("sound={0}", info.sounds));
            }
        }
        public static void  PlaceInfo   (Stream stream, out OptionDataHandler info)
        {
            OptionDataHandler outinfo = new OptionDataHandler();

            using (TextReader read = new StreamReader(stream))
            {
                Enum.TryParse<GameMode>(read.ReadLine().Split('=')[1], out outinfo.gamemode);
                outinfo.musics = Convert.ToBoolean(read.ReadLine().Split('=')[1]);
                outinfo.sounds = Convert.ToBoolean(read.ReadLine().Split('=')[1]);
            }

            info = outinfo;
        }
        public static void  SetDefaults (out OptionDataHandler info)
        {
            OptionDataHandler outinfo = new OptionDataHandler();

            outinfo.gamemode    = GameMode.Normal;
            outinfo.musics      = true;
            outinfo.sounds      = true;

            info = outinfo;
        }
    }
}
