using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Diseases.Input
{
    public class DGInputSequence
    {
        internal string inputname;

        bool complete;
        public bool CompleteSequence
        {
            get { return this.complete; }
            set { this.complete = value; }
        }

        public readonly Keys[] keys;
        public readonly bool newPressOnly;

        private delegate bool KeyPress(Keys key);

        public DGInputSequence(Keys[] keys, bool newPressOnly, bool complete)
        {
            this.keys = keys != null ? keys.Clone() as Keys[] : new Keys[0];
            this.newPressOnly = newPressOnly;

            this.complete = complete;
        }

        public bool Evaluate(DGInput input)
        {
            bool returnvalue = true;
            KeyPress keypress;

            if (this.newPressOnly)
                keypress = input.TestKeyPress;
            else
                keypress = input.TestKeyDown;

            foreach (Keys key in keys)
            {
                if (complete)
                    returnvalue = keypress(key) && returnvalue;
                else if (keypress(key))
                    return true;
            }

            if (returnvalue)
                Debug.WriteLine("input invoked! " + this.inputname, "INFO");

            if (!complete)
                return false;

            return returnvalue;
        }
    }
}
