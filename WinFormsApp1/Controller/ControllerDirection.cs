namespace WinFormsApp1.Controller
{
    public static class ControllerDirection
    {
        public enum Direction
        {
            None,
            Top,
            Bottom,
            Left,
            Right
        }

        public struct CollisionInfo
        {
            public Direction Direction;
            public float AdjustX;
            public float AdjustY;

            public CollisionInfo(Direction direction, float adjustX, float adjustY)
            {
                Direction = direction;
                AdjustX = adjustX;
                AdjustY = adjustY;
            }
        }
    }
}