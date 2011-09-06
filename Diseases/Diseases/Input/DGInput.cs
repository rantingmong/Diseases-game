using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Diseases.Input
{
    public class DGInput
    {
        public KeyboardState CurKeyboardState;
        public KeyboardState OldKeyboardState;

        public void Synch           ()
        {
            CurKeyboardState = Keyboard.GetState();
            OldKeyboardState = Keyboard.GetState();
        }

        public void Update          (GameTime gametime)
        {
            OldKeyboardState = CurKeyboardState;

            CurKeyboardState = Keyboard.GetState();
        }

        public bool TestKeyRleas    (Keys key)
        {
            return OldKeyboardState.IsKeyDown(key) && CurKeyboardState.IsKeyUp(key);
        }
        public bool TestKeyPress    (Keys key)
        {
            return OldKeyboardState.IsKeyUp(key) && CurKeyboardState.IsKeyDown(key);
        }

        public bool TestKeyUp       (Keys key)
        {
            this.Synch();

            return CurKeyboardState.IsKeyUp(key);
        }
        public bool TestKeyDown     (Keys key)
        {
            this.Synch();

            return CurKeyboardState.IsKeyDown(key);
        }
    }
}
