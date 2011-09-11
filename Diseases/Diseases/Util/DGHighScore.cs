using System;
using System.Collections.Generic;

using System.IO;

namespace Diseases.Util
{
    public struct DGHighScore
    {
        List<int>                   scores;
        public List<int>            Scores
        {
            get { return this.scores; }
            set { this.scores = value; }
        }

        public static void          SaveScores  (Stream otstream, DGHighScore info)
        {
            using (TextWriter writ = new StreamWriter(otstream))
            {
                for (int i = 0; i < (info.scores.Count > 10 ? info.scores.Count : 10); i++)
                {
                    writ.WriteLine(info.scores[i]);
                }
            }
        }
        public static DGHighScore   OpenScores  (Stream instream)
        {
            DGHighScore returnval = new DGHighScore();

            using (TextReader read = new StreamReader(instream))
            {
                string line = "";

                while ((line == read.ReadLine()) != null)
                {
                    returnval.scores.Add(Convert.ToInt32(line));
                }
            }

            return returnval;
        }
    }
}
