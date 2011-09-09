using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Diseases.Input
{
    public class DGInputAction
    {
        internal string inputname;

        public readonly Keys key;
        public readonly bool newPressOnly;

        private delegate bool KeyPress(Keys key);

        public DGInputAction(Keys keys, bool newPressOnly)
        {
            this.key = keys;
            this.newPressOnly = newPressOnly;
        }

        public bool Evaluate(DGInput input)
        {
            bool returnvalue = true;
            KeyPress keypress;

            if (this.newPressOnly)
                keypress = input.TestKeyPress;
            else
                keypress = input.TestKeyDown;

            returnvalue = keypress(key);

            if (returnvalue)
                Debug.WriteLine("input invoked! " + this.inputname, "INFO");

            return returnvalue;
        }
    }
}