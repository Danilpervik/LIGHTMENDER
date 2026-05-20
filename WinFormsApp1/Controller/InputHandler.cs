namespace WinFormsApp1.Controller
{
    public class InputHandler
    {
        private bool isLeftPressed;
        private bool isRightPressed;
        private bool isJumpPressed;

        public void KeyDown(Keys key)
        {
            KeyDownOrUp(key, true);
        }

        public void KeyUp(Keys key)
        {
            KeyDownOrUp(key, false);
        }

        private void KeyDownOrUp(Keys key, bool isPressed)
        {
            switch (key)
            {   
                case Keys.A:
                case Keys.Left:
                    isLeftPressed = isPressed;
                    break;
                case Keys.D:
                case Keys.Right:
                    isRightPressed = isPressed;
                    break;
                case Keys.W:
                case Keys.Up:
                case Keys.Space:
                    isJumpPressed = isPressed;
                    break;
            }
        }

        public bool IsLeftPressed()
        { 
            return isLeftPressed;
        }

        public bool IsRightPressed()
        {
            return isRightPressed;
        }

        public bool IsJumpPressed()
        {
            return isJumpPressed;
        }

        public void Reset()
        {
            isLeftPressed = false;
            isRightPressed = false;
            isJumpPressed = false;
        }
    }
}