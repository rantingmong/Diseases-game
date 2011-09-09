using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Diseases.Input
{
    public enum MouseButton
    {
        Left,
        Center,
        Right
    }

    public class DGInputPoint
    {
        internal string inputname;

        private bool ishothit = false;

        public readonly Rectangle point;
        public readonly MouseButton button;

        public DGInputPoint(Rectangle hitLocation, MouseButton button)
        {
            this.point = hitLocation;
            this.button = button;
        }

        public bool Evaluate(DGInput input)
        {
            bool returnvalue = true;

            switch (button)
            {
                case MouseButton.Center:
                    returnvalue = input.TestCenterButton();
                    break;
                case MouseButton.Left:
                    returnvalue = input.TestLeftButton();
                    break;
                case MouseButton.Right:
                    returnvalue = input.TestRightButton();
                    break;
            }

            return returnvalue && input.TestLocation(point);
        }
    }
}
