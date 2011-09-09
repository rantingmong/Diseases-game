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

        public MouseState CurMouseState;

        public void Sync                ()
        {
            CurKeyboardState = Keyboard.GetState();
            OldKeyboardState = Keyboard.GetState();
        }

        public void Update              (GameTime gametime)
        {
            OldKeyboardState = CurKeyboardState;

            CurKeyboardState = Keyboard.GetState();
            CurMouseState = Mouse.GetState();
        }

        public bool TestKeyRleas        (Keys key)
        {
            return OldKeyboardState.IsKeyDown(key) && CurKeyboardState.IsKeyUp(key);
        }
        public bool TestKeyPress        (Keys key)
        {
            return OldKeyboardState.IsKeyUp(key) && CurKeyboardState.IsKeyDown(key);
        }

        public bool TestKeyUp           (Keys key)
        {
            return CurKeyboardState.IsKeyUp(key);
        }
        public bool TestKeyDown         (Keys key)
        {
            return CurKeyboardState.IsKeyDown(key);
        }

        public bool TestLocation        (Rectangle rectangle)
        {
            return (rectangle.Left <= CurMouseState.X) && (rectangle.Right >= CurMouseState.X) ||
                    (rectangle.Top <= CurMouseState.X) && (rectangle.Bottom >= CurMouseState.Y);
        }

        public bool TestLeftButton      ()
        {
            return CurMouseState.LeftButton == ButtonState.Pressed ? true : false;
        }
        public bool TestRightButton     ()
        {
            return CurMouseState.RightButton == ButtonState.Pressed ? true : false;
        }
        public bool TestCenterButton    ()
        {
            return CurMouseState.MiddleButton == ButtonState.Pressed ? true : false;
        }
    }
}
