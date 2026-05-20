using WinFormsApp1.Controller;
using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.World;
using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Services
{
    public static class CollisionService
    {
        public static ControllerDirection.CollisionInfo CheckPlatformCollision(Player player, Level level)
        {
            var currTop = player.Y;
            var currLeft = player.X;
            var currRight = player.X + player.Width;
            var currBottom = player.Y + player.Height;

            var prevTop = player.Y - player.VelocityY;
            var prevLeft = player.X - player.VelocityX;
            var prevRight = prevLeft + player.Width;
            var prevBottom = prevTop + player.Height;

            foreach (var platform in level.Platforms)
            {
                var platformLeft = platform.X;
                var platformRight = platform.X + platform.Width;
                var platformTop = platform.Y;
                var platformBottom = platform.Y + platform.Height;

                var conditionWithoutCollision = new List<bool>
                {
                    prevRight < platformLeft && currRight < platformLeft,
                    prevLeft > platformRight && currLeft > platformRight,
                    prevBottom < platformTop && currBottom < platformTop,
                    prevTop > platformBottom && currTop > platformBottom
                };

                if (conditionWithoutCollision.Exists(x => x))
                    continue;

                var xOverlap = (currRight > platformLeft && currLeft < platformRight) ||
                               (prevRight > platformLeft && prevLeft < platformRight);
                var yOverlap = (currBottom > platformTop && currTop < platformBottom) ||
                               (prevBottom > platformTop && prevTop < platformBottom);

                if (xOverlap)
                {
                    if (prevBottom <= platformTop && currBottom >= platformTop)
                    {
                        float adjustY = platformTop - currBottom;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Top, 0, adjustY);
                    }

                    if (prevTop >= platformBottom && currTop <= platformBottom)
                    {
                        float adjustY = platformBottom - currTop;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Bottom, 0, adjustY);
                    }
                }

                if (yOverlap)
                {
                    if (prevRight <= platformLeft && currRight >= platformLeft)
                    {
                        float adjustX = platformLeft - currRight;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Left, adjustX, 0);
                    }

                    if (prevLeft >= platformRight && currLeft <= platformRight)
                    {
                        float adjustX = platformRight - currLeft;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Right, adjustX, 0);
                    }
                }
            }

            return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.None, 0, 0);
        }

        public static bool Intersects(GameObject firstObject, GameObject secondObject)
        {
            if (firstObject == null || secondObject == null)
                return false;
            var firstRect = firstObject.GetBounds();
            var secondRect = secondObject.GetBounds();
            return firstRect.IntersectsWith(secondRect);
        }

        public static List<T> GetCollisions<T>(Player player, IEnumerable<T> objects) where T : GameObject
        {
            var result = new List<T>();
            if (player == null || objects == null)
                return result;

            var playerRect = player.GetBounds();
            foreach (var obj in objects)
            {
                if (obj == null)
                    continue;

                if (playerRect.IntersectsWith(obj.GetBounds()))
                    result.Add(obj);
            }

            return result;
        }
    }
}