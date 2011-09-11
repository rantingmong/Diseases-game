using System;
using System.Collections.Generic;

using System.IO;

namespace Diseases.Util
{
    public struct DGSettings
    {
        public static readonly int MAXRBC = 30;
        public static readonly int MAXWBC = 10;
        public static readonly int MAXLIF = 3;

        private int                 maxRBC;
        private int                 maxWBC;
        private int                 maxLIF;

        private bool                sound;
        private bool                music;

        public  int                 MaxRBC
        {
            get { return this.maxRBC; }
            set { this.maxRBC = value; }
        }
        public  int                 MaxWBC
        {
            get { return this.maxWBC; }
            set { this.maxWBC = value; }
        }
        public  int                 MaxLIF
        {
            get { return this.maxLIF; }
            set { this.maxLIF = value; }
        }

        public  bool                Music
        {
            get { return this.music; }
            set { this.music = value; }
        }
        public  bool                Sound
        {
            get { return this.sound; }
            set { this.sound = value; }
        }

        public  static void          SaveSettings    (Stream otStream, DGSettings info)
        {
            using (TextWriter writ = new StreamWriter(otStream))
            {
                writ.WriteLine(string.Format("maxRBC={0}", info.maxRBC));
                writ.WriteLine(string.Format("maxWBC={0}", info.maxWBC));
                writ.WriteLine(string.Format("maxLIF={0}", info.maxLIF));
                writ.WriteLine(string.Format("sounds={0}", info.sound));
                writ.WriteLine(string.Format("musics={0}", info.music));
            }
        }
        public  static DGSettings    OpenSettings    (Stream inStream)
        {
            DGSettings returnval = new DGSettings();

            using (TextReader read = new StreamReader(inStream))
            {
                returnval.maxRBC = Convert.ToInt32(read.ReadLine().Split('=')[1]);
                returnval.maxWBC = Convert.ToInt32(read.ReadLine().Split('=')[1]);
                returnval.maxLIF = Convert.ToInt32(read.ReadLine().Split('=')[1]);
                returnval.music = Convert.ToBoolean(read.ReadLine().Split('=')[1]);
                returnval.sound = Convert.ToBoolean(read.ReadLine().Split('=')[1]);
            }

            return returnval;
        }

        public  static DGSettings    DefaultInfo     ()
        {
            return new DGSettings()
            {
                maxLIF = DGSettings.MAXLIF,
                maxRBC = DGSettings.MAXRBC,
                maxWBC = DGSettings.MAXWBC,
                music = true,
                sound = true
            };
        }
    }
}
