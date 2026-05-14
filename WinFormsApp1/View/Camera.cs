namespace WinFormsApp1.View
{
    public class Camera
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public Camera(int viewportWidth, int viewportHeight)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            X = 0;
            Y = 0;
        }

        public void Follow(float targetX, float targetY)
        {
            X = targetX - ViewportWidth / 2;
            Y = targetY - ViewportHeight / 2;
        }

        public PointF Transform(float worldX, float worldY)
        {
            var screenX = worldX - X;
            var screenY = worldY - Y;
            return new PointF(screenX, screenY);
        }

        public RectangleF Transform(RectangleF worldRect)
        {
            var screenX = worldRect.X - X;
            var screenY = worldRect.Y - Y;
            return new RectangleF(screenX, screenY, worldRect.Width, worldRect.Height);
        }

        public bool IsVisible(RectangleF worldRect)
        {
            var screenRect = Transform(worldRect);
            return screenRect.Right > 0 && screenRect.Left < ViewportWidth &&
                   screenRect.Bottom > 0 && screenRect.Top < ViewportHeight;
        }
    }
}