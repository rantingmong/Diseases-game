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
        public MouseState OldMouseState;

        public void Sync                ()
        {
            CurKeyboardState = Keyboard.GetState();
            OldKeyboardState = Keyboard.GetState();
        }

        public void Update              (GameTime gametime)
        {
            OldKeyboardState = CurKeyboardState;
            OldMouseState = CurMouseState;

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
            return rectangle.Contains(new Point(CurMouseState.X, CurMouseState.Y));
        }

        public bool TestLeftButton      ()
        {
            return CurMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released;
        }
        public bool TestRightButton     ()
        {
            return CurMouseState.RightButton == ButtonState.Pressed && OldMouseState.RightButton == ButtonState.Released;
        }
        public bool TestCenterButton    ()
        {
            return CurMouseState.MiddleButton == ButtonState.Pressed && OldMouseState.MiddleButton == ButtonState.Released;
        }
    }
}
